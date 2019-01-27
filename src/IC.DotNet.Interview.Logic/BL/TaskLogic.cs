using IC.DotNet.Interview.Core.Models;
using IC.DotNet.Interview.Core.Repositories;
using IC.DotNet.Interview.Logic.Models;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;


namespace IC.DotNet.Interview.Logic.BL
{
    public class TaskLogic : ITaskLogic
    {
        private readonly ITaskRepository _tasksRepository;
        private readonly IUserRepository _usersRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ICommentRepository _commentRepository;
        public TaskLogic(ITaskRepository tasksRepository, IUserRepository usersRepository,
                         IRoleRepository roleRepository, IUserRoleRepository userRoleRepository,
                         ICommentRepository commentRepository)
        {
            _tasksRepository = tasksRepository;
            _usersRepository = usersRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _commentRepository = commentRepository;
        }

        public bool Add(TaskViewModel task)
        {
            var user = HttpContext.Current.Request.Cookies["CurrentUser"];
            if (user != null)
            {
                task.AssignedUser.Id = task.AssignedUser.Id == null ? user.Value : task.AssignedUser.Id; //asign self if task not asigned
                User asignedUser = _usersRepository.Get(new Guid(task.AssignedUser.Id));
                if (asignedUser != null)  //make sure that the asignedUser actually exists, it shouldn't be necessary but just in case
                {
                    return _tasksRepository.Create(new Task
                    {
                        // Id = new Guid(task.Id), We shouldn't do this as the repository needs to create an ID for each new item and not receive it as a parameter (unless we want some custom ID logic)
                        Title = task.Title,
                        Description = task.Description,
                        IsFinished = task.IsFinished,
                        AssignedUserId = asignedUser.Id
                    });
                }
                else return false;
            }
            else return false;
        }

        public bool Delete(string id) //chaged this function to only get an ID as that is all we need to delete an entity from the DB
        {
            var user = HttpContext.Current.Request.Cookies["CurrentUser"];
            if (user != null)
            {
                return _tasksRepository.Delete(new Guid(id));
            }
            else return false;

        }

        public bool Edit(TaskViewModel task) //the id will be contained in the task model itself no need to send it separatelly
        {
            User asignedUser = _usersRepository.Get(new Guid(task.AssignedUser.Id));
            if (asignedUser != null)  //make sure that the asignedUser actually exists, it shouldn't be necessary but just in case
            {
                return _tasksRepository.Update(new Task //made some changes to the repository itself
                {
                    Id = new Guid(task.Id),
                    Title = task.Title,
                    Description = task.Description,
                    IsFinished = task.IsFinished,
                    AssignedUserId = asignedUser.Id // we have the ID from the object no need to generate it
                });
            }
            else return false;
        }

        public TaskViewModel Get(string id)
        {
            var user = HttpContext.Current.Request.Cookies["CurrentUser"];
            if (user != null)
            {
                var task = _tasksRepository.Get(new Guid(id));
                if (IsUserAdmin(user.Value))
                {
                    return mapTaskToViewModel(task); //return task if user is admin
                }
                else
                {
                    return task.AssignedUserId == new Guid(user.Value) ? mapTaskToViewModel(task) : null; //return task only if user is asigned to it
                }
            }
            else return null;
        }

        public IEnumerable<TaskViewModel> Get()
        {
            var user = HttpContext.Current.Request.Cookies["CurrentUser"];
            IEnumerable<TaskViewModel> result = null;
            if (user != null)
            {
                if (IsUserAdmin(user.Value))
                {
                    var tasks = _tasksRepository.Get(); //get all tasks for admin
                    result = tasks.Select(t => mapTaskToViewModel(t));
                }
                else
                {
                    var tasks = _tasksRepository.Get(x => x.AssignedUserId == new Guid(user.Value)); //get only tasks asigned for ordinary user
                    result = tasks.Select(t => mapTaskToViewModel(t));
                }
            }
            return result;
        }

        public IEnumerable<CommentViewModel> GetTaskComments(string taskId)
        {

            var user = HttpContext.Current.Request.Cookies["CurrentUser"];
            IEnumerable<TaskViewModel> result = null;
            if (user != null && taskId != null)
            {
                return _commentRepository.Get(x => x.TaskId != null && x.TaskId == new Guid(taskId))
                        .Select(c => new CommentViewModel
                        {
                            Author = new UserViewModel {
                                Id = c.UserCreatedId.ToString(),
                                Username = _usersRepository.Get(c.UserCreatedId).Username
                            },
                            DateCreated = c.DateCreated,
                            Text = c.Text
                        });
            }
            else return null;
        }

        public bool AddTaskComment(string taskId, CommentViewModel comment) {
            var user = HttpContext.Current.Request.Cookies["CurrentUser"];
            if (user != null && taskId != null && comment != null) //we should also validate for empty comments in a real implementation
            {
                return _commentRepository.Create(new Comment {
                    TaskId = new Guid(taskId),
                    Text = comment.Text
                });
            }
            else return false;
        }

        // we could also use a mapping function or automaper for the transformations done in the add and edit functions but i left them as is to show what i have changed and why

        private TaskViewModel mapTaskToViewModel(Task task)
        {
            if (task == null)
            {
                return null;
            }
            var user = _usersRepository.Get(task.AssignedUserId);

            return new TaskViewModel
            {
                Id = task.Id.ToString(),
                Title = task.Title,
                Description = task.Description,
                IsFinished = task.IsFinished,
                AssignedUser = new UserViewModel
                {
                    Id = user.Id.ToString(),
                    Username = user.Username
                }
            };
        }

        private bool IsUserAdmin(string userId)
        {

            var userGuid = new Guid(userId);
            var userRoleId = _userRoleRepository.Get(x => x.UserId == userGuid).FirstOrDefault().RoleId;
            var userRole = _roleRepository.Get(x => x.Id == userRoleId).FirstOrDefault();

            return userRole.Name == "administrator";
        }
    }
}
