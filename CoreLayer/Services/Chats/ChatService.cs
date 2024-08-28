using CoreLayer.ViewModels.Chats;
using DataLayer.Context;
using DataLayer.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Chats
{
    public class ChatService : BaseService, IChatService
    {
        public ChatService(EchatContext context) : base(context) { }

        public async Task<List<ChatViewModel>> GetChats(int groupId)
        {
            return await Table<Chat>().Where(x => x.GroupId == groupId)
                .Include(s => s.UserChat)
                .Include(v => v.ChatGroup)
                .Select(x => new ChatViewModel()
                {
                    UserName = x.UserChat.Username,
                    CreateDate = $"{x.CreateDate.Hour}: {x.CreateDate.Minute}",
                    GroupName = x.ChatGroup.GroupTitle,
                    UserId = x.UserId,
                    ChatBody = x.ChatBody,
                    GroupId = x.GroupId
                })
                .ToListAsync();
        }

        public async Task SendMessage(Chat _chat)
        {
            Insert(_chat);
            await Save();
        }
    }
}
