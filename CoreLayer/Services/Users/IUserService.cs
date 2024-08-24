using CoreLayer.ViewModels.authentication;
using DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Users
{
    public interface IUserService
    {
        Task<bool> IsUserExist(string username);
        Task<bool> IsUserExist(int userID);
        Task<bool> RegisterUser(RegisterViewModel viewModel);

        Task<UserChat> LoginUser(LoginViewModel loginViewModel);
    }
}
