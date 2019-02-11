using Microsoft.AspNet.Identity;

namespace WebApi.Infrastructure
{
    public class RepositoryUser : IUser
    {
        public string Id { get; }

        public string UserName { get; set; }
    }
}