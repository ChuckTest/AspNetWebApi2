using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Infrastructure
{
    public class RepositoryUserManager 
    {
        public Task<RepositoryUser> FindAsync(string userName, string password)
        {
            return null;
        }

        //Rest of code is removed for brevity
        public ClaimsIdentity GenerateUserIdentityAsync(string authenticationType)
        {
            var userIdentity = new ClaimsIdentity(authenticationType); 
            // Add custom user claims here
            return userIdentity;
        }
    }
}