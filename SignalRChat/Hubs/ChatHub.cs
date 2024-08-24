using CoreLayer.Services.Chats;
using CoreLayer.Services.Chats.ChatGroups;
using CoreLayer.Services.Users.UserGroups;
using CoreLayer.Utilities;
using DataLayer.Entities.Chats;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub, ICahtHub
    {
        IChatGroupService _chatGroupService;
        IUserGroupService _userGroupService;
        IChatService _chatService;
        public ChatHub(IChatGroupService chatGroupService, IUserGroupService userGroupService, IChatService chatService)
        {
            _chatGroupService = chatGroupService;
            _userGroupService = userGroupService;
            _chatService = chatService;
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Test", Context.User.GetUserId());
            return base.OnConnectedAsync();
        }

        public async Task JoinGroup(string token, int currentId)
        {
            var group = await _chatGroupService.GetGroupBy(token);
            if (group == null)
            {
                await Clients.Caller.SendAsync("Errorr", "Group Notfound");
            }
            else
            {
                if (!await _userGroupService.IsUserInGroup(Context.User.GetUserId(), token))
                {
                    await _userGroupService.JoinGroup(Context.User.GetUserId(), group.Id);
                    await Clients.Caller.SendAsync("NewGroup", group.GroupTitle, group.GroupToken);
                }

                if (currentId > 0)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentId.ToString());

                }

                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                await Clients.Group(group.Id.ToString()).SendAsync("JoinGroup", group, group.Chats);

            }
        }

        public async Task SendMessage(string text, int groupId)
        {
            var chat = new Chat()
            {
                GroupId = groupId,
                ChatBody = text,
                CreateDate = DateTime.Now,
                UserId= Context.User.GetUserId()
            };
            await _chatService.SendMessage(chat);
            chat.CreateDate = DateTime.Now.Date;
            await Clients.Group(groupId.ToString()).SendAsync("ReceivedMessage", chat);
        }
    }
}
