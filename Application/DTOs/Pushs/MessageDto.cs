using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Pushs
{
    public class MessageDto
    {
        public string Title { get; set; }
        public string Body { get; set; }

        [Display(Name = "Optional parameter")]
        public  string Topic { get; set; }

        [Display(Name ="Optional parameter")]
        public string Token { get; set; }
    }
}
