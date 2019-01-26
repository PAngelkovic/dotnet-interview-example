using System.ComponentModel;
using System.Web.Mvc;
namespace IC.DotNet.Interview.Logic.Models
{
    public class TaskViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        [AllowHtml] //added this to enable the controller not to throw a validation error because of the html elements within the field
        public string Description { get; set; }
        [DisplayName("Is task finished?")] //added this for a neater display name for this property
        public bool IsFinished { get; set; }

        [DisplayName("Assigned to:")]
        public UserViewModel AssignedUser { get; set; }
    }
}