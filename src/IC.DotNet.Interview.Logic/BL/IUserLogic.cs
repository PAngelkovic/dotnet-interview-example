using IC.DotNet.Interview.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.DotNet.Interview.Logic.BL
{
    public interface IUserLogic
    {
        IEnumerable<UserViewModel> Get();
        UserViewModel Get(string id);
    }
}
