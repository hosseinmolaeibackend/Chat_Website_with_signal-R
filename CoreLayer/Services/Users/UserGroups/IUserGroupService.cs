using CoreLayer.ViewModels.Chats;
using System;
using DataLayer.Entities.User;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Users.UserGroups
{
    public interface IUserGroupService
    {
        Task<List<UserGroupViewModel>> GetUserGroups(int userId);
        Task JoinGroup(int userId,int groupId);
        Task<bool> IsUserInGroup(int userId,int groupId);
        Task<bool> IsUserInGroup(int userId, string groupToken);

    }
}
