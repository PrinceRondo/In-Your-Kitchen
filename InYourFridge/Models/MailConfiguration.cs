using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Models
{
    public class MailConfiguration
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MailFromAddress { get; set; }
        public string SMTPHostName { get; set; }
        public string SMTPPort { get; set; }
        public string MailUsername { get; set; }
        public string MailPassword { get; set; }
        public bool EnableSSL { get; set; }
    }
}
