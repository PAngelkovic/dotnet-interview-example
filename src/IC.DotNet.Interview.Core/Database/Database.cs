using IC.DotNet.Interview.Core.Models;
using System.Collections.Generic;

namespace IC.DotNet.Interview.Core.Database
{
    public class Database
    {
        public ICollection<Task> Tasks { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
