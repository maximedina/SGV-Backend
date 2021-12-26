using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MOM.Core.Models.Security;
using System;
using MOM.Core.WebAPI.Models.Practicas;

namespace MOM.Core.Models.Pacientes
{
    public class Paciente
    {
        [Key]
        public int id { get; set; }
        public int razaId { get; set; }
        public int userId { get; set; }
        public string nombre { get; set; }
        public User Propietario { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public DateTime? fechaFallecimiento { get; set; }
        public Raza Raza { get; set; }
        public List<Vacuna> vacuna { get; set; }
        public List<Desparacitacion> desparacitacion { get; set; }
        public List<RegistroPractica> practicas { get; set; }
        public string foto { get; set; }
        public string observaciones { get; set; }
        public bool? inactivo { get; set; }
        public bool? fallecido { get; set; }
        //[NotMapped]
        //public string Identificador => $"{Nombre} - {Raza.Especie.Nombre} - {Propietario.Name}";
        public float Edad {
            get
            {
                return ((TimeSpan)(DateTime.Now - fechaNacimiento)).Days/365;
            }   
        }

        private CoreDbContext _coreDbContext;

        public Paciente(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public void Add(Paciente paciente)
        {
            /*if (Validate(paciente.nombre, paciente.id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + paciente.Nombre);
            }
            else
            {
                paciente.inactivo = false;
                _coreDbContext.Entry(paciente).State = EntityState.Added;
                _coreDbContext.Pacientes.Add(paciente);
                _coreDbContext.SaveChanges();
            }*/
        }

        public void Update(Paciente paciente)
        {
            /*if (Validate(paciente.nombre, paciente.id))
            {
                throw new ValidationException("Ya existe un registro con el nombre " + paciente.Nombre);
            }
            else
            {
                _coreDbContext.Update(paciente);
                _coreDbContext.SaveChanges();
            }*/
        }

        public void Delete(int id)
        {
            var paciente = _coreDbContext.Pacientes.Where(hq => hq.id == id).FirstOrDefault();
            if (paciente != null)
            {
                paciente.inactivo = true;
                _coreDbContext.Pacientes.Update(paciente);
                _coreDbContext.SaveChanges();
            }
        }

        public List<Paciente> List(
            bool incluirInactivos,
            int id = 0,
            string nombre = "",
            int idPropietario = 0,
            int idRaza = 0
            )
       {
            var propietario = _coreDbContext.Users.Where(p => p.UserId == idPropietario).FirstOrDefault();
            var raza = _coreDbContext.Razas.Where(p => p.Id == idRaza).FirstOrDefault();

            var pacientes = _coreDbContext.Pacientes
            //.Include(r => r.Propietario)
            //.Include(r => r.Raza)
            .Where(r => (r.nombre.Contains(nombre) || nombre == null)
                        && (r.id== id)
                        //&& (r.Propietario == propietario || propietario == null)
                        //&& (r.Raza == raza || raza == null)
                        && (r.inactivo == incluirInactivos || incluirInactivos)).ToList();

            return pacientes;
        }

        public List<Paciente> Listar()
        {
            //return  _coreDbContext.Pacientes.Include(p=>p.Propietario).Include(p=>p.Raza).Include(p=>p.desparacitacion).ThenInclude(d=>d.item).Include(p=>p.vacuna).ThenInclude(v=>v.item).ToList();
            return _coreDbContext.Pacientes.Include(p => p.Propietario).Include(p => p.Raza).ToList();

            //return pacientes.ToList();
        }

        //public bool Validate(string nombre, int id) => _coreDbContext.Pacientes.Where(obj => obj.Nombre.Equals(nombre) && obj.Id != id).Any();

    }
}