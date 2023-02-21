using System.ComponentModel.DataAnnotations;

namespace AdminApp.Models
{
    public partial class UserRole
    {
        public int IdRole { get; set; }
        public int IdUser { get; set; }

        public virtual Role IdRoleNavigation { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
