namespace WebApi.Infrastructure
{
    public class ClientManager
    {
        public static Client FindClient(string clientId)
        {
            Client client = new Client();
            client.ClientId = clientId;
            return client;
        }
    }
}