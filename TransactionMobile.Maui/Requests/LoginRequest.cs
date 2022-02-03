using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionMobile.Maui.Services;

namespace TransactionMobile.Maui.Requests
{
    public class LoginRequest : IRequest<String>
    {
        public String UserName { get; set; }
        public String Password { get; set; }
    }
}
