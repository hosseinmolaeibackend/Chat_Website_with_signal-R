﻿namespace SignalRChat.Hubs
{
    public interface ICahtHub
    {
        Task JoinGroup(string token,int currentGroupId);
        Task SendMessage(string text,int groupId);
        Task JoinPrivateGroup(int userId,int currentGroupId);
    }
}
