using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecurityService.DataTransferObjects;
using TransactionProcessorACL.DataTransferObjects;
using TransactionProcessorACL.DataTransferObjects.Responses;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TransactionProcessor.Mobile.UiTestBackend;

public sealed class TestBackendHost : IAsyncDisposable
{
    private readonly WebApplication app;
    private readonly string loopbackBaseUrl;
    private readonly string hostBaseUrl;
    private readonly BackendState state;

    private TestBackendHost(WebApplication app, string hostBaseUrl, int port)
    {
        this.app = app;
        this.hostBaseUrl = hostBaseUrl.TrimEnd('/');
        this.loopbackBaseUrl = $"http://127.0.0.1:{port}";
        this.state = app.Services.GetRequiredService<BackendState>();
        this.Port = port;
    }

    public int Port { get; private set; }

    public string WindowsBaseUrl => this.loopbackBaseUrl;

    public string AndroidBaseUrl => this.loopbackBaseUrl.Replace("127.0.0.1", "10.0.2.2", StringComparison.OrdinalIgnoreCase);

    public string ConfigHostUrlForPlatform(string platform) =>
        platform.Equals("android", StringComparison.OrdinalIgnoreCase)
            ? this.AndroidBaseUrl
            : this.WindowsBaseUrl;

    public HttpClient CreateClient() => new() { BaseAddress = new Uri(this.loopbackBaseUrl) };

    public BackendSeed CurrentSeed => this.state.Seed.CloneSeed();

    public void ApplySeed(BackendSeed seed) => this.state.ReplaceSeed(seed);

    public void Reset() => this.state.Reset();

    public void SetDeviceMapping(string deviceIdentifier) => this.state.RegisterDevice(deviceIdentifier);

    public string[] GetRequestTraceSnapshot() => this.state.GetRequestTraceSnapshot();

