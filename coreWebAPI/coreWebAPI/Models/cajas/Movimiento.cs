using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Items;
using MOM.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.cajas
{
    public class Movimiento
    {
        [Key]
		public int idMovimiento { get; set; }
		public int idCaja { get; set; }
		public DateTime FecMovimiento { get; set; }
		public String TipoMovimiento { get; set; }
		public decimal Importe { get; set; }
		public int idMotivo { get; set; }
		public string Comentario { get; set; }
		public string TipoPago { get; set; }
		public string NroComprobante { get; set; }
        public int idCliente { get; set; }
        public int idProveedor { get; set; }
        private CoreDbContext _coreDbContext;
        public Movimiento(CoreDbContext coreDbContext)
		{
			_coreDbContext = coreDbContext;
		}
        public void Add(Movimiento movimiento)
        {
            Caja caja = _coreDbContext.Cajas.Where(c => c.Id == movimiento.idCaja && c.Estatus == true).FirstOrDefault();
            if (caja == null)
            {
                throw new ValidationException("No tienes abierta la caja");
            }
            else
            {
                using (var context = _coreDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        OperationLog oper = new OperationLog();
                        oper.UserId = caja.userId;

                        if (movimiento.TipoMovimiento.Equals("I"))
                        {
                            caja.Ingresos += movimiento.Importe;
                            oper.Description = "Ingreso";
                        }
                        else
                        {
                            caja.Egresos += movimiento.Importe;
                            oper.Description = "Egreso";
                        }

                        Permission permisos = _coreDbContext.Permissions.Where(p => p.Name.ToLower() == "GESTION_VENTA").FirstOrDefault();
                        if (permisos != null)
                        {
                            oper.PermissionId = permisos.PermissionId;
                            oper.OperationDateTime = DateTime.Now;
                            
                            _coreDbContext.Entry(oper).State = EntityState.Added;
                            _coreDbContext.OperationLog.Add(oper);
                            _coreDbContext.SaveChanges();
                        }

                        _coreDbContext.Update(caja);
                        _coreDbContext.SaveChanges();

                        movimiento.FecMovimiento = DateTime.Now;
                        _coreDbContext.Entry(movimiento).State = EntityState.Added;
                        _coreDbContext.MovimientosCaja.Add(movimiento);
                        _coreDbContext.SaveChanges();

                        if (movimiento.idCliente != 0)
                        {
                            User user = _coreDbContext.Users.Find(movimiento.idCliente);
                            if (user != null)
                            {
                                user.cuentaCorriente += (double)movimiento.Importe;
                                _coreDbContext.Entry(user).State = EntityState.Added;
                                _coreDbContext.Users.Update(user);
                                _coreDbContext.SaveChanges();

                                MCCCliente cccte = new MCCCliente();
                                cccte.idMovimientoCaja = movimiento.idMovimiento;
                                cccte.clienteId = user.UserId;
                                cccte.Importe = movimiento.Importe;
                                cccte.TipoMovimiento = movimiento.TipoMovimiento;
                                _coreDbContext.Entry(cccte).State = EntityState.Added;
                                _coreDbContext.MovCCCliente.Add(cccte);
                                _coreDbContext.SaveChanges();

                            }
                            else
                            {
                                throw new ValidationException("No se encontro el cliente");
                            }
                        }
                        if (movimiento.idProveedor != 0)
                        {
                            Proveedor proveedor = _coreDbContext.Proveedores.Find(movimiento.idProveedor);
                            if (proveedor != null)
                            {
                                proveedor.CuentaCorriente += movimiento.Importe;
                                _coreDbContext.Entry(proveedor).State = EntityState.Added;
                                _coreDbContext.Proveedores.Update(proveedor);
                                _coreDbContext.SaveChanges();

                                MCCProveedor ccp = new MCCProveedor();
                                ccp.idMovimientoCaja = movimiento.idMovimiento;
                                ccp.proveedorId = proveedor.Id;
                                ccp.Importe = movimiento.Importe;
                                _coreDbContext.Entry(ccp).State = EntityState.Added;
                                _coreDbContext.MovCCProveedor.Add(ccp);
                                _coreDbContext.SaveChanges();
                            }
                            else
                            {
                                throw new ValidationException("No se encontro el proveedor");
                            }
                        }
                        context.Commit();
                    }
                    catch(Exception e)
                    {
                        context.Rollback();
                        throw new ValidationException(e.Message);
                    }
                    
                }
            }
        }
        public bool Validate(int idCaja)
        {
            return _coreDbContext.Cajas.Where(c => c.Id == idCaja && c.Estatus == true).Any();
        }

    }
}
