using IC.DotNet.Interview.Core.Models;
using IC.DotNet.Interview.Core.Repositories;
using IC.DotNet.Interview.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.DotNet.Interview.Logic.BL
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _usersRepository;

        public UserLogic( IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public IEnumerable<UserViewModel> Get()
        {
            return _usersRepository.Get().Select(x => mapUserToViewModel(x));
        }

        public UserViewModel Get(string id)
        {
           var user = _usersRepository.Get(new Guid(id));
            return mapUserToViewModel(user);
        }

        private UserViewModel mapUserToViewModel(User user) {
            if (user == null) {
                return null;
            }
            return new UserViewModel
            {
                Id = user.Id.ToString(),
                Username = user.Username
            };
        }
    }
}
