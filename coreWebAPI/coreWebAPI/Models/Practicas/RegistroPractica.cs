using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Items;
using MOM.Core.Models.Pacientes;
using MOM.Core.Models.Security;
using MOM.Core.WebAPI.Models.Ventas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.Practicas
{
    public class RegistroPractica
    {
        private CoreDbContext _coreDbContext;
        [Key]
        public Int64 idPractica{get;set;}
		public int userId { get; set; }
        public int  clienteId { get; set; }
        public int pacienteId { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string comentarios { get; set; }
        public string estatus { get; set; }
        public double total { get; set; }
        public double descuento { get; set; }
        [ForeignKey("userId")]
        public User usuario { get; set; }
        [ForeignKey("clienteId")]
        public User cliente { get; set; }
        
        public Paciente paciente { get; set; }
        public List<DetallePractica> detallePractica { get; set; }
        public List<Desparacitacion> desparacitacion { get; set; }
        public List<Vacuna> vacuna { get; set; }

        public RegistroPractica(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }
        public Venta Add(RegistroPractica practica)
        {
            Venta v = new Venta(_coreDbContext);
            using (var context = _coreDbContext.Database.BeginTransaction())
            {
                try
                {
                    practica.cliente = null;
                    practica.paciente = null;
                    practica.usuario = null;
                    practica.fechaRegistro = DateTime.Now;
                    /*_coreDbContext.Entry(practica).State = EntityState.Added;
                    _coreDbContext.RegistroPracticas.Add(practica);
                    _coreDbContext.SaveChanges();*/

                    foreach (DetallePractica d in practica.detallePractica)
                    {
                        Item item = _coreDbContext.Items.Find(d.itemId);
                        if (item != null){
                            /*d.idPractica = practica.idPractica;
                            _coreDbContext.Entry(d).State = EntityState.Added;
                            _coreDbContext.DetallePractica.Add(d);
                            _coreDbContext.SaveChanges();*/

                            //Vacuna
                            /*if (item.categoriaId == 1)
                            {
                                Vacuna vacuna = new Vacuna(_coreDbContext);
                                vacuna.Fecha = DateTime.Now;
                                vacuna.idPractica = practica.idPractica;
                                vacuna.itemId = item.Id;
                                vacuna.PacienteId = practica.pacienteId;
                                _coreDbContext.Entry(vacuna).State = EntityState.Added;
                                _coreDbContext.fVacunas.Add(vacuna);
                                _coreDbContext.SaveChanges();
                            }*/
                            //Desparacitante
                            /*if (item.categoriaId == 2)
                            {
                                Desparacitacion desp = new Desparacitacion(_coreDbContext);
                                desp.Fecha = DateTime.Now;
                                desp.idPractica = practica.idPractica;
                                desp.itemId = item.Id;
                                desp.PacienteId = practica.pacienteId;
                                _coreDbContext.Entry(desp).State = EntityState.Added;
                                _coreDbContext.Desparacitaciones.Add(desp);
                                _coreDbContext.SaveChanges();
                            }*/
                        }
                        else
                        {
                            throw new ValidationException("No se pudo encontrar el Item con id "+ d.itemId.ToString());
                        }
                    }

                    if (practica.estatus.Equals("cerrada"))
                    {
                        v.total = practica.total;
                        v.idCaja = 4;
                        v.fechaHora = DateTime.Now;
                        v.userId = practica.clienteId;
                        v.total = practica.total;
                        v.descuento = practica.descuento;
                        v.ticketTarjeta = "";
                        v.tipoPagoId = 1;//Efectivo
                        v.Estatus = "pendiente";
                        _coreDbContext.Entry(v).State = EntityState.Added;
                        _coreDbContext.Ventas.Add(v);
                        _coreDbContext.SaveChanges();
                        foreach (DetallePractica d in practica.detallePractica)
                        {
                            DetalleVenta dv = new DetalleVenta();
                            dv.cantidad = d.cantidad;
                            dv.descuento = d.descuento;
                            dv.importe = d.importe;
                            dv.itemId = d.itemId;
                            dv.precio = d.precio;
                            dv.ventaId = v.id;
                            _coreDbContext.Entry(dv).State = EntityState.Added;
                            _coreDbContext.DetalleVentas.Add(dv);
                            _coreDbContext.SaveChanges();
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
            return v;
        }
        /*public List<RegistroPractica> listar()
        {
            return _coreDbContext.RegistroPracticas.Include(r=>r.usuario)
                .Include(r => r.cliente)
                .Include(r => r.detallePractica)
                .Include(r => r.vacuna)
                .Include(r => r.desparacitacion)
                .Include(r=>r.paciente).ToList();
        }
        public List<RegistroPractica> PracticasPaciente(int id)
        {
            return _coreDbContext.RegistroPracticas.Include(r => r.usuario)
                .Include(r => r.cliente)
                .Include(r => r.detallePractica)
                .Include(r => r.vacuna)
                .Include(r => r.desparacitacion)
                .Include(r => r.paciente).OrderByDescending(P=>P.fechaRegistro).Where(p=>p.pacienteId == id).ToList();
        }
        public RegistroPractica Get(int idp)
        {
            return _coreDbContext.RegistroPracticas.Find(idp);
        }*/
    }
}
