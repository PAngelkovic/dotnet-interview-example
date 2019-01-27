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
        private readonly IAuthorizationLogic _authorizationLogic;

        public TaskController(ITaskLogic taskLogic, IUserLogic userLogic, IAuthorizationLogic authorizationLogic)
        {
            _taskLogic = taskLogic;
            _userLogic = userLogic;
            _authorizationLogic = authorizationLogic;
        }

        public ActionResult Index()
        {
            return View(_taskLogic.Get());
        }
        [HttpGet]
        public ActionResult Edit(string taskID)
        {
            if (taskID == null)
            {
                return RedirectToAction("Index"); //this should be done with a filter function in a real implementation
            }
            ViewBag.Users = _userLogic.Get().ToList(); //Idealy both the users and the task need to be encapsulated in a common View Model so they can be sent together to the view
            var task = _taskLogic.Get(taskID);
            return View(_taskLogic.Get(taskID));
        }
        [HttpPost]
        public ActionResult Edit(TaskViewModel editedTask)
        {
            if (editedTask == null)
            {
                return RedirectToAction("Index"); //this should be done with a filter function in a real implementation and probably display some sort of an error or validation message
            }
            var response = _taskLogic.Edit(editedTask);
            // Validate the response here, return to the editing page if the backend validation failed and display a custom validation error 
            //also if valid display message that edit was successfull
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (_authorizationLogic.IsUserLoggedIn()) // this can also be done with a filter function
            {
                ViewBag.Users = _userLogic.Get().ToList();
                return View();
            }
            else return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Create(TaskViewModel task) {
            var result = _taskLogic.Add(task);
            if (result)
            {
                return RedirectToAction("Index"); //display message task created successfully
            }
            else return View();
        }
        [HttpPost]
        public ActionResult Delete(string taskId) {
            var result = true;
            if (_authorizationLogic.IsUserLoggedIn()) // this can also be done with a filter function
            {
                result = _taskLogic.Delete(taskId); 
            }
            return RedirectToAction("Index"); //Display message about the success of the delete
        }
    }
}