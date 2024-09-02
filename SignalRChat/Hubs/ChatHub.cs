using CoreLayer.Services.Chats;
using CoreLayer.Services.Chats.ChatGroups;
using CoreLayer.Services.Users.UserGroups;
using CoreLayer.Utilities;
using CoreLayer.ViewModels.Chats;
using DataLayer.Entities.Chats;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub, ICahtHub
    {
        #region Dependency
        IChatGroupService _chatGroupService;
        IUserGroupService _userGroupService;
        IChatService _chatService;
        public ChatHub(IChatGroupService chatGroupService, IUserGroupService userGroupService, IChatService chatService)
        {
            _chatGroupService = chatGroupService;
            _userGroupService = userGroupService;
            _chatService = chatService;
        }
        #endregion
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Test", Context.User.GetUserId());
            return base.OnConnectedAsync();
        }

        public async Task JoinGroup(string token, int currentGroupId)
        {
            var group = await _chatGroupService.GetGroupBy(token);
            var groupDto = FixGroupModel(group);
            if (group == null)
            {
                await Clients.Caller.SendAsync("Errorr", "Group Notfound");
            }
            else
            {
                var chats = await _chatService.GetChats(groupDto.Id);
                if (!await _userGroupService.IsUserInGroup(Context.User.GetUserId(), token))
                {
                    await _userGroupService.JoinGroup(Context.User.GetUserId(), group.Id);
                    await Clients.Caller.SendAsync("NewGroup", groupDto.GroupTitle, groupDto.GroupToken);
                }

                if (currentGroupId > 0)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroupId.ToString());
                }


                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                await Clients.Group(group.Id.ToString()).SendAsync("JoinGroup", groupDto, chats);

            }
        }

        public async Task SendMessage(string text, int groupId)
        {
            var group = await _chatGroupService.GetGroupBy(groupId);
            if (group == null)
                return;
            var chat = new Chat()
            {
                GroupId = groupId,
                ChatBody = text,
                CreateDate = DateTime.Now,
                UserId = Context.User.GetUserId()
            };
            await _chatService.SendMessage(chat);
            chat.CreateDate = DateTime.Now.Date;
            var chatModel = new ChatViewModel()
            {
                ChatBody = text,
                CreateDate = $"{chat.CreateDate.Hour}:{chat.CreateDate.Minute}",
                UserName = Context.User.GetUserName(),
                UserId = Context.User.GetUserId(),
                GroupName = group.GroupTitle,
                GroupId = groupId,
            };

            var userIds = await _userGroupService.GetUserGroupsAsync(groupId);

            await Clients.Users(userIds).SendAsync("ReceivedNotification", chatModel);
            await Clients.Group(groupId.ToString()).SendAsync("ReceivedMessage", chatModel);
        }

        public async Task JoinPrivateGroup(int userId, int currentGroupId)
        {
            if (currentGroupId > 0)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroupId.ToString());
            }
            var group = await _chatGroupService.InsertPrivateChatGroup(Context.User.GetUserId(), userId);
            var groupDto = FixGroupModel(group);
            if (!await _userGroupService.IsUserInGroup(Context.User.GetUserId(), group.GroupToken))
            {
                await _userGroupService.JoinGroup(new List<int>()
                { groupDto.ReceiverId??0, group.OwnerId}, group.Id);


                await Clients.Caller.SendAsync("NewGroup", groupDto.GroupTitle, groupDto.GroupToken);
                await Clients.User(groupDto.ReceiverId.ToString()).SendAsync("NewGroup", Context.User.GetUserName(), groupDto.GroupToken);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());

            var chats = await _chatService.GetChats(group.Id);

            await Clients.Group(group.Id.ToString()).SendAsync("JoinGroup", groupDto, chats);
        }


        #region utils
        private ChatGroup FixGroupModel(ChatGroup chatGroup)
        {
            if (chatGroup.IsPrivate)
            {
                if (chatGroup.OwnerId == Context.User.GetUserId())
                {
                    return new ChatGroup()
                    {
                        Id = chatGroup.Id,
                        GroupToken = chatGroup.GroupToken,
                        CreateDate = chatGroup.CreateDate,
                        GroupTitle = chatGroup.Receiver.Username,

                        IsPrivate = false,
                        OwnerId = chatGroup.OwnerId,
                        ReceiverId = chatGroup.ReceiverId
                    };
                }
                return new ChatGroup()
                {
                    Id = chatGroup.Id,
                    GroupToken = chatGroup.GroupToken,
                    CreateDate = chatGroup.CreateDate,

                    IsPrivate = false,
                    OwnerId = chatGroup.OwnerId,
                    ReceiverId = chatGroup.ReceiverId
                };
            }
            return new ChatGroup()
            {
                Id = chatGroup.Id,
                GroupToken = chatGroup.GroupToken,
                CreateDate = chatGroup.CreateDate,
                GroupTitle = chatGroup.GroupTitle,

                IsPrivate = false,
                OwnerId = chatGroup.OwnerId,
                ReceiverId = chatGroup.ReceiverId

                #endregion
            };
            }
    }
}
