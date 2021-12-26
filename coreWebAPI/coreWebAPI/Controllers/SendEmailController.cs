using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOM.Core.Db;
using MOM.Core.Models.Email;
using MOM.Core.WebAPI.Controllers.Interceptor;
using MOM.Core.WebAPI.Util;


namespace MOM.Core.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ControllerInterceptor))]
    public class SendEmailController : Controller
    {
        private readonly IEmailSend _emailSender;
        private CoreDbContext _coreDbContext;

        public SendEmailController(IEmailSend emailSender, IHostingEnvironment env,CoreDbContext coreDbContext)
        {
            _emailSender = emailSender;
            _coreDbContext = coreDbContext;
        }

        public IActionResult EnviaEmail()
        {
            return View();
        }

        [SecurityExclusion]
        [HttpPost("SendEmail")]
        public IActionResult SendEmail(EmailModel data)
        {
 
            if (ModelState.IsValid)
            {
                try
                {
                    var empresa = _coreDbContext.SystemSettings.Where(x => x.Key == "RazonSocial").FirstOrDefault();
                    data.Html += "<br><div><span style = 'color: #ff0000;'><strong>" + empresa.Value + "</strong></span></div>";
                    ////////////////////////////////TURNOS/////////////////////////////
                    var usuarios = _coreDbContext.Users.Where(x => !x.IsDeleted && x.Email!="");
                    foreach (var usuario in usuarios)
                    {
                        if (Convert.ToBoolean(usuario.Avisos))
                        {
                            data.Recipient = usuario.Email;
                            //TaskSendEmail(email.Recipient, email.Sender, email.Subject, email.Html , email.Message).GetAwaiter();
                            TaskSendEmail(data).GetAwaiter();
                        }
                    }


                    //return RedirectToAction("EmailEnviado");
                    return Ok();
                }
                catch (Exception e)
                {
                    //return RedirectToAction("EmailFallo");
                    return BadRequest(e.Message);
                }
            }
            return View(data);
        }

        public async Task TaskSendEmail(EmailModel data)
        {
            try
            {
                //email destino, assunto do email, mensagem a enviar
                await _emailSender.SendEmailAsync(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EmailEnviado()
        {
            return View();
        }

        public ActionResult EmailFalhou()
        {
            return View();
        }
    }
}
