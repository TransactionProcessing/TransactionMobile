using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Models;

public class TokenResponseModel
{
    public String AccessToken { get; set; }
    public String RefreshToken { get; set; }
    public Int64 ExpiryInMinutes { get; set; }
}