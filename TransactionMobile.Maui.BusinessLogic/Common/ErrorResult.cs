namespace TransactionMobile.Maui.BusinessLogic.Common;

using Newtonsoft.Json;
using SimpleResults;

public static class ResultExtensions
{
    public static Result FailureExtended(string message, Exception exception){
        return Result.Failure(message,
                              new List<String>{
                                                  exception.Message
                                              });
    }
}