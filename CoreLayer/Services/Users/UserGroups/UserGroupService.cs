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
            var result = await Table<UserGroup>()
                .Include(x => x.ChatGroup.Chats)
                .Include(c => c.ChatGroup.Receiver)
                .Include(o => o.ChatGroup.User)
                .Where(g => g.userId == userId && !g.ChatGroup.IsPrivate)
                .ToListAsync();
            var model= new List<UserGroupViewModel>();

            foreach (var userGroup in result)
            {
                var chatGroup=userGroup.ChatGroup;
                if (chatGroup.ReceiverId != null)
                {
                    if (chatGroup.ReceiverId == userId)
                    {
                        model.Add(new UserGroupViewModel()
                        {
                            GroupName = chatGroup.User.Username,
                            token = chatGroup.GroupToken,
                            LastChat = chatGroup.Chats.OrderByDescending(d => d.Id).FirstOrDefault()
                        });
                    }
                    else
                    {
                        model.Add(new UserGroupViewModel()
                        {
                            GroupName = chatGroup.Receiver.Username,
                            token = chatGroup.GroupToken,
                            LastChat = chatGroup.Chats.OrderByDescending(d => d.Id).FirstOrDefault()
                        });
                    }
                }
                else
                {
                    model.Add(new UserGroupViewModel()
                    {
                        GroupName = chatGroup.GroupTitle,
                        token = chatGroup.GroupToken,
                        LastChat = chatGroup.Chats.OrderByDescending(d => d.Id).FirstOrDefault()
                    });
                }
            }


            return model;
        }

        public async Task<List<string>> GetUserGroupsAsync(int groupId)
        {
            return await Table<UserGroup>()
                .Where(x => x.groupId == groupId)
                .Select(x => x.userId.ToString())
                .ToListAsync();
        }

        public async Task<bool> IsUserInGroup(int userId, int groupId)
        {
            return await Table<UserGroup>().AnyAsync(x => x.groupId == groupId && x.userId == userId);
        }

        public async Task<bool> IsUserInGroup(int userId, string groupToken)
        {
            return await Table<UserGroup>()
                .Include(c => c.ChatGroup).AnyAsync(g => g.userId == userId && g.ChatGroup.GroupToken == groupToken);
        }

        public async Task JoinGroup(int userId, int groupId)
        {
            var model = new UserGroup()
            {
                CreateDate = DateTime.Now,
                groupId = groupId
                ,
                userId = userId,

            };
            Insert(model);
            await Save();
        }

        public async Task JoinGroup(List<int> userIds, int groupId)
        {
            foreach (int userId in userIds)
            {
                var model = new UserGroup()
                {
                    CreateDate = DateTime.Now,
                    groupId = groupId
                   ,
                    userId = userId,

                };
                Insert(model);
            }
            await Save();
        }
    }
}
