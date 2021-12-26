using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MOM.Core.WebAPI.Models.cajas
{
    public class Caja
    {
        private CoreDbContext _coreDbContext;
        public int Id { get; set; }
		public int userId { get; set; }
		public DateTime Fecha_Apertura { get; set; }
		public DateTime Fecha_Cierre { get; set; }
		public decimal SaldoInicial { get; set; }
		public decimal Ingresos { get; set; }
		public decimal  Egresos { get; set; }
		public Boolean Estatus { get; set; }
		public string Comentario { get; set; }
		public Caja(CoreDbContext coreDbContext)
		{
			_coreDbContext = coreDbContext;
		}
        public void Add(Caja caja)
        {
            if (Validate(caja.userId))
            {
                throw new ValidationException("Este usuario ya aperturo una caja imposible continuar" );
            }
            else
            {
                using (var context = _coreDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        OperationLog oper = new OperationLog();
                        oper.UserId = caja.userId;
                        Permission permisos = _coreDbContext.Permissions.Where(p => p.Name.ToLower() == "CAJA_ABRIR").FirstOrDefault();
                        if (permisos != null)
                        {
                            oper.PermissionId = permisos.PermissionId;
                            oper.OperationDateTime = DateTime.Now;
                            oper.Description = "Abrir caja";
                            _coreDbContext.Entry(oper).State = EntityState.Added;
                            _coreDbContext.OperationLog.Add(oper);
                            _coreDbContext.SaveChanges();
                        }
                        caja.Estatus = true;
                        caja.Fecha_Apertura = DateTime.Now;
                        caja.Fecha_Cierre = DateTime.Now;
                        caja.Egresos = 0;
                        caja.Ingresos = 0;
                        caja.Comentario = "";
                        _coreDbContext.Entry(caja).State = EntityState.Added;
                        _coreDbContext.Cajas.Add(caja);
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
        }

        public void cerrar(Caja caja)
        {
            Caja cjant = _coreDbContext.Cajas.Where(c => c.userId == caja.userId && c.Estatus == true).FirstOrDefault();
            if(cjant != null)
            {
                using (var context = _coreDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        OperationLog oper = new OperationLog();
                        oper.UserId = caja.userId;
                        Permission permisos = _coreDbContext.Permissions.Where(p => p.Name.ToLower() == "GESTION_VENTA").FirstOrDefault();
                        if (permisos != null)
                        {
                            oper.PermissionId = permisos.PermissionId;
                            oper.OperationDateTime = DateTime.Now;
                            oper.Description = "cerrar caja";
                            _coreDbContext.Entry(oper).State = EntityState.Added;
                            _coreDbContext.OperationLog.Add(oper);
                            _coreDbContext.SaveChanges();
                        }

                        cjant.Fecha_Cierre = DateTime.Now;
                        cjant.Comentario = caja.Comentario;
                        cjant.Estatus = false;
                        _coreDbContext.Update(cjant);
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
            else
            {
                throw new ValidationException("el usuario no tiene caja abierta");
            }
        }

        public Caja consultar(int id)
        {
            Caja cjant = _coreDbContext.Cajas.Where(c => c.userId == id && c.Estatus == true).FirstOrDefault();
            if (cjant == null)
            {
                cjant = new Caja(_coreDbContext);
                cjant.Estatus = false;
                cjant.userId = id;
            }
            return cjant;
        }

        //public void Delete(int id)
        //{
        //    var item = _coreDbContext.Items.Where(hq => hq.Id == id).FirstOrDefault();
        //    if (item != null)
        //    {
        //        item.Inactivo = true;
        //        _coreDbContext.Items.Update(item);
        //        _coreDbContext.SaveChanges();
        //    }
        //}

        public bool Validate(int userid)
        {
            return _coreDbContext.Cajas.Where(obj=>obj.userId == userid && obj.Estatus == true).Any();
        }

    }
}
