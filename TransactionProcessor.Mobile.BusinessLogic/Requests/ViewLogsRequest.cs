﻿using MediatR;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public class ViewLogsRequest : IRequest<List<Models.LogMessage>>
{
    private ViewLogsRequest() {
            
    }

    public static ViewLogsRequest Create() {
        return new ViewLogsRequest();
    }
}