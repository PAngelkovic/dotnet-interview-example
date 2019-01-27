using IC.DotNet.Interview.Core.Database;
using IC.DotNet.Interview.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.DotNet.Interview.Core.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IDbContext dbContext) : base(dbContext)
        {

        }
    }
}
