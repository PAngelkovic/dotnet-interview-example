using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.DotNet.Interview.Core.Models
{
    public class Comment : BaseModel
    {
        public Guid TaskId { get; set; }
        public string Text { get; set; }
    }
}
