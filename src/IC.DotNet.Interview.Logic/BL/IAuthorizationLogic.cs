using IC.DotNet.Interview.Logic.Models;

namespace IC.DotNet.Interview.Logic.BL
{
    public interface IAuthorizationLogic
    {
        bool TryLogin(LoginViewModel loginViewModel);
        bool TryLogout();
        LoginButtonViewModel GetLoginButton();
        bool IsUserLoggedIn();
    }
}
