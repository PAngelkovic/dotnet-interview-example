using System.Web.Mvc;
namespace IC.DotNet.Interview.Logic.Models
{
    public class TaskViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        [AllowHtml] //added this to enable the controller not to throw a validation error because of the html elements within the field
        public string Description { get; set; }
        public bool IsFinished { get; set; }

        public UserViewModel AssignedUser { get; set; }
    }
}