    public static async Task<TestBackendHost> StartAsync(int port, BackendSeed? seed = null, CancellationToken cancellationToken = default)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = ["--urls", $"http://0.0.0.0:{port}"]
        });

        builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
        builder.Services.AddSingleton<BackendState>();
        builder.Services.AddRouting();
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            options.SerializerOptions.Converters.Add(new DateTimeSpaceConverter());
        });

        WebApplication app = builder.Build();
        ConfigureRoutes(app);

        var host = new TestBackendHost(app, $"http://127.0.0.1:{port}", port);
        host.ApplySeed(seed ?? new BackendSeed());

        await app.StartAsync(cancellationToken).ConfigureAwait(false);
        await host.WaitUntilReadyAsync(cancellationToken).ConfigureAwait(false);
        return host;
    }

    public static async Task RunAsync(string[] args, CancellationToken cancellationToken = default)
    {
        int port = 5055;
        string? portArgument = args.FirstOrDefault(arg => int.TryParse(arg, out _));
        if (string.IsNullOrWhiteSpace(portArgument) == false && int.TryParse(portArgument, out int parsedPort))
        {
            port = parsedPort;
        }
        else if (int.TryParse(Environment.GetEnvironmentVariable("BACKEND_PORT"), out int envPort))
        {
            port = envPort;
        }

        await using TestBackendHost host = await StartAsync(port, cancellationToken: cancellationToken).ConfigureAwait(false);
        await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        await this.app.StopAsync().ConfigureAwait(false);
        await this.app.DisposeAsync().ConfigureAwait(false);
    }

    private static void ConfigureRoutes(WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            BackendState state = context.RequestServices.GetRequiredService<BackendState>();
            string requestPayload = await ReadRequestBodyAsync(context.Request).ConfigureAwait(false);
            state.RecordRequest($">>> {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
            state.RecordRequest($">>> REQUEST PAYLOAD {FormatTracePayload(requestPayload)}");

            Stream originalResponseBody = context.Response.Body;
            await using MemoryStream responseBuffer = new();
            context.Response.Body = responseBuffer;
            try
            {
                await next().ConfigureAwait(false);

                responseBuffer.Position = 0;
                using StreamReader reader = new(responseBuffer, Encoding.UTF8, leaveOpen: true);
                string responsePayload = await reader.ReadToEndAsync().ConfigureAwait(false);
                responseBuffer.Position = 0;
                await responseBuffer.CopyToAsync(originalResponseBody).ConfigureAwait(false);

                state.RecordRequest($"<<< {(int)context.Response.StatusCode} {context.Request.Method} {context.Request.Path}");
                state.RecordRequest($"<<< RESPONSE PAYLOAD {FormatTracePayload(responsePayload)}");
            }
            finally
            {
                context.Response.Body = originalResponseBody;
            }
        });

        app.MapGet("/health", (BackendState state) =>
        {
            return Results.Ok(new { status = "ok" });
        });

        app.MapPost("/api/test/reset", (BackendState state) =>
        {
            state.Reset();
            return Results.NoContent();
        });

        app.MapPost("/api/test/seed", (BackendState state, BackendSeed seed) =>
        {
            state.ReplaceSeed(seed);
            return Results.NoContent();
        });

        app.MapPost("/api/test/device/{deviceIdentifier}", (BackendState state, string deviceIdentifier) =>
        {
            state.RegisterDevice(deviceIdentifier);
            return Results.NoContent();
        });

        app.MapGet("/api/transactionmobileconfiguration/{deviceIdentifier}", (HttpContext context, BackendState state, string deviceIdentifier) =>
        {
            state.RegisterDevice(deviceIdentifier);
            string baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";
            return Results.Ok(state.BuildConfiguration(deviceIdentifier, baseUrl));
        });

        app.MapPost("/api/transactionmobileconfiguration/{deviceIdentifier}", (BackendState state, string deviceIdentifier) =>
        {
            state.RegisterDevice(deviceIdentifier);
            return Results.NoContent();
        });

        app.MapPost("/api/applicationcentreconfiguration", () => Results.Ok());
        app.MapPost("/api/applicationupdates/check", () => Results.Ok(new ApplicationUpdateCheckResponse
        {
            UpdateRequired = false,
            DownloadUri = string.Empty,
            LatestVersion = string.Empty,
            Message = string.Empty
        }));

        app.MapPost("/connect/token", async (HttpContext context, BackendState state) =>
        {
            object response = await state.HandleTokenRequestAsync(context.Request).ConfigureAwait(false);
            return Results.Ok(response);
        });

        app.MapGet("/api/merchants/contracts", (BackendState state) =>
        {
            return Results.Ok(state.BuildContractResponses());
        });

        app.MapGet("/api/merchants", (BackendState state) =>
        {
            return Results.Ok(state.BuildMerchantResponse());
        });

        app.MapPost("/api/logontransactions", async (BackendState state, LogonTransactionRequestMessage request) =>
        {
            LogonTransactionResponseMessage response = await state.HandleLogonAsync(request).ConfigureAwait(false);
            return Results.Ok(response);
        });

        app.MapPost("/api/saletransactions", async (BackendState state, SaleTransactionRequestMessage request) =>
        {
            SaleTransactionResponseMessage response = await state.HandleSaleAsync(request).ConfigureAwait(false);
            return Results.Ok(response);
        });

        app.MapPost("/api/reconciliationtransactions", async (BackendState state, ReconciliationRequestMessage request) =>
        {
            ReconciliationResponseMessage response = await state.HandleReconciliationAsync(request).ConfigureAwait(false);
            return Results.Ok(response);
        });

        app.MapPost("/api/reporting/dailymerchantprformancesummary", (BackendState state, MerchantDailyPerformanceSummaryRequest request) =>
        {
            return Results.Ok(state.BuildDailyPerformanceSummary(request));
        });

        app.MapPost("/api/reporting/transactionmixsummary", (BackendState state, TransactionMixSummaryRequest request) =>
        {
            return Results.Ok(state.BuildTransactionMixSummary(request));
        });

        app.MapPost("/api/reporting/recentactivityreceiptsearch", (BackendState state, RecentActivityReceiptSearchRequest request) =>
        {
            return Results.Ok(state.BuildRecentActivityReceiptSearch(request));
        });

        app.MapPost("/api/transactions/resendreceipt", (BackendState state, ResendReceiptRequest request) =>
        {
            return Results.Ok(state.ResendReceipt(request));
        });

        app.MapPost("/api/transactionmobilelogging/{deviceIdentifier}", (BackendState state, string deviceIdentifier, LogEnvelope envelope) =>
        {
            state.RecordLogs(deviceIdentifier, envelope.Messages);
            return Results.NoContent();
        });
    }

    private async Task WaitUntilReadyAsync(CancellationToken cancellationToken)
    {
        using HttpClient client = new() { BaseAddress = new Uri(this.loopbackBaseUrl) };
        for (int i = 0; i < 50; i++)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("/health", cancellationToken).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch
            {
                // Retry until the host is accepting requests.
            }

            await Task.Delay(100, cancellationToken).ConfigureAwait(false);
        }

        throw new TimeoutException("Backend host did not become ready in time.");
    }

    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        string body = await reader.ReadToEndAsync().ConfigureAwait(false);
        request.Body.Position = 0;
        return FormatTracePayload(body);
    }

    private static string FormatTracePayload(string? payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            return "<empty>";
        }

        return payload
            .Replace("\r\n", "\\n", StringComparison.Ordinal)
            .Replace("\n", "\\n", StringComparison.Ordinal);
    }

    private sealed class DateTimeSpaceConverter : JsonConverter<DateTime>
    {
        private static readonly string[] AcceptedFormats =
        {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd H:mm:ss",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss.FFFFFFFK",
            "o"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return default;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    return default;
                }

                if (DateTime.TryParseExact(
                        value,
                        AcceptedFormats,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.AssumeLocal | System.Globalization.DateTimeStyles.AllowWhiteSpaces,
                        out DateTime exact))
                {
                    return exact;
                }

                if (DateTime.TryParse(
                        value,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.AssumeLocal,
                        out DateTime parsed))
                {
                    return parsed;
                }

                throw new JsonException($"Unable to parse DateTime: '{value}'.");
            }

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long seconds))
            {
                return DateTimeOffset.FromUnixTimeSeconds(seconds).LocalDateTime;
            }

            throw new JsonException($"Unexpected token parsing DateTime. Token: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}

internal sealed class BackendState
{
    private readonly object gate = new();
    private readonly Dictionary<string, string> deviceToMerchant = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<BackendRuntimeReceipt> runtimeReceipts = new();
    private readonly List<BackendRuntimeLog> runtimeLogs = new();
    private readonly List<string> requestTrace = new();

    public BackendSeed Seed { get; private set; } = new();

    public void Reset()
    {
        lock (this.gate)
        {
            this.Seed = new BackendSeed();
            this.deviceToMerchant.Clear();
            this.runtimeReceipts.Clear();
            this.runtimeLogs.Clear();
            this.requestTrace.Clear();
        }
    }

    public void ReplaceSeed(BackendSeed seed)
    {
        lock (this.gate)
        {
            this.Seed = seed.CloneSeed();
            this.runtimeReceipts.Clear();
            this.runtimeLogs.Clear();
            this.deviceToMerchant.Clear();

            this.runtimeReceipts.AddRange(this.Seed.PreseededReceipts.Select(receipt => new BackendRuntimeReceipt(
                receipt.Reference,
                receipt.ReceiptReference,
                receipt.TransactionType,
                receipt.Product,
                receipt.Operator,
                receipt.Status,
                receipt.Amount,
                receipt.TransactionDateTime)));

            this.runtimeReceipts.AddRange(this.Seed.ReportTransactions.Select(transaction => new BackendRuntimeReceipt(
                transaction.Reference,
                transaction.ReceiptReference,
                transaction.TransactionType,
                transaction.Product,
                transaction.Operator,
                transaction.Status,
                transaction.Amount,
                transaction.TransactionDateTime)));

            this.runtimeReceipts.AddRange(this.Seed.Deposits.Select(deposit => new BackendRuntimeReceipt(
                deposit.Reference,
                $"RCPT-{deposit.Reference}",
                "Deposit",
                "Deposit",
                "Merchant",
                "Success",
                deposit.Amount,
                deposit.DateTime)));
        }
    }

    public void RecordRequest(string message)
    {
        lock (this.gate)
        {
            this.requestTrace.Add($"{DateTime.UtcNow:O} {message}");
        }
    }

    public string[] GetRequestTraceSnapshot()
    {
        lock (this.gate)
        {
            return this.requestTrace.ToArray();
        }
    }

    public void RegisterDevice(string deviceIdentifier)
    {
        lock (this.gate)
        {
            MerchantSeed? merchant = this.Seed.Merchants.FirstOrDefault();
            if (merchant != null)
            {
                this.deviceToMerchant[deviceIdentifier] = merchant.MerchantName;

                int deviceIndex = this.Seed.Devices.FindIndex(device =>
                    string.Equals(device.EstateName, merchant.EstateName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(device.MerchantName, merchant.MerchantName, StringComparison.OrdinalIgnoreCase));

                if (deviceIndex >= 0)
                {
                    DeviceSeed existingDevice = this.Seed.Devices[deviceIndex];
                    this.Seed.Devices[deviceIndex] = existingDevice with
                    {
                        DeviceIdentifier = deviceIdentifier
                    };
                }
                else
                {
                    this.Seed.Devices.Add(new DeviceSeed
                    {
                        EstateName = merchant.EstateName,
                        MerchantName = merchant.MerchantName,
                        DeviceIdentifier = deviceIdentifier
                    });
                }
            }
        }
    }

    public object BuildConfiguration(string deviceIdentifier, string baseUrl)
    {
        lock (this.gate)
        {
            ClientSeed? appClient = this.Seed.Clients.FirstOrDefault(c => c.IsAppClient) ?? this.Seed.Clients.FirstOrDefault();
            return new
            {
                clientId = appClient?.ClientId ?? "mobileAppClient",
                clientSecret = appClient?.Secret ?? "Secret1",
                deviceIdentifier,
                enableAutoUpdates = false,
                applicationUpdateUri = string.Empty,
                hostAddresses = new[]
                {
                    new { serviceType = 1, uri = baseUrl },
                    new { serviceType = 2, uri = baseUrl }
                },
                id = deviceIdentifier,
                logLevel = 3,
                logMessageBatchSize = 50,
                sentryDsn = string.Empty
            };
        }
    }

    public async Task<object> HandleTokenRequestAsync(HttpRequest request)
    {
        IFormCollection form = await request.ReadFormAsync().ConfigureAwait(false);
        string grantType = form["grant_type"].ToString();
        string clientId = form["client_id"].ToString();
        string clientSecret = form["client_secret"].ToString();

        return grantType switch
        {
            "password" => this.BuildToken(clientId, clientSecret, form["username"].ToString(), form["password"].ToString()),
            "refresh_token" => this.BuildRefreshToken(clientId, clientSecret, form["refresh_token"].ToString()),
            "client_credentials" => this.BuildClientCredentialsToken(clientId, clientSecret),
            _ => throw new InvalidOperationException($"Unsupported grant type '{grantType}'.")
        };
    }

    public List<ContractResponse> BuildContractResponses()
    {
        lock (this.gate)
        {
            return this.Seed.Contracts
                .GroupBy(c => new { c.EstateName, c.OperatorName, c.ContractDescription })
                .Select(group =>
                {
                    var products = this.Seed.Products
                        .Where(p => string.Equals(p.EstateName, group.Key.EstateName, StringComparison.OrdinalIgnoreCase)
                                    && string.Equals(p.OperatorName, group.Key.OperatorName, StringComparison.OrdinalIgnoreCase)
                                    && string.Equals(p.ContractDescription, group.Key.ContractDescription, StringComparison.OrdinalIgnoreCase))
                        .Select(p => new ContractProduct
                        {
                            DisplayText = p.DisplayText,
                            Name = p.ProductName,
                            ProductId = SeedIdGenerator.CreateGuid($"product:{p.EstateName}:{p.OperatorName}:{p.ContractDescription}:{p.ProductName}"),
                            ProductReportingId = SeedIdGenerator.CreateInt($"product-report:{p.EstateName}:{p.OperatorName}:{p.ContractDescription}:{p.ProductName}"),
                            ProductType = ParseProductType(p.ProductType),
                            Value = p.Value,
                            TransactionFees = p.FeeValue.HasValue ? [new ContractProductTransactionFee
                            {
                                CalculationType = ParseCalculationType(p.CalculationType),
                                Description = p.FeeDescription ?? "Merchant Commission",
                                Value = p.FeeValue.Value
                            }] : []
                        })
                        .ToList();

                    return new ContractResponse
                    {
                        ContractId = SeedIdGenerator.CreateGuid($"contract:{group.Key.EstateName}:{group.Key.OperatorName}:{group.Key.ContractDescription}"),
                        ContractReportingId = SeedIdGenerator.CreateInt($"contract-report:{group.Key.EstateName}:{group.Key.OperatorName}:{group.Key.ContractDescription}"),
                        Description = group.Key.ContractDescription,
                        EstateId = this.GetEstateId(group.Key.EstateName),
                        EstateReportingId = this.GetEstateReportingId(group.Key.EstateName),
                        OperatorId = this.GetOperatorId(group.Key.EstateName, group.Key.OperatorName),
                        OperatorName = group.Key.OperatorName,
                        Products = products
                    };
                })
                .ToList();
        }
    }

    public MerchantResponse BuildMerchantResponse()
    {
        lock (this.gate)
        {
            MerchantSeed merchant = this.Seed.Merchants.FirstOrDefault()
                ?? new MerchantSeed
                {
                    EstateName = "Default Estate",
                    MerchantName = "Dummy Merchant",
                    AddressLine1 = "test address line 1",
                    AddressLine2 = "test address line 2",
                    AddressLine3 = "test address line 3",
                    AddressLine4 = "test address line 4",
                    Town = "Town",
                    Region = "Region",
                    PostalCode = "TE57 1NG",
                    Country = "United Kingdom",
                    ContactName = "Test Contact",
                    ContactEmailAddress = "test@example.com",
                    ContactPhoneNumber = "123456789"
                };

            string estateName = merchant.EstateName;
            return new MerchantResponse
            {
                EstateId = this.GetEstateId(estateName),
                EstateReportingId = this.GetEstateReportingId(estateName),
                MerchantId = this.GetMerchantId(estateName, merchant.MerchantName),
                MerchantReportingId = this.GetMerchantReportingId(estateName, merchant.MerchantName),
                MerchantName = merchant.MerchantName,
                MerchantReference = merchant.MerchantName,
                NextStatementDate = DateTime.UtcNow.Date.AddDays(30),
                SettlementSchedule = ParseSettlementSchedule(merchant.SettlementSchedule),
                Addresses =
                [
                    new AddressResponse
                    {
                        AddressId = SeedIdGenerator.CreateGuid($"address:{estateName}:{merchant.MerchantName}"),
                        AddressLine1 = merchant.AddressLine1,
                        AddressLine2 = merchant.AddressLine2,
                        AddressLine3 = merchant.AddressLine3,
                        AddressLine4 = merchant.AddressLine4,
                        Town = merchant.Town,
                        Region = merchant.Region,
                        PostalCode = merchant.PostalCode,
                        Country = merchant.Country
                    }
                ],
                Contacts =
                [
                    new ContactResponse
                    {
                        ContactId = SeedIdGenerator.CreateGuid($"contact:{estateName}:{merchant.MerchantName}"),
                        ContactName = merchant.ContactName,
                        ContactEmailAddress = merchant.ContactEmailAddress,
                        ContactPhoneNumber = merchant.ContactPhoneNumber
                    }
                ],
                Devices = this.GetMerchantDevices(estateName, merchant.MerchantName),
                Operators = this.BuildMerchantOperators(estateName, merchant.MerchantName),
                Contracts = this.BuildMerchantContracts(estateName, merchant.MerchantName),
                OpeningHours = new Dictionary<DayOfWeek, OpeningHoursResponse>()
            };
        }
    }

    public async Task<LogonTransactionResponseMessage> HandleLogonAsync(LogonTransactionRequestMessage request)
    {
        lock (this.gate)
        {
            MerchantSeed merchant = this.ResolveMerchant(request.DeviceIdentifier);
            
            this.Seed.ReportTransactions.Add(new ReportTransactionSeed
            {
                Reference = request.TransactionNumber,
                ReceiptReference = $"RCPT-{request.TransactionNumber}",
                TransactionType = "Logon",
                Product = "Logon",
                Operator = "System",
                Status = "Success",
                Amount = 0m,
                TransactionDateTime = request.TransactionDateTime
            });
            
            return new LogonTransactionResponseMessage
            {
                EstateId = this.GetEstateId(merchant.EstateName),
                MerchantId = this.GetMerchantId(merchant.EstateName, merchant.MerchantName),
                ResponseCode = "0000",
                ResponseMessage = "SUCCESS",
                RequiresApplicationUpdate = false,
                TransactionId = SeedIdGenerator.CreateGuid($"logon:{request.TransactionNumber}")
            };
        }
    }

    public async Task<SaleTransactionResponseMessage> HandleSaleAsync(SaleTransactionRequestMessage request)
    {
        lock (this.gate)
        {
            MerchantSeed merchant = this.ResolveMerchant(request.DeviceIdentifier);
            (string kind, string productName, string operatorName, decimal amount, string? customerAccountNumber, string? customerAccountName, string? meterName, string? recipientEmail, string? recipientMobile) = this.InterpretSaleRequest(request);

            bool shouldFail = amount == 150m;
            string responseCode = shouldFail ? "1000" : "0000";
            string responseMessage = shouldFail ? "Failed" : "SUCCESS";
            var response = new SaleTransactionResponseMessage
            {
                EstateId = this.GetEstateId(merchant.EstateName),
                MerchantId = this.GetMerchantId(merchant.EstateName, merchant.MerchantName),
                ResponseCode = responseCode,
                ResponseMessage = responseMessage,
                RequiresApplicationUpdate = false,
                TransactionId = SeedIdGenerator.CreateGuid($"sale:{request.TransactionNumber}")
            };

            if (shouldFail == false)
            {
                BackendRuntimeReceipt receipt = new(
                    request.TransactionNumber,
                    $"RCPT-{request.TransactionNumber}",
                    kind,
                    productName,
                    operatorName,
                    "Success",
                    amount,
                    request.TransactionDateTime);

                this.runtimeReceipts.Add(receipt);
                this.Seed.ReportTransactions.Add(new ReportTransactionSeed
                {
                    Reference = request.TransactionNumber,
                    ReceiptReference = $"RCPT-{request.TransactionNumber}",
                    TransactionType = kind,
                    Product = productName,
                    Operator = operatorName,
                    Status = "Success",
                    Amount = amount,
                    TransactionDateTime = request.TransactionDateTime
                });
            }

            if (kind == "BillPaymentGetAccount")
            {
                BillSeed bill = this.Seed.Bills.FirstOrDefault(b => string.Equals(b.AccountNumber, customerAccountNumber, StringComparison.OrdinalIgnoreCase))
                                ?? new BillSeed { AccountNumber = customerAccountNumber ?? string.Empty, AccountName = customerAccountName ?? "Mr Test Customer", Amount = amount, DueDate = DateTime.UtcNow.Date.AddDays(3) };
                response.AdditionalResponseMetadata = new Dictionary<string, string>
                {
                    ["customerAccountName"] = bill.AccountName,
                    ["customerAccountNumber"] = bill.AccountNumber,
                    ["customerBillBalance"] = bill.Amount.ToString("0.00"),
                    ["customerBillDueDate"] = bill.DueDate.ToString("dd-MM-yyyy")
                };
            }
            else if (kind == "BillPaymentGetMeter")
            {
                MeterSeed meter = this.Seed.Meters.FirstOrDefault(m => string.Equals(m.MeterNumber, meterName ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                                  ?? new MeterSeed { MeterNumber = meterName ?? string.Empty, CustomerName = "Mr Test Customer" };
                response.AdditionalResponseMetadata = new Dictionary<string, string>
                {
                    ["pataPawaPrePaidCustomerName"] = meter.CustomerName
                };
            }

            return response;
        }
    }

    public async Task<ReconciliationResponseMessage> HandleReconciliationAsync(ReconciliationRequestMessage request)
    {
        lock (this.gate)
        {
            MerchantSeed merchant = this.ResolveMerchant(request.DeviceIdentifier);
            return new ReconciliationResponseMessage
            {
                EstateId = this.GetEstateId(merchant.EstateName),
                MerchantId = this.GetMerchantId(merchant.EstateName, merchant.MerchantName),
                ResponseCode = "0000",
                ResponseMessage = "SUCCESS",
                RequiresApplicationUpdate = false,
                TransactionId = SeedIdGenerator.CreateGuid($"recon:{request.TransactionNumber}")
            };
        }
    }

    public AuthTokenResponse BuildToken(string clientId, string clientSecret, string username, string password)
    {
        lock (this.gate)
        {
            ClientSeed? client = this.Seed.Clients.FirstOrDefault(c => string.Equals(c.ClientId, clientId, StringComparison.OrdinalIgnoreCase));
            UserSeed? user = this.Seed.Users.FirstOrDefault(u => string.Equals(u.UserName, username, StringComparison.OrdinalIgnoreCase));

            if (client == null || string.Equals(client.Secret, clientSecret, StringComparison.Ordinal) == false || user == null || string.Equals(user.Password, password, StringComparison.Ordinal) == false)
            {
                throw new InvalidOperationException("Invalid credentials.");
            }

            return new AuthTokenResponse
            {
                AccessToken = $"access-{client.ClientId}-{user.UserName}",
                RefreshToken = $"refresh-{client.ClientId}-{user.UserName}",
                ExpiresIn = 60,
                Issued = DateTimeOffset.UtcNow,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            };
        }
    }

    public AuthTokenResponse BuildRefreshToken(string clientId, string clientSecret, string refreshToken)
    {
        lock (this.gate)
        {
            ClientSeed? client = this.Seed.Clients.FirstOrDefault(c => string.Equals(c.ClientId, clientId, StringComparison.OrdinalIgnoreCase));
            if (client == null || string.Equals(client.Secret, clientSecret, StringComparison.Ordinal) == false || refreshToken.StartsWith("refresh-", StringComparison.OrdinalIgnoreCase) == false)
            {
                throw new InvalidOperationException("Invalid refresh token.");
            }

            return new AuthTokenResponse
            {
                AccessToken = $"access-{client.ClientId}-refreshed",
                RefreshToken = refreshToken,
                ExpiresIn = 60,
                Issued = DateTimeOffset.UtcNow,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            };
        }
    }

    public AuthTokenResponse BuildClientCredentialsToken(string clientId, string clientSecret)
    {
        lock (this.gate)
        {
            ClientSeed? client = this.Seed.Clients.FirstOrDefault(c => string.Equals(c.ClientId, clientId, StringComparison.OrdinalIgnoreCase));
            if (client == null || string.Equals(client.Secret, clientSecret, StringComparison.Ordinal) == false)
            {
                throw new InvalidOperationException("Invalid client credentials.");
            }

            return new AuthTokenResponse
            {
                AccessToken = $"access-{client.ClientId}-service",
                RefreshToken = $"refresh-{client.ClientId}-service",
                ExpiresIn = 60,
                Issued = DateTimeOffset.UtcNow,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            };
        }
    }

    public object BuildDailyPerformanceSummary(MerchantDailyPerformanceSummaryRequest request)
    {
        lock (this.gate)
        {
            var transactions = this.runtimeReceipts
                .Where(r => r.TransactionDateTime.Date >= request.StartDate.Date && r.TransactionDateTime.Date <= request.EndDate.Date)
                .ToList();

            return new
            {
                metrics = new object[]
                {
                    new { title = "Transactions", value = transactions.Count, description = "Transactions in range", category = 0 },
                    new { title = "Value", value = transactions.Sum(t => t.Amount), description = "Total value in range", category = 1 }
                },
                drillDownTransactions = transactions.Select(t => new
                {
                    reference = t.Reference,
                    product = t.Product,
                    status = t.Status,
                    amount = t.Amount,
                    transactionDateTime = t.TransactionDateTime
                }).ToList()
            };
        }
    }

    public object BuildTransactionMixSummary(TransactionMixSummaryRequest request)
    {
        lock (this.gate)
        {
            int topN = request.TopN > 0 ? request.TopN : 5;
            var items = this.runtimeReceipts
                .GroupBy(r => r.Product)
                .OrderByDescending(g => g.Count())
                .Take(topN)
                .Select(g => new
                {
                    key = g.Key,
                    label = g.Key,
                    count = g.Count(),
                    value = g.Sum(t => t.Amount)
                })
                .ToList();

            var transactions = this.runtimeReceipts
                .Select(t => new
                {
                    reference = t.Reference,
                    transactionType = t.TransactionType,
                    product = t.Product,
                    operatorName = t.OperatorName,
                    status = t.Status,
                    amount = t.Amount,
                    transactionDateTime = t.TransactionDateTime
                })
                .ToList();

            return new
            {
                fromDate = this.runtimeReceipts.Select(r => r.TransactionDateTime.Date).DefaultIfEmpty(DateTime.UtcNow.Date).Min(),
                toDate = this.runtimeReceipts.Select(r => r.TransactionDateTime.Date).DefaultIfEmpty(DateTime.UtcNow.Date).Max(),
                breakdown = 0,
                measure = 0,
                totalCount = (decimal)this.runtimeReceipts.Count,
                totalValue = this.runtimeReceipts.Sum(r => r.Amount),
                items,
                drillDownTransactions = transactions
            };
        }
    }

    public object BuildRecentActivityReceiptSearch(RecentActivityReceiptSearchRequest request)
    {
        lock (this.gate)
        {
            string? searchText = string.IsNullOrWhiteSpace(request.SearchText) ? null : request.SearchText;
            int pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
            int pageSize = request.PageSize > 0 ? request.PageSize : 5;
            DateTime reportDate = request.ReportDate.Date;

            var filtered = this.runtimeReceipts
                .Where(r => r.TransactionDateTime.Date == reportDate)
                .Where(r => string.IsNullOrWhiteSpace(searchText) || r.Reference.Contains(searchText, StringComparison.OrdinalIgnoreCase) || r.ReceiptReference.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(r => r.TransactionDateTime)
                .ToList();

            var paged = filtered.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new
            {
                reportDate,
                pageNumber,
                pageSize,
                totalCount = filtered.Count,
                items = paged.Select(item => new
                {
                    reference = item.Reference,
                    transactionType = item.TransactionType,
                    product = item.Product,
                    operatorName = item.OperatorName,
                    status = item.Status,
                    amount = item.Amount,
                    transactionDateTime = item.TransactionDateTime,
                    receiptReference = item.ReceiptReference
                }).ToList()
            };
        }
    }

    public object ResendReceipt(ResendReceiptRequest request)
    {
        lock (this.gate)
        {
            var receipt = this.runtimeReceipts.FirstOrDefault(r => string.Equals(r.Reference, request.Reference, StringComparison.OrdinalIgnoreCase));
            return new
            {
                success = receipt != null,
                message = receipt != null ? "SUCCESS" : "Receipt not found",
                reference = receipt?.Reference ?? request.Reference ?? string.Empty,
                receiptReference = receipt?.ReceiptReference ?? string.Empty,
                transactionReference = receipt?.Reference ?? string.Empty
            };
        }
    }

    public void RecordLogs(string deviceIdentifier, List<BackendLogMessage> messages)
    {
        lock (this.gate)
        {
            foreach (BackendLogMessage message in messages)
            {
                this.runtimeLogs.Add(new BackendRuntimeLog(deviceIdentifier, message.Message, message.EntryDateTime, message.LogLevel.ToString()));
            }
        }
    }

    private MerchantSeed ResolveMerchant(string deviceIdentifier)
    {
        if (this.deviceToMerchant.TryGetValue(deviceIdentifier, out string? merchantName))
        {
            MerchantSeed? merchant = this.Seed.Merchants.FirstOrDefault(m => string.Equals(m.MerchantName, merchantName, StringComparison.OrdinalIgnoreCase));
            if (merchant != null)
            {
                return merchant;
            }
        }

        return this.Seed.Merchants.FirstOrDefault()
               ?? new MerchantSeed
               {
                   EstateName = "Default Estate",
                   MerchantName = "Dummy Merchant",
                   AddressLine1 = "test address line 1",
                   AddressLine2 = "test address line 2",
                   AddressLine3 = "test address line 3",
                   AddressLine4 = "test address line 4",
                   Town = "Town",
                   Region = "Region",
                   PostalCode = "TE57 1NG",
                   Country = "United Kingdom",
                   ContactName = "Test Contact",
                   ContactEmailAddress = "test@example.com",
                   ContactPhoneNumber = "123456789"
               };
    }

    private Guid GetEstateId(string estateName) => SeedIdGenerator.CreateGuid($"estate:{estateName}");

    private int GetEstateReportingId(string estateName) => SeedIdGenerator.CreateInt($"estate-report:{estateName}");

    private Guid GetMerchantId(string estateName, string merchantName) => SeedIdGenerator.CreateGuid($"merchant:{estateName}:{merchantName}");

    private int GetMerchantReportingId(string estateName, string merchantName) => SeedIdGenerator.CreateInt($"merchant-report:{estateName}:{merchantName}");

    private Guid GetOperatorId(string estateName, string operatorName) => SeedIdGenerator.CreateGuid($"operator:{estateName}:{operatorName}");

    private Dictionary<Guid, string> GetMerchantDevices(string estateName, string merchantName)
    {
        var mappings = this.Seed.Devices
            .Where(device => string.Equals(device.EstateName, estateName, StringComparison.OrdinalIgnoreCase) &&
                             string.Equals(device.MerchantName, merchantName, StringComparison.OrdinalIgnoreCase))
            .Select((device, index) => new
            {
                Key = SeedIdGenerator.CreateGuid($"device:{device.DeviceIdentifier ?? $"{estateName}:{merchantName}:{index}"}"),
                Value = device.DeviceIdentifier ?? string.Empty
            })
            .ToDictionary(device => device.Key, device => device.Value);
        return mappings;
    }

    private List<MerchantOperatorResponse> BuildMerchantOperators(string estateName, string merchantName)
    {
        return this.Seed.MerchantOperators
            .Where(assignment => string.Equals(assignment.EstateName, estateName, StringComparison.OrdinalIgnoreCase) &&
                                 string.Equals(assignment.MerchantName, merchantName, StringComparison.OrdinalIgnoreCase))
            .Select(assignment => new MerchantOperatorResponse
            {
                Name = assignment.OperatorName,
                OperatorId = GetOperatorId(assignment.EstateName, assignment.OperatorName),
                MerchantNumber = assignment.MerchantNumber,
                TerminalNumber = assignment.TerminalNumber,
                IsDeleted = false
            })
            .ToList();
    }

    private List<MerchantContractResponse> BuildMerchantContracts(string estateName, string merchantName)
    {
        return this.Seed.MerchantContracts
            .Where(assignment => string.Equals(assignment.EstateName, estateName, StringComparison.OrdinalIgnoreCase) &&
                                 string.Equals(assignment.MerchantName, merchantName, StringComparison.OrdinalIgnoreCase))
            .Select(assignment => new MerchantContractResponse
            {
                ContractId = SeedIdGenerator.CreateGuid($"contract:{assignment.EstateName}:{assignment.ContractDescription}"),
                IsDeleted = false,
                ContractProducts = this.Seed.Products
                    .Where(p => string.Equals(p.EstateName, assignment.EstateName, StringComparison.OrdinalIgnoreCase) &&
                                string.Equals(p.ContractDescription, assignment.ContractDescription, StringComparison.OrdinalIgnoreCase))
                    .Select(p => SeedIdGenerator.CreateGuid($"product:{p.EstateName}:{p.OperatorName}:{p.ContractDescription}:{p.ProductName}"))
                    .ToList()
            })
            .ToList();
    }

    private static SettlementSchedule ParseSettlementSchedule(string? schedule) =>
        schedule?.Trim().ToLowerInvariant() switch
        {
            "immediate" => SettlementSchedule.Immediate,
            "weekly" => SettlementSchedule.Weekly,
            "monthly" => SettlementSchedule.Monthly,
            _ => SettlementSchedule.Monthly
        };

    private static ProductType ParseProductType(string productType) =>
        productType.Trim().ToLowerInvariant() switch
        {
            "mobiletopup" => ProductType.MobileTopup,
            "voucher" => ProductType.Voucher,
            "billpayment" => ProductType.BillPayment,
            _ => ProductType.NotSet
        };

    private static CalculationType ParseCalculationType(string? calculationType) =>
        calculationType?.Trim().ToLowerInvariant() switch
        {
            "percentage" => CalculationType.Percentage,
            "fixed" => CalculationType.Fixed,
            _ => CalculationType.Fixed
        };

    private (string kind, string productName, string operatorName, decimal amount, string? customerAccountNumber, string? customerAccountName, string? meterName, string? recipientEmail, string? recipientMobile) InterpretSaleRequest(SaleTransactionRequestMessage request)
    {
        decimal amount = request.AdditionalRequestMetadata?.TryGetValue("Amount", out string? amountText) == true && decimal.TryParse(amountText, out decimal parsedAmount)
            ? parsedAmount
            : 0m;

        string operatorName = this.ResolveOperatorName(request.OperatorId);
        string productName = this.ResolveProductName(request.ProductId);

        if (request.AdditionalRequestMetadata?.ContainsKey("PataPawaPostPaidMessageType") == true)
        {
            if (TryGetMetadataValue(request.AdditionalRequestMetadata, "PataPawaPostPaidMessageType", out string? type) && string.Equals(type, "VerifyAccount", StringComparison.OrdinalIgnoreCase))
            {
                return ("BillPaymentGetAccount", productName, operatorName, amount, TryGetMetadataValue(request.AdditionalRequestMetadata, "CustomerAccountNumber", out string? accountNumber) ? accountNumber : null, request.CustomerEmailAddress, null, null, null);
            }

            return ("BillPaymentMakePayment", productName, operatorName, amount, TryGetMetadataValue(request.AdditionalRequestMetadata, "CustomerAccountNumber", out string? accountNumber2) ? accountNumber2 : null, request.CustomerEmailAddress, null, null, null);
        }

        if (request.AdditionalRequestMetadata?.ContainsKey("PataPawaPrePayMessageType") == true)
        {
            if (TryGetMetadataValue(request.AdditionalRequestMetadata, "PataPawaPrePayMessageType", out string? type) && string.Equals(type, "meter", StringComparison.OrdinalIgnoreCase))
            {
                return ("BillPaymentGetMeter", productName, operatorName, amount, null, null, TryGetMetadataValue(request.AdditionalRequestMetadata, "MeterNumber", out string? meterNumber) ? meterNumber : null, null, null);
            }

            return ("BillPaymentMakePayment", productName, operatorName, amount, null, null, TryGetMetadataValue(request.AdditionalRequestMetadata, "MeterNumber", out string? meterNumber2) ? meterNumber2 : null, null, null);
        }

        if (request.AdditionalRequestMetadata?.ContainsKey("RecipientMobile") == true)
        {
            return ("Voucher", productName, operatorName, amount, null, null, null, TryGetMetadataValue(request.AdditionalRequestMetadata, "RecipientEmail", out string? email) ? email : null, TryGetMetadataValue(request.AdditionalRequestMetadata, "RecipientMobile", out string? mobile) ? mobile : null);
        }

        if (request.AdditionalRequestMetadata?.ContainsKey("CustomerAccountNumber") == true)
        {
            return ("MobileTopup", productName, operatorName, amount, TryGetMetadataValue(request.AdditionalRequestMetadata, "CustomerAccountNumber", out string? accountNumber3) ? accountNumber3 : null, null, null, null, null);
        }

        return ("MobileTopup", productName, operatorName, amount, null, null, null, null, null);
    }

    private static bool TryGetMetadataValue(Dictionary<string, string>? metadata, string key, out string? value)
    {
        value = null;
        if (metadata == null)
        {
            return false;
        }

        foreach (KeyValuePair<string, string> entry in metadata)
        {
            if (string.Equals(entry.Key, key, StringComparison.OrdinalIgnoreCase))
            {
                value = entry.Value;
                return true;
            }
        }

        return false;
    }

    private string ResolveOperatorName(Guid operatorId)
    {
        return this.Seed.Operators
            .FirstOrDefault(op => GetOperatorId(op.EstateName, op.OperatorName) == operatorId)
            ?.OperatorName ?? "Unknown";
    }

    private string ResolveProductName(Guid productId)
    {
        return this.Seed.Products
            .FirstOrDefault(product => SeedIdGenerator.CreateGuid($"product:{product.EstateName}:{product.OperatorName}:{product.ContractDescription}:{product.ProductName}") == productId)
            ?.DisplayText ?? "Unknown";
    }

    private sealed record BackendRuntimeReceipt(string Reference, string ReceiptReference, string TransactionType, string Product, string OperatorName, string Status, decimal Amount, DateTime TransactionDateTime);
    private sealed record BackendRuntimeLog(string DeviceIdentifier, string Message, DateTime EntryDateTime, string LogLevel);
}

public sealed record AuthTokenResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public long ExpiresIn { get; init; }
    public DateTimeOffset Issued { get; init; }
    public DateTimeOffset Expires { get; init; }
}

public sealed record ApplicationUpdateCheckResponse
{
    public bool UpdateRequired { get; init; }
    public string? DownloadUri { get; init; }
    public string? LatestVersion { get; init; }
    public string? Message { get; init; }
}

public sealed record MerchantDailyPerformanceSummaryRequest
{
    public int MerchantReportingId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}

public sealed record TransactionMixSummaryRequest
{
    public int MerchantReportingId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int Breakdown { get; init; }
    public int Measure { get; init; }
    public int TopN { get; init; }
}

public sealed record RecentActivityReceiptSearchRequest
{
    public int MerchantReportingId { get; init; }
    public DateTime ReportDate { get; init; }
    public string? SearchText { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? ApplicationVersion { get; init; }
}

public sealed record ResendReceiptRequest
{
    public string Reference { get; init; } = string.Empty;
    public string RecipientEmailAddress { get; init; } = string.Empty;
}

public sealed record LogEnvelope
{
    public List<BackendLogMessage> Messages { get; init; } = [];
}

public sealed record BackendLogMessage
{
    public string LogLevel { get; init; } = string.Empty;
    public string LogLevelString { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public DateTime EntryDateTime { get; init; }
    public int Id { get; init; }
    public bool IsTrainingMode { get; init; }
}
