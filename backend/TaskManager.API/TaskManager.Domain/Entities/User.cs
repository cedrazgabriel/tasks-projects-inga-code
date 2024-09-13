using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    public class User : BaseEntity 
    {

        private User() { }

        public User(string username, string password)
        {
            UserName = username;
            Password = password;
            CreatedAt = DateTime.UtcNow;
        }

        public string UserName { get; set; } 
        public string Password { get; set; } 
        public DateTime CreatedAt { get; set; }  
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Collaborator Collaborator { get; set; }
    }
}
