using CoreLayer.Services.Chats.ChatGroups;
using CoreLayer.Services.Users.UserGroups;
using CoreLayer.Utilities;
using CoreLayer.ViewModels.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;
using SignalRChat.Models;
using System.Diagnostics;

namespace SignalRChat.Controllers
{
    public class HomeController : Controller
    {

        private readonly IChatGroupService _chatGroupService;
        private IHubContext<ChatHub> _hubContext;
        private readonly IUserGroupService _userGroupService;
        public HomeController(IChatGroupService chatGroupService, IHubContext<ChatHub> hubContext, IUserGroupService userGroup)
        {
            _chatGroupService = chatGroupService;
            _hubContext = hubContext;
            _userGroupService = userGroup;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = await _userGroupService.GetUserGroups(User.GetUserId());
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task CreateGroup([FromForm] CreateChatGroupViewModel model)
        {
            try
            {
                model.userId = User.GetUserId();
                var result = await _chatGroupService.InsertChatGroup(model);
                await _hubContext.Clients.User(User.GetUserId().ToString())
                    .SendAsync("NewGroup", result.GroupTitle, result.GroupToken);

            }
            catch (Exception ex)
            {
                await _hubContext.Clients.User(User.GetUserId().ToString())
                    .SendAsync("NewGroup", $"{ex.Message}", "result.GroupToken");
            }
        }

        [Authorize]
        public async Task<IActionResult> Search(string title)
        {
            return new ObjectResult(await _chatGroupService.Search(title,User.GetUserId()));
        }
       
    }
}
