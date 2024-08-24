using CoreLayer.Services.Users.UserGroups;
using CoreLayer.ViewModels.Chats;
using DataLayer.Context;
using DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Users.UserGroups
{
    public class UserGroupService : BaseService, IUserGroupService
    {
        public UserGroupService(EchatContext context) : base(context)
        {
        }

        public async Task<List<UserGroupViewModel>> GetUserGroups(int userId)
        {
            var result = Table<UserGroup>().Include(x => x.ChatGroup.Chats)
                .Where(g => g.userId == userId).Select(s => new UserGroupViewModel()
                {
                    GroupName = s.ChatGroup.GroupTitle,
                    token = s.ChatGroup.GroupToken,
                    LastChat = s.ChatGroup.Chats.OrderBy(s => s.CreateDate).First()
                });

            return await result.ToListAsync();
        }

        public async Task<bool> IsUserInGroup(int userId, int groupId)
        {
            return await Table<UserGroup>().AnyAsync(x=>x.groupId == groupId && x.userId==userId);
        }

        public async Task<bool> IsUserInGroup(int userId, string groupToken)
        {
            return await Table<UserGroup>()
                .Include(c => c.ChatGroup).AnyAsync(g => g.userId == userId && g.ChatGroup.GroupToken == groupToken);
        }

        public async Task JoinGroup(int userId, int groupId)
        {
            var model=new UserGroup()
            {
                CreateDate = DateTime.Now,
                groupId = groupId
                ,userId = userId,
                
            };
            Insert(model);
            await Save();
        }
    }
}
