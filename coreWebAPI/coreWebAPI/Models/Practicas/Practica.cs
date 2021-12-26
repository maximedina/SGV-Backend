using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Items;
using MOM.Core.Models.Pacientes;
using MOM.Core.Models.Security;
using MOM.Core.Models.Turnos;
using MOM.Core.WebAPI.Models.Ventas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.Practicas
{
    public class Practica
    {
        private CoreDbContext _coreDbContext;
        [Key]
        public int id { get; set; }
        public DateTime fechaHora { get; set; }
        public Int64 ventaId { get; set; }
        public Int64 turnoId { get; set; }
        public int pacienteId { get; set; }
        public int userId { get; set; }
        public int itemId { get; set; }
        public DateTime proximaPractica { get; set; }
        public string texto { get; set; }
        //[ForeignKey("userId")]
        public Venta venta { get; set; }//Nuevo
        public Turno turno { get; set; } //Nuevo
        public Paciente paciente { get; set; }//Ya se selecciona
        public User usuario { get; set; }//Ya se selecciona
        public Item item{ get; set; }//Nuevo
        public Practica(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public Venta add(Practica practica)
        {
            Venta v = new Venta(_coreDbContext);
            using (var context = _coreDbContext.Database.BeginTransaction())
            {
                try
                {

                    OperationLog oper = new OperationLog();
                    oper.UserId = practica.userId;
                    Permission permisos = _coreDbContext.Permissions.Where(p => p.Name.ToLower() == "PRACTICAS").FirstOrDefault();
                    if (permisos != null)
                    {
                        oper.PermissionId = permisos.PermissionId;
                        oper.OperationDateTime = DateTime.Now;
                        oper.Description = "Crea practica";
                        _coreDbContext.Entry(oper).State = EntityState.Added;
                        _coreDbContext.OperationLog.Add(oper);
                        _coreDbContext.SaveChanges();
                    }

                    if (practica.turnoId == 0)
                    {
                        Turno turno = _coreDbContext.Turnos.Where(t => t.start.Year == 1900).FirstOrDefault();
                        if (turno == null)
                        {
                            throw new ValidationException("No seleccionaste el turno y no existe un turno en el año 1900");
                        }
                        practica.turnoId = turno.id;
                    }
                    
                    v = practica.venta;
                    v.total += Math.Round(practica.item.PrecioVenta,2);
                    v.idCaja = 4;
                    v.fechaHora = DateTime.Now;
                    /*v.userId = 1;
                    v.total = 0;
                    v.descuento = 0;
                    v.ticketTarjeta = "";*/
                    v.tipoPagoId = 1;//Efectivo
                    v.Estatus = "pendiente";
                    _coreDbContext.Entry(v).State = EntityState.Added;
                    _coreDbContext.Ventas.Add(v);
                    _coreDbContext.SaveChanges();
                    
                    foreach (DetalleVenta dv in practica.venta.detalleVenta)
                    {
                        dv.ventaId = v.id;
                        _coreDbContext.Entry(dv).State = EntityState.Added;
                        _coreDbContext.DetalleVentas.Add(dv);
                        _coreDbContext.SaveChanges();
                    }

                    DetalleVenta dpractica = new DetalleVenta();
                    dpractica.cantidad = 1;
                    dpractica.descuento = 0;
                    dpractica.importe = Math.Round(practica.item.PrecioVenta,2);
                    dpractica.itemId = practica.item.Id;
                    dpractica.precio = Math.Round(practica.item.PrecioVenta,2);
                    dpractica.ventaId = v.id;
                    _coreDbContext.Entry(dpractica).State = EntityState.Added;
                    _coreDbContext.DetalleVentas.Add(dpractica);
                    _coreDbContext.SaveChanges();

                    practica.venta = null;
                    practica.turno = null;
                    practica.paciente = null;
                    practica.usuario = null;
                    practica.item = null;
                    practica.fechaHora = DateTime.Now;
                    practica.ventaId = v.id;
                    if(practica.proximaPractica.Year == 1)
                    {
                        practica.proximaPractica = DateTime.Now;
                    }
                    _coreDbContext.Entry(practica).State = EntityState.Added;
                    _coreDbContext.Practicas.Add(practica);
                    _coreDbContext.SaveChanges();

                    
                    context.Commit();
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    throw new ValidationException(ex.Message);
                }
            }
            return v;
        }

        public List<Practica> PracticasPaciente(int idPaciente)
        {
            return _coreDbContext.Practicas.Where(p => p.pacienteId == idPaciente).Include(p => p.item).ThenInclude(i=>i.Tipo).Include(p=>p.venta).ThenInclude(v=>v.detalleVenta).ThenInclude(d=>d.item).ToList();
        }
        
    }
}
