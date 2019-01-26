using IC.DotNet.Interview.Core.Models;
using IC.DotNet.Interview.Core.Repositories;
using IC.DotNet.Interview.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IC.DotNet.Interview.Logic.BL
{
    public class TaskLogic : ITaskLogic
    {
        private readonly ITaskRepository _tasksRepository;
        private readonly IUserRepository _usersRepository;

        public TaskLogic(ITaskRepository tasksRepository, IUserRepository usersRepository)
        {
            _tasksRepository = tasksRepository;
            _usersRepository = usersRepository;
        }

        public bool Add(TaskViewModel task)
        {
            User asignedUser = _usersRepository.Get(new Guid(task.AssignedUser.Id));
            if (asignedUser != null)  //make sure that the asignedUser actually exists, it shouldn't be necessary but just in case
            {
                return _tasksRepository.Create(new Task
                {
                    // Id = new Guid(task.Id), We shouldn't do this as the repository needs to create an ID for each new item and not receive it as a parameter (unless we want some custom ID logic)
                    Title = task.Title,
                    Description = task.Description,
                    IsFinished = task.IsFinished,
                    AssignedUserId = asignedUser.Id //no need to recreate the GUID we have it from the object
                });
            }
            else return false;
            
        }

        public bool Delete(string id) //chaged this function to only get an ID as that is all we need to delete an entity from the DB
        {
            return _tasksRepository.Delete(new Guid(id)); //changed the generic Repo and Interface as well since we only need an ID, no need to check if it exists, repo can handle that case
        }

        public bool Edit(TaskViewModel task) //the id will be contained in the task model itself no need to send it separatelly
        {
            User asignedUser = _usersRepository.Get(new Guid(task.AssignedUser.Id));
            if (asignedUser != null)  //make sure that the asignedUser actually exists, it shouldn't be necessary but just in case
            {
                return _tasksRepository.Update(new Task //made some changes to the repository itself, it handles a non existing DB entity update and empty objects
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
            var taskModel = _tasksRepository.Get(new Guid(id));
            return mapTaskToViewModel(taskModel);
        }

        public IEnumerable<TaskViewModel> Get()
        {
            var tasks = _tasksRepository.Get();
            return tasks.Select(t => mapTaskToViewModel(t));
        }
        // we could also use a mapping function or automaper for the transformations done in the add and edit functions but i left the as is to show what i have changed and why

        private TaskViewModel mapTaskToViewModel(Task task) {
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

    }
}
