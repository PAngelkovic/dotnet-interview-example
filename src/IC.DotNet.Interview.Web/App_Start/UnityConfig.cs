using IC.DotNet.Interview.Core.Database;
using IC.DotNet.Interview.Core.Repositories;
using IC.DotNet.Interview.Logic.BL;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

namespace IC.DotNet.Interview.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            
            container.RegisterType<IDbContext, DbContext>(new ContainerControlledLifetimeManager());

            container.RegisterType<ITaskRepository, TaskRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IRoleRepository, RoleRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserRoleRepository, UserRoleRepository>(new ContainerControlledLifetimeManager());

            container.RegisterType<ITaskLogic, TaskLogic>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserLogic, UserLogic>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAuthorizationLogic, AuthorizationLogic>(new ContainerControlledLifetimeManager());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}