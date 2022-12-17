namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers
{
    using MediatR;
    using Models;
    using Requests;
    using Services;

    public class LoginRequestHandler : IRequestHandler<LoginRequest, Result<TokenResponseModel>>,
                                       IRequestHandler<RefreshTokenRequest, Result<TokenResponseModel>>,
                                       IRequestHandler<GetConfigurationRequest, Result<Configuration>>
    {
        private readonly Func<Boolean, IConfigurationService> ConfigurationServiceResolver;
        private readonly IApplicationCache ApplicationCache;
        public Func<Boolean, IAuthenticationService> AuthenticationServiceResolver { get; }

        #region Constructors
        public LoginRequestHandler(Func<Boolean, IAuthenticationService> authenticationServiceResolver,
                                   Func<Boolean, IConfigurationService> configurationServiceResolver,
                                   IApplicationCache applicationCache)
        {
            this.ConfigurationServiceResolver = configurationServiceResolver;
            this.AuthenticationServiceResolver = authenticationServiceResolver;
            this.ApplicationCache = applicationCache;
        }

        #endregion
        

        #region Methods

        public async Task<Result<TokenResponseModel>> Handle(LoginRequest request,
                                                             CancellationToken cancellationToken) {

            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            Result<TokenResponseModel> tokenResult = await authenticationService.GetToken(request.UserName, request.Password, cancellationToken);

            return tokenResult;
        }

        public async Task<Result<Configuration>> Handle(GetConfigurationRequest request,
                                                        CancellationToken cancellationToken) {

            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IConfigurationService configurationService = this.ConfigurationServiceResolver(useTrainingMode);

            return await configurationService.GetConfiguration(request.DeviceIdentifier, cancellationToken);
        }

        #endregion

        public async Task<Result<TokenResponseModel>> Handle(RefreshTokenRequest request,
                                                             CancellationToken cancellationToken)
        {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();

            IAuthenticationService authenticationService = this.AuthenticationServiceResolver(useTrainingMode);

            Result<TokenResponseModel> tokenResult = await authenticationService.RefreshAccessToken(request.RefreshToken, cancellationToken);

            return tokenResult;
        }
    }

    public abstract class Result
    {
        public bool Success { get; protected set; }
        public bool Failure => !Success;
    }

    public abstract class Result<T> : Result
    {
        private T _data;

        protected Result(T data)
        {
            Data = data;
        }

        public T Data
        {
            get => Success ? _data : throw new Exception($"You can't access .{nameof(Data)} when .{nameof(Success)} is false");
            set => _data = value;
        }
    }

    public class SuccessResult : Result
    {
        public SuccessResult()
        {
            Success = true;
        }
    }

    public class SuccessResult<T> : Result<T>
    {
        public SuccessResult(T data) : base(data)
        {
            Success = true;
        }
    }
    
    public class ErrorResult : Result, IErrorResult
    {
        public ErrorResult(string message) : this(message, Array.Empty<Error>())
        {
        }

        public ErrorResult(string message, IReadOnlyCollection<Error> errors)
        {
            Message = message;
            Success = false;
            Errors = errors ?? Array.Empty<Error>();
        }

        public string Message { get; }
        public IReadOnlyCollection<Error> Errors { get; }
    }

    public class ErrorResult<T> : Result<T>, IErrorResult
    {
        public ErrorResult(string message) : this(message, Array.Empty<Error>())
        {
            Message = message;
            Success = false;
        }

        public ErrorResult(string message, IReadOnlyCollection<Error> errors) : base(default)
        {
            
        }

        public string Message { get; set; }
    }

    public class Error
    {
        public Error(string details) : this(null, details)
        {

        }

        public Error(string code, string details)
        {
            Code = code;
            Details = details;
        }

        public string Code { get; }
        public string Details { get; }
    }

    internal interface IErrorResult
    {
        string Message { get; }
    }
}