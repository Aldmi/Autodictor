using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitys.Authentication
{
    public class User : EntityBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public string FullName { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
