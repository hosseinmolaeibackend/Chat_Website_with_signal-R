using CoreLayer.ViewModels.Chats;
using DataLayer.Entities.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Chats.ChatGroups
{
    public interface IChatGroupService
    {
        Task<List<SearchResultViewModel>> Search(string title);
        public Task<List<ChatGroup>> GetChatGroups(int id);
        public Task<ChatGroup> InsertChatGroup(CreateChatGroupViewModel model);
        public Task<ChatGroup> GetGroupBy(int id);
        public Task<ChatGroup> GetGroupBy(string token);

    }
}
