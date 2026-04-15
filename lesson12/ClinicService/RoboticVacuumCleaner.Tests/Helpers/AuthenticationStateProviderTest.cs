using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RoboticVacuumCleaner.Tests.Helpers
{
    public class AuthenticationStateProviderTest : AuthenticationStateProvider
    {
        private ClaimsPrincipal _user;

        public AuthenticationStateProviderTest()
        {
            _user = new ClaimsPrincipal(new ClaimsIdentity());
        }

        public void SetAuthenticatedUser(string email, string userId = "1", string firstName = "Тест", string lastName = "Пользователь")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.GivenName, firstName),
                new Claim(ClaimTypes.Surname, lastName),
                new Claim(ClaimTypes.Name, $"{firstName} {lastName}")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            _user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void SetUnauthenticated()
        {
            _user = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_user));
        }
    }
}