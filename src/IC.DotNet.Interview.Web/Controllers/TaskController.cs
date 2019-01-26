using IC.DotNet.Interview.Logic.BL;
using IC.DotNet.Interview.Logic.Models;
using System.Web.Mvc;
using System.Linq;
namespace IC.DotNet.Interview.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskLogic _taskLogic;

        public TaskController(ITaskLogic taskLogic)
        {
            _taskLogic = taskLogic;
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