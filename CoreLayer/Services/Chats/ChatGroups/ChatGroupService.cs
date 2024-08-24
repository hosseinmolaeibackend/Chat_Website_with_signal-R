using CoreLayer.Services.Users.UserGroups;
using CoreLayer.ViewModels.Chats;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Entities.Chats;
using DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreLayer.Services.Chats.ChatGroups
{
    public class ChatGroupService : BaseService, IChatGroupService
    {
        #region Dependencies
        private readonly IUserGroupService _userGroupService;
        public ChatGroupService(EchatContext context, IUserGroupService groupService) : base(context)
        {
            _userGroupService = groupService;
        }
        #endregion
        public async Task<List<ChatGroup>> GetChatGroups(int id)
        {
            return await Table<ChatGroup>().Where(i => i.OwnerId == id).OrderByDescending(i => i.CreateDate)
                .ToListAsync();
        }

        public async Task<ChatGroup> GetGroupBy(int id)
        {
            return await GetById<ChatGroup>(id);
        }

        public async Task<ChatGroup> GetGroupBy(string token)
        {
            return await Table<ChatGroup>()
                .Include(c=>c.Chats)
                .FirstOrDefaultAsync(x => x.GroupToken == token);
        }

        public async Task<ChatGroup> InsertChatGroup(CreateChatGroupViewModel model)
        {
            var chatGroup = new ChatGroup()
            {
                CreateDate = DateTime.Now,
                GroupToken = Guid.NewGuid().ToString(),
                GroupTitle = model.groupName,
                OwnerId = model.userId
            };
            Insert(chatGroup);
            await Save();
            await _userGroupService.JoinGroup(model.userId, chatGroup.Id);
            return chatGroup;
        }

        public async Task<List<SearchResultViewModel>> Search(string title)
        {
            var result = new List<SearchResultViewModel>();
            if(string.IsNullOrEmpty(title))
                return result;

            var groups = await Table<ChatGroup>().Where(x => x.GroupTitle.Contains(title))
                .Select(s => new SearchResultViewModel() 
                { Tiltle = s.GroupTitle,
                    Token = s.GroupToken,
                    IsUser = false 
                }).ToListAsync();

            var users = await Table<UserChat>().Where(x => x.Username.Contains(title))
                .Select(s => new SearchResultViewModel() 
                { Tiltle = s.Username,
                    Token = s.Id.ToString(),
                    IsUser = true 
                }).ToListAsync();

            result.AddRange(groups);
            result.AddRange(users);

            return result;
        }
    }
}
