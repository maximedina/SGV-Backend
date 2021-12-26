using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Localizacion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOM.Core.Models.Security
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Profile Profile { get; set; }
        public bool IsDeleted { get; private set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Calle { get; set; }
        public string Altura { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }
        public Ciudad Ciudad { get; set; }
        public double cuentaCorriente { get; set; }
        public double limiteCC { get; set; }
        public string dni { get; set; }
        public bool Avisos { get; set; }
        public bool NotificacionTurnos { get; set; }
        public bool NotificacionProxVisita { get; set; }

        public string Identificador => $"{Login} - {Name}";
        public string Domicilio => $"{(string.IsNullOrEmpty(Calle) ? "" : Calle)} {(string.IsNullOrEmpty(Altura) ? "" : Altura)} {(string.IsNullOrEmpty(Piso) ? "" : Piso)} {(string.IsNullOrEmpty(Dpto) ? "" : Dpto)}";
        //{ 
        //    get
        //    {
        //        return ((Calle==null) ? Calle : "") + " " + (string.IsNullOrEmpty(Altura) ? Altura : "");
        //    }
        //}

        private CoreDbContext _dbContext;

        public User() {
        }

        public User(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(User user)
        {
            if (Validate(user.Name, user.UserId))
            {
                throw new Exception($"Usuário {user.Name} ya registrado!");
            }

            user.IsDeleted = false;
            _dbContext.Entry(user).State = EntityState.Added;
            _dbContext.SaveChanges();
        }

        public void Update(User user)
        {
            if (Validate(user.Name, user.UserId))
            {
                throw new Exception($"Usuário {user.Name} ya registrado!");
            }

            _dbContext.Entry(user).State = EntityState.Added;
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public void ChangePassword(User user,string passwordActual="", string passwordNueva="")
        {
            var usuario = _dbContext.Users.Where(x => x.UserId != user.UserId).Any();
            if (user.Password == passwordActual)
            {
                user.Password = passwordNueva;
            }
            else
            {
                throw new Exception($"El password actual no coincide.");
            }

            _dbContext.Entry(user).State = EntityState.Added;
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                user.IsDeleted = true;
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
            }
        }

        public List<User> ListAll() => _dbContext.Users
                                        .Include(u => u.Profile)
                                        .Where(u => !u.IsDeleted).OrderBy(u => u.Name)
                                        .ToList();

        public List<User> ListByValue(bool includeDeleted, string login = "", string name = "", int profileId = 0)
        {
            var profile = _dbContext.Profiles.Where(p => p.ProfileId == profileId).FirstOrDefault();

            var users = _dbContext.Users
            .Include(u => u.Profile)
            .Where(u => (u.Login.Contains(login) || login == null)
                        && (u.Name.Contains(name) || name == null)
                        && (u.Profile == profile || profile == null)
                        && u.IsDeleted == includeDeleted || includeDeleted).ToList();

            return users;
        }


        public List<User> Buscar(int? codigo, string nombre, string dnisearch)
        {
            List<User> users = _dbContext.Users.Include(u=>u.Ciudad)
                                      .ToList();
            if (codigo.HasValue)
            {
                users = users.Where(u=>u.UserId == codigo).ToList();
            }
            if (nombre != null)
            {
                users = users.Where(u => u.Name.ToLower().Contains(nombre.ToLower())).ToList();
            }
            if (dnisearch != null)
            {
                users = users.Where(u => u.dni.ToLower().Contains(dnisearch.ToLower())).ToList();
            }
            return users;
        }


        public bool Validate(string name, long id) => _dbContext.Users.Where(x => x.Name.Equals(name) && x.UserId != id).Any();
    }
}