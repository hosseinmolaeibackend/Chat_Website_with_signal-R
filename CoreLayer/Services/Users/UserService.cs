using CoreLayer.Utilities;
using DataLayer.Context;
using DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.ViewModels.authentication;

namespace CoreLayer.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        public UserService(EchatContext context) : base(context) { }

        public async Task<bool> IsUserExist(string username)
        {
            return await Table<UserChat>().AnyAsync(x => x.Username == username.ToLower());
        }

        public async Task<bool> IsUserExist(int userID)
        {
            return await Table<UserChat>().AnyAsync(x => x.Id == userID);
        }

        public async Task<UserChat> LoginUser(LoginViewModel loginViewModel)
        {
            var user=await Table<UserChat>().SingleOrDefaultAsync(u=>u.Username == loginViewModel.Username);
            if (user == null) return null;

            var pass = loginViewModel.Password.Encrypte();
            if (user.Password != pass) return null;

            return user;
        }

        public async Task<bool> RegisterUser(RegisterViewModel viewModel)
        {
            if (await IsUserExist(viewModel.Username)) return false;

            if (viewModel.Password != viewModel.RePassword) return false;

            var Password = viewModel.Password.Encrypte();

            var user = new UserChat() 
            {
                Username = viewModel.Username.ToLower(),
                Password = Password,
                CreateDate = DateTime.Now,
                Avatar="Default.png"
            };
            Insert(user);
            await Save();

            return true;
        }
    }
}
