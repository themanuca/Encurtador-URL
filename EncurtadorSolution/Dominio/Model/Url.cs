using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dominio.Model
{
    public class Url
    {
        [Key]
        public string Shortcoded { get; set; }  = string.Empty;
        public string LongUrl {get;set; } = string.Empty;
        public DateTime CreatedAt {get;set; } = DateTime.UtcNow;

    }
}
