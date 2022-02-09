namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;

public class LogonTransactionRequest : IRequest<Boolean>
{
    public String DeviceIdentifier { get; private set; }
    public DateTime TransactionDateTime { get; private set; }
    public String TransactionNumber { get; private set; }
    public String ApplicationVersion { get; private set; }

    private LogonTransactionRequest(DateTime transactionDateTime,
                                    String transactionNumber,
                                    String deviceIdentifier,
                                    String applicationVersion)
    {
        this.ApplicationVersion=applicationVersion;
        this.DeviceIdentifier=deviceIdentifier;
        this.TransactionDateTime=transactionDateTime;
        this.TransactionNumber=transactionNumber;
    }

    public static LogonTransactionRequest Create(DateTime transactionDateTime,
                                                 String transactionNumber,
                                                 String deviceIdentifier,
                                                 String applicationVersion)
    {
        return new LogonTransactionRequest(transactionDateTime, transactionNumber, deviceIdentifier, applicationVersion);
    }

}