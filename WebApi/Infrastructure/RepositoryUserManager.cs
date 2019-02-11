using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace WebApi.Infrastructure
{
    public class RepositoryUserManager :IDisposable
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

        public static RepositoryUserManager Create(IdentityFactoryOptions<RepositoryUserManager> options,
            IOwinContext context)
        {
            return new RepositoryUserManager();
        }

        public void Dispose()
        {
        }
    }
}