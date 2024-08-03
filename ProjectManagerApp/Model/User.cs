using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagerApp.Model.Base;

namespace ProjectManagerApp.Model
{
    internal class User : Entity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Position Position { get; set; }

        public List<TaskProject>? Tasks { get; set; } = [];
        public List<Comment>? Comments { get; set; } = [];

        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public override string ToString() => $"Пользователь {FullName} | {Position}";
    }
}
