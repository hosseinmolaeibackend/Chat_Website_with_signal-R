namespace SignalRChat.Hubs
{
    public interface ICahtHub
    {
        Task JoinGroup(string token,int id);
        Task SendMessage(string text,int groupId);
    }
}
