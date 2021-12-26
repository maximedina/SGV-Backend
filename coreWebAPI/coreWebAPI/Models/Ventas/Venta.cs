using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Items;
using MOM.Core.Models.Security;
using MOM.Core.WebAPI.Models.cajas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.Ventas
{
    public class Venta
    {
        public Int64 id { get; set; }
        public DateTime fechaHora { get; set; }
        public int userId { get; set; }
        public double total { get; set; }
        public double descuento { get; set; }
        public string ticketTarjeta { get; set; }
        public int tipoPagoId { get; set; }
        public int idCaja { get; set; }
        public string Estatus { get; set; }
        public User Cliente { get; set; }
        public List<DetalleVenta> detalleVenta { get; set; }
        private CoreDbContext _coreDbContext;
        public Venta(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }
        public void registrar(Venta venta)
        {
            Caja caja = _coreDbContext.Cajas.Where(obj => obj.Id == venta.idCaja && obj.Estatus == true).FirstOrDefault();
            if (caja == null)
            {
                throw new ValidationException("No tienes ninguna caja abierta");
            }
            else
            {
                using (var context = _coreDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        OperationLog oper = new OperationLog();
                        oper.UserId = venta.userId;
                        Permission permisos = _coreDbContext.Permissions.Where(p => p.Name.ToLower() == "GESTION_VENTA").FirstOrDefault();
                        if (permisos != null)
                        {
                            oper.PermissionId = permisos.PermissionId;
                            oper.OperationDateTime = DateTime.Now;
                            oper.Description = "Crea venta";
                            _coreDbContext.Entry(oper).State = EntityState.Added;
                            _coreDbContext.OperationLog.Add(oper);
                            _coreDbContext.SaveChanges();
                        }

                        venta.fechaHora = DateTime.Now;
                        if(venta.id > 0)
                        {
                            _coreDbContext.Update(venta);
                            _coreDbContext.SaveChanges();
                        }
                        else
                        {
                            _coreDbContext.Entry(venta).State = EntityState.Added;
                            _coreDbContext.Ventas.Add(venta);
                            _coreDbContext.SaveChanges();
                        }

                        if(venta.Estatus == "cerrada")
                        {
                            caja.Ingresos += (decimal)venta.total;
                            _coreDbContext.Update(caja);
                            _coreDbContext.SaveChanges();

                            Movimiento movimiento = new Movimiento(_coreDbContext);
                            movimiento.Comentario = "Venta";
                            movimiento.FecMovimiento = venta.fechaHora;
                            movimiento.idCaja = caja.Id;
                            movimiento.idMotivo = 1;
                            movimiento.TipoMovimiento = "I";
                            movimiento.Importe = (decimal)venta.total;
                            movimiento.TipoPago = venta.tipoPagoId.ToString();
                            movimiento.NroComprobante = venta.ticketTarjeta;

                            _coreDbContext.Entry(movimiento).State = EntityState.Added;
                            _coreDbContext.MovimientosCaja.Add(movimiento);
                            _coreDbContext.SaveChanges();

                            MovCajaVta mcv = new MovCajaVta();
                            mcv.idVenta = venta.id;
                            mcv.idMovimiento = movimiento.idMovimiento;

                            _coreDbContext.Entry(mcv).State = EntityState.Added;
                            _coreDbContext.MovCaja_venta.Add(mcv);
                            _coreDbContext.SaveChanges();

                            if (venta.tipoPagoId == 4)
                            {
                                User user = _coreDbContext.Users.Find(venta.userId);
                                if (user != null)
                                {
                                    user.cuentaCorriente -= venta.total;
                                    if (user.cuentaCorriente >= 0)
                                    {
                                        _coreDbContext.Update(user);
                                        _coreDbContext.SaveChanges();
                                    }
                                    else
                                    {
                                        throw new ValidationException("El cliente no cuenta con saldo suficiente");
                                    }
                                }
                                else
                                {
                                    throw new ValidationException("No se encontro el usuario");
                                }
                            }
                        }
                        foreach (var detvta in venta.detalleVenta)
                        {
                            Item item = _coreDbContext.Items.Find(detvta.itemId);
                            if(item != null)
                            {
                                if (detvta.id > 0)
                                {
                                    _coreDbContext.Update(detvta);
                                    _coreDbContext.SaveChanges();
                                }
                                else
                                {
                                    detvta.ventaId = venta.id;
                                    _coreDbContext.Entry(detvta).State = EntityState.Added;
                                    _coreDbContext.DetalleVentas.Add(detvta);
                                    _coreDbContext.SaveChanges();
                                }

                                if (venta.Estatus == "cerrada")
                                {
                                    item.Stock -= detvta.cantidad;
                                    if (item.Stock >= 0)
                                    {
                                        _coreDbContext.Update(item);
                                        _coreDbContext.SaveChanges();
                                    }
                                    else
                                    {
                                        throw new ValidationException("El item con id " + detvta.itemId + " no cuenta con suficiente inventario");
                                    }
                                }
                            }
                            else
                            {
                                throw new ValidationException("No se encontro el item con id "+ detvta.itemId);
                            }
                        }
                        context.Commit();
                    }
                    catch(Exception ex)
                    {
                        context.Rollback();
                        throw new ValidationException(ex.Message);
                    }
                }
                
            }
        }
        public List<Venta> buscar(string fecIni,string fecFin)
        {
            DateTime fini = DateTime.Parse(fecIni+"T00:00:00");
            DateTime ffin = DateTime.Parse(fecIni + "T23:59:59");
            if (fecFin != null)
            {
                ffin = DateTime.Parse(fecFin + "T23:59:59");
            }
            
            return _coreDbContext.Ventas.Where(v => v.fechaHora >= fini && v.fechaHora <= ffin).Include(v => v.Cliente).Include(v=>v.detalleVenta).ThenInclude(d=>d.item).ToList();
        }

        public Venta Get(Int64 idVta)
        {
            return _coreDbContext.Ventas.Include(v => v.Cliente)
                    .Include(v => v.detalleVenta)
                    .ThenInclude(d => d.item)
                    .FirstOrDefault(v=>v.id == idVta);
        }

        public bool Validate(int userid)
        {
            return _coreDbContext.Cajas.Where(obj => obj.userId == userid && obj.Estatus == true).Any();
        }
    }
}
