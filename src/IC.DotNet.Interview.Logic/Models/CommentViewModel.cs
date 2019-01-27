using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace IC.DotNet.Interview.Logic.Models
{
    public class CommentViewModel
    {
        public UserViewModel Author { get; set; }
        [AllowHtml]
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
