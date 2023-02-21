using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AdminApp.Models
{
    public partial class LogIn
    {
        public int IdLogin { get; set; }

        public int IdUser { get; set; }


        public DateTime? LoggedIn  { get ;  set ; }

        public DateTime? LoggedOut { get; set; }

        public virtual User IdUserNavigation { get; set; }

        public LogIn() {     }              
    }
}
