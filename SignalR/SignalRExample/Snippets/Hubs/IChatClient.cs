namespace SignalRExample.Snippets.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }
}
