﻿using DataLayer.Context;
using DataLayer.Entities.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Chats
{
    public class ChatService : BaseService, IChatService
    {
        public ChatService(EchatContext context): base(context) {}

        public async Task SendMessage(Chat _chat)
        {
            Insert(_chat);
            await Save();
        }
    }
}