using Microsoft.AspNetCore.Http;
using MOM.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MOM.Core.Models.Email
{
    [NotMapped]
    public class EmailModel
    {
        //[Required, Display(Name = "Email de destino")]
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Html { get; set; }
        public string Sender { get; set; }
    }
}
