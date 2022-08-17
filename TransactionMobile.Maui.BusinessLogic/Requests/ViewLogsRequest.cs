namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;

public class ViewLogsRequest : IRequest<List<Models.LogMessage>>
{
    private ViewLogsRequest() {
            
    }

    public static ViewLogsRequest Create() {
        return new ViewLogsRequest();
    }
}