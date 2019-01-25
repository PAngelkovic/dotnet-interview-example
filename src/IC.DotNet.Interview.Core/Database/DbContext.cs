using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using IC.DotNet.Interview.Core.Models;
using Newtonsoft.Json;

namespace IC.DotNet.Interview.Core.Database
{
    public class DbContext : IDbContext
    {
        private static readonly string DATABASE_PATH = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings["DBPath"]);
        //had to change this from cons to static readonly because AppSettings aren't known before compiling
        //HttpContext.Current.Request.PhysicalApplicationPath is added to make sure that the file is always located withing the project folder
        //Added the file path in the web.config file as a variable so it can be changed without editing code/beeing hardcoded

        private Database _database;

        public DbContext()
        {
            LoadDatabase();
        }
        
        public ICollection<T> GetDataset<T>()
        {
            var property = _database.GetType()
                .GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                    p.PropertyType.GenericTypeArguments?[0] == typeof(T))
                .FirstOrDefault();

            return (ICollection<T>)property.GetValue(_database);
        }

        public bool Save()
        {
            if (_database == null)
                return false;

            string json = JsonConvert.SerializeObject(_database);

            if(File.Exists(DATABASE_PATH))
                File.Delete(DATABASE_PATH); //we probably don't need this since File.WriteAllText overwrittes everything in the file

            File.WriteAllText(DATABASE_PATH, json);

            return true;
        }

        private void LoadDatabase()
        {
            if (!File.Exists(DATABASE_PATH))
            {
                _database = InitializeDatabase();
                Save();
            }

            string json = File.ReadAllText(DATABASE_PATH);
            _database = JsonConvert.DeserializeObject<Database>(json);
        }

        private Database InitializeDatabase()
        {
            var database = new Database();

            database.Tasks = new List<Task>();
            database.Users = new List<User>();
            database.Roles = new List<Role>();
            database.UserRoles = new List<UserRole>();

            var administratorRoleGuid = Guid.NewGuid();
            var editorRoleGuid = Guid.NewGuid();
            var adminUserGuid = Guid.NewGuid();
            var candidateUserGuid = Guid.NewGuid();

            //Roles
            database.Roles.Add(new Role
            {
                Id = administratorRoleGuid,
                Name = "administrator"
            });

            database.Roles.Add(new Role
            {
                Id = editorRoleGuid,
                Name = "editor"
            });

            //Users
            database.Users.Add(new User
            {
                Id = adminUserGuid,
                Username = "interviewer",
                Password = "interviewer"
            });

            database.Users.Add(new User
            {
                Id = candidateUserGuid,
                Username = "candidate",
                Password = "candidate"
            });

            //User to Role Mappings
            database.UserRoles.Add(new UserRole
            {
                RoleId = administratorRoleGuid,
                UserId = adminUserGuid
            });

            database.UserRoles.Add(new UserRole
            {
                RoleId = editorRoleGuid,
                UserId = candidateUserGuid
            });

            //News
            database.Tasks.Add(new Task
            {
                Id = Guid.NewGuid(),
                AssignedUserId = candidateUserGuid,
                Title = "Start the Solution from your Visual Studio",
                Description = "Well done! If you see this task in your browser you already finished the first task.<br />" +
                "Now you can start with the tasks below mark it later as finished when you built the functionality for it.<br />" +
                "Make sure you follow the development guidelines from the Readme.md in the root of this solution.<br />" +
                "Good luck!"
            });

            database.Tasks.Add(new Task
            {
                Id = Guid.NewGuid(),
                AssignedUserId = candidateUserGuid,
                Title = "Refactor TaskLogic",
                Description = "IC.DotNet.Interview.Logic.BL.TaskLogic has some flaws and needs a refactor.<br />" +
                "Keep the functionality of all the methods but make sure its clean code."
            });

            database.Tasks.Add(new Task
            {
                Id = Guid.NewGuid(),
                AssignedUserId = candidateUserGuid,
                Title = "Edit functionality for tasks",
                Description = "The 'Edit' button below should lead to the edit functionality of a task.<br />" +
                "Build everything needed so the user can edit an existing task."
            });

            database.Tasks.Add(new Task
            {
                Id = Guid.NewGuid(),
                AssignedUserId = candidateUserGuid,
                Title = "Store DateCreated, LastUpdated, UserCreatedId, UserLastUpdatedId for all datatypes",
                Description = "Make sure the database stores for all data types the date and time when an entry was created and last updated<br />" +
                "Also store the user which created and last updated an element in the database."
            });

            database.Tasks.Add(new Task
            {
                Id = Guid.NewGuid(),
                AssignedUserId = candidateUserGuid,
                Title = "Build authentication functionality",
                Description = "The Login functionality already exists<br />" +
                "In the login form a user can enter username and password (use username: candidate and password: candidate for your tests), when the credentials were correct, the backend stores the user id of the current user in a cookie in the browser.<br />" +
                "When the user clicks logout, the cookie is deleted.<br />" +
                "Now build the functionality which ensures the following:<br />" +
                "-tasks can only be seen if logged in<br />" +
                "-a user can only see/edit/delete the tasks that are assigned to it<br />" +
                "-a user with the role administrator can see every task<br />" +
                "-only logged in users can create tasks<br />" +
                "-if a creator of a task doesn't assign it, it will be assigned to the creator"
            });

            database.Tasks.Add(new Task
            {
                Id = Guid.NewGuid(),
                AssignedUserId = candidateUserGuid,
                Title = "Build commentary function",
                Description = "Build a function which allows to comment in tasks.<br />" +
                "The UI should display all comments (with the create date and the user who created it)."
            });

            database.Tasks.Add(new Task
            {
                Id = Guid.NewGuid(),
                AssignedUserId = candidateUserGuid,
                Title = "Answer questions about the architecture of the solution",
                Description = "Answer the questions below in the comments of the task (if you didn't build the comments module just add your answers in a text file to the git repository):<br />" +
                "Would you use the architecture of the solution (or parts of it) in a real project?<br />" +
                "Why would you use which parts of the solution? Why not?<br />" +
                "What would you change? How would you change it?"
            });
            
            database.Tasks.Add(new Task
            {
                Id = Guid.NewGuid(),
                AssignedUserId = adminUserGuid,
                Title = "Review the work of the interview candidate",
                Description = "This task is not for you as the candidate of the interview<br />" +
                "It is here for you to check if the authorization works cause it is assigned to the interviewer.<br />" +
                "When you fix the authorization task you should not see this anymore as a candidate."
            });

            return database;
        }
    }
}
