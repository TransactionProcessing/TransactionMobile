using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.Services
{
    public interface IAuthenticationService
    {
        Task<String> GetToken(String username, String password, CancellationToken cancellationToken);
    }

    public class DummyAuthenticationService : IAuthenticationService
    {
        public async Task<String> GetToken(string username, string password, CancellationToken cancellationToken)
        {
            return "MyToken";
        }
    }
}
