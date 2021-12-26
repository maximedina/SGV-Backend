using Microsoft.EntityFrameworkCore;
using MOM.Core.Models.Security;
using MOM.Core.Db.Configuration.Security;
using MOM.Core.Models.Shared;
using MOM.Core.Db.Configuration.Shared;
using MOM.Core.Models.Items;
using MOM.Core.Models.Localizacion;
using MOM.Core.Db.Configuration.Items;
using MOM.Core.Db.Configuration.Localizacion;
using MOM.Core.Db.Configuration.Pacientes;
using MOM.Core.Db.Configuration.Turnos;
using MOM.Core.Models.Pacientes;
using MOM.Core.Models.Turnos;
using MOM.Core.WebAPI.Models.cajas;
using MOM.Core.WebAPI.Db.Configuration.cajas;
using MOM.Core.WebAPI.Db.Configuration.ventas;
using MOM.Core.WebAPI.Models.Ventas;
using MOM.Core.WebAPI.Models.Proveedores;
using MOM.Core.WebAPI.Models.Personal;
using MOM.Core.WebAPI.Models.Practicas;

namespace MOM.Core.Db
{
    public class CoreDbContext : BaseDbContext
    {
        #region Tables

        #region Pacientes
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Raza> Razas { get; set; }
        public DbSet<Especie> Especies { get; set; }
        public DbSet<Persona> Personal { get; set; }
        #endregion

        #region Practicas
        //public DbSet<RegistroPractica> RegistroPracticas { get; set; }
        public DbSet<Practica> Practicas { get; set; }
        //public DbSet<DetallePractica> DetallePractica { get; set; }
        //public DbSet<Desparacitacion> Desparacitaciones { get; set; }
        //public DbSet<Vacuna> Vacunas { get; set; }

        #endregion

        #region Turnos
        public DbSet<Turno> Turnos { get; set; }
        #endregion

        #region Items
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Rubro> Rubros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<TipoMovimiento> MotivoMovimiento { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<MovCajaVta> MovCaja_venta { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Movimiento> MovimientosCaja { get; set; }
        public DbSet<MCCProveedor> MovCCProveedor { get; set; }
        public DbSet<MCCCliente> MovCCCliente { get; set; }

        #endregion

        #region Localizacion
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        #endregion

        #region Security
        public DbSet<SystemModule> SystemModules { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProfilePermission> ProfilePermissions { get; set; }
        public DbSet<OperationLog> OperationLog { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<SessionLog> SessionLog { get; set; }

        #endregion

        #region Shared
        public DbSet<Uom> Uoms { get; set; }
        public DbSet<Shape> Shapes { get; set; }
        public DbSet<DataTypes> DataTypes { get; set; }
        #endregion

        #endregion

        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Database Properties

            #region Paciente
            modelBuilder.ApplyConfiguration(new PacientesConfiguration());
            modelBuilder.ApplyConfiguration(new RazasConfiguration());
            modelBuilder.ApplyConfiguration(new EspeciesConfiguration());
            #endregion

            #region Turno
            modelBuilder.ApplyConfiguration(new TurnosConfiguration());
            #endregion

            #region Item
            modelBuilder.ApplyConfiguration(new TiposConfiguration());
            modelBuilder.ApplyConfiguration(new GruposConfiguration());
            modelBuilder.ApplyConfiguration(new RubrosConfiguration());
            modelBuilder.ApplyConfiguration(new ProveedoresConfiguration());
            modelBuilder.ApplyConfiguration(new ItemsConfiguration());
            modelBuilder.ApplyConfiguration(new TipoMovimientoConfiguration());
            modelBuilder.ApplyConfiguration(new CajasConfiguration());
            modelBuilder.ApplyConfiguration(new MovimientosCajaConfiguration());
            modelBuilder.ApplyConfiguration(new DetalleVentasConfiguration());
            modelBuilder.ApplyConfiguration(new VentasConfiguration());
            #endregion

            #region Localizacion
            modelBuilder.ApplyConfiguration(new ProvinciasConfiguration());
            modelBuilder.ApplyConfiguration(new CiudadesConfiguration());
            #endregion

            #region security
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new ProfilePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new SystemModuleConfiguration());
            modelBuilder.ApplyConfiguration(new SystemSettingConfiguration());
            modelBuilder.ApplyConfiguration(new OperationLogConfiguration());
            modelBuilder.ApplyConfiguration(new SessionConfiguration());
            #endregion

            #region Shared
            modelBuilder.ApplyConfiguration(new DataTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UomConfiguration());
            #endregion

        #endregion
    }
}
}