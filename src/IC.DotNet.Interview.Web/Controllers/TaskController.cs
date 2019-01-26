using IC.DotNet.Interview.Logic.BL;
using IC.DotNet.Interview.Logic.Models;
using System.Web.Mvc;
using System.Linq;
namespace IC.DotNet.Interview.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskLogic _taskLogic;
        private readonly IUserLogic _userLogic;

        public TaskController(ITaskLogic taskLogic, IUserLogic userLogic)
        {
            _taskLogic = taskLogic;
            _userLogic = userLogic;
        }

        public ActionResult Index()
        {
            return View(_taskLogic.Get());
        }
        [HttpGet]
        public ActionResult Edit(string taskID) {
            if (taskID == null)
            {
                return RedirectToAction("Index"); //this should be done with a filter function in a real implementation
            }
            ViewBag.Users = _userLogic.Get().ToList(); //Idealy both the users and the task need to be encapsulated in a common View Model so they can be sent together to the view
            var task = _taskLogic.Get(taskID);
            return View(_taskLogic.Get(taskID)); 
        }
        [HttpPost]
        public ActionResult Edit(TaskViewModel editedTask) { 
            if (editedTask == null)
            {
                return RedirectToAction("Index"); //this should be done with a filter function in a real implementation and probably display some sort of an error or validation message
            }
            var response = _taskLogic.Edit(editedTask);
            // Validate the response here, return to the editing page if the backend validation failed and display a custom validation error 
            return RedirectToAction("Index");
        }
    }
}