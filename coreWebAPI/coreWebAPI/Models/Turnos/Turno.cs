using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using MOM.Core.Models.Security;
using MOM.Core.Models.Pacientes;
using MOM.Core.Models.Items;
using System.ComponentModel.DataAnnotations.Schema;
using MOM.Core.WebAPI.Models.Personal;

namespace MOM.Core.Models.Turnos
{
    public class Turno
    {
        [Key]
        public Int64 id { get; set; }
        public int idPaciente { get; set; }
        public int idPractica { get; set; }
        public int idProfesional { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        [ForeignKey("idPaciente")]
        public Paciente paciente { get; set; }
        [ForeignKey("idPractica")]
        public Item practica { get; set; }
        [ForeignKey("idProfesional")]
        public Persona profesional { get; set; }
        public bool? Inactivo { get; set; }
        public bool? Tomado { get; set; }
        public string observaciones { get; set; }
        public int UserId { get; set; }
        public string title { 
                get
            {
                return practica.Nombre +"-"+ paciente.nombre;
            }
        }



        private CoreDbContext _coreDbContext;

        public Turno(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Turno turno)
        {
            using (var context = _coreDbContext.Database.BeginTransaction())
            {
                try
                {
                    if(turno.UserId != 0)
                    {
                        OperationLog oper = new OperationLog();
                        oper.UserId = turno.UserId;
                        Permission permisos = _coreDbContext.Permissions.Where(p => p.Name.ToLower() == "LEER_TURNO").FirstOrDefault();
                        if (permisos != null)
                        {
                            oper.PermissionId = permisos.PermissionId;
                            oper.OperationDateTime = DateTime.Now;
                            oper.Description = "Crea turno";
                            _coreDbContext.Entry(oper).State = EntityState.Added;
                            _coreDbContext.OperationLog.Add(oper);
                            _coreDbContext.SaveChanges();
                        }
                    }

                    turno.Tomado = false;
                    turno.Inactivo = false;
                    _coreDbContext.Entry(turno).State = EntityState.Added;
                    _coreDbContext.Turnos.Add(turno);
                    _coreDbContext.SaveChanges();
                    context.Commit();
                }
                catch(Exception ex)
                {
                    context.Rollback();
                    throw new ValidationException(ex.Message);
                }
            }
        }


        public void Update(Turno turno)
        {
            using (var context = _coreDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (turno.UserId != 0)
                    {
                        OperationLog oper = new OperationLog();
                        oper.UserId = turno.UserId;
                        Permission permisos = _coreDbContext.Permissions.Where(p => p.Name.ToLower() == "LEER_TURNO").FirstOrDefault();
                        if (permisos != null)
                        {
                            oper.PermissionId = permisos.PermissionId;
                            oper.OperationDateTime = DateTime.Now;
                            oper.Description = "Actualiza turno";
                            _coreDbContext.Entry(oper).State = EntityState.Added;
                            _coreDbContext.OperationLog.Add(oper);
                            _coreDbContext.SaveChanges();
                        }
                    }

                    _coreDbContext.Update(turno);
                    _coreDbContext.SaveChanges();
                    context.Commit();
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    throw new ValidationException(ex.Message);
                }
            }
        }

        public void Delete(Turno turno)
        {
            using (var context = _coreDbContext.Database.BeginTransaction())
            {
                try
                {
                    OperationLog oper = new OperationLog();
                    oper.UserId = turno.UserId;
                    Permission permisos = _coreDbContext.Permissions.Where(p => p.Name.ToLower() == "LEER_TURNO").FirstOrDefault();
                    if (permisos != null)
                    {
                        oper.PermissionId = permisos.PermissionId;
                        oper.OperationDateTime = DateTime.Now;
                        oper.Description = "Elimina turno";
                        _coreDbContext.Entry(oper).State = EntityState.Added;
                        _coreDbContext.OperationLog.Add(oper);
                        _coreDbContext.SaveChanges();
                    }

                    _coreDbContext.Turnos.Remove(turno);
                    _coreDbContext.SaveChanges();
                    context.Commit();
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    throw new ValidationException(ex.Message);
                }
            }
        }

        public Turno Get(Int64 idt)
        {
            var turno = _coreDbContext.Turnos.Where(t=>t.id == idt).Include(t=>t.paciente).ThenInclude(p=>p.Propietario).Include(t=>t.practica).Include(t=>t.profesional).FirstOrDefault();
            return turno;
        }

        public List<Turno> List(
            DateTime inicio,
            DateTime fin,
            int idPaciente = 0,
            int idPractica=0,
            int idProfesional = 0
            )
       {
            //var paciente = _coreDbContext.Pacientes.Where(p => p.Id == idPaciente).FirstOrDefault();
            var practica = _coreDbContext.Items.Where(p => p.Id == idPractica).FirstOrDefault();
            var profesional = _coreDbContext.Users.Where(p => p.UserId == idProfesional).FirstOrDefault();

            var turnos = _coreDbContext.Turnos
            //.Include(r => r.Paciente)
            //.Include(r => r.Practica)
            //.Include(r => r.Profesional)
            .Where(r => (r.start>=inicio || inicio == null)
                        && (r.end <= fin || inicio == null)
                        //&& (r.Paciente == paciente || paciente == null)
                        //&& (r.Practica == practica || practica == null)
                        //&& (r.Profesional == profesional || profesional == null))
                        ).ToList();

            return turnos;
        }

        public List<Turno> listar()
        {
            //return _coreDbContext.Turnos.Include(t => t.paciente).Include(r => r.practica).Include(r => r.profesional).ToList();
            return _coreDbContext.Turnos.Include(t => t.paciente).Include(r => r.practica).Include(r => r.profesional).ToList();
            //return turnos;
        }
        public List<Turno> porPaciente(int id)
        {
            return _coreDbContext.Turnos.Where(t=>t.idPaciente == id && t.Tomado == false).Include(t => t.paciente).Include(r => r.practica).Include(r => r.profesional).ToList();

            //return turnos;
        }

    }
}