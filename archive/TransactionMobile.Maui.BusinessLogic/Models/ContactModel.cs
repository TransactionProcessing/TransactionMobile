using System.Diagnostics.CodeAnalysis;

namespace TransactionMobile.Maui.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class ContactModel
{
    public String EmailAddress { get; set; }
    public String Name { get; set; }
    public String MobileNumber { get; set; }
}