using Microsoft.Maui.Platform;
using TransactionMobile.Maui.Pages.Reports;
using TransactionMobile.Maui.Pages.Transactions.MobileTopup;
using TransactionMobile.Maui.Pages.Transactions.Voucher;

namespace TransactionMobile.Maui;

using Pages.AppHome;
using Pages.Transactions.Admin;
using TransactionMobile.Maui.BusinessLogic.Services;
using TransactionMobile.Maui.Controls;

public partial class App : Application
{
	public App()
    {
        InitializeComponent();

        Microsoft.Maui.Handlers.LabelHandler.ElementMapper.AppendToMapping("TrainingMode", (handler, view) =>
        {
            if (view is TitleLabel)
            {
                var memoryCache = MauiProgram.Container.Services.GetService<IMemoryCacheService>();

                memoryCache.TryGetValue("UseTrainingMode", out Boolean useTrainingMode);

                if (useTrainingMode)
                {
                    ((TitleLabel)view).Text = $"{((TitleLabel)view).Text} - Training Mode";
                }
            }
        });

        Microsoft.Maui.Handlers.EntryHandler.ElementMapper.AppendToMapping("AutomationIdEntry", (handler, view) =>
        {
            if (view is Entry)
            {
                var e = (Entry) view;
                if (String.IsNullOrEmpty(e.AutomationId) == false)
                {
                    SemanticProperties.SetHint(e,e.AutomationId);
                }
            }
        });

        Microsoft.Maui.Handlers.LabelHandler.ElementMapper.AppendToMapping("AutomationIdLabel", (handler, view) =>
        {
            if (view is Label)
            {
                var e = (Label) view;
                if (String.IsNullOrEmpty(e.AutomationId) == false)
                {
                    SemanticProperties.SetDescription(e, e.AutomationId);
                }
            }
        });

        Microsoft.Maui.Handlers.ButtonHandler.ElementMapper.AppendToMapping("AutomationIdButton", (handler, view) =>
        {
            if (view is Button)
            {
                var e = (Button) view;
                if (String.IsNullOrEmpty(e.AutomationId) == false)
                {
                    SemanticProperties.SetDescription(e, e.AutomationId);
                }
            }
        });
        
        MainPage = new AppShell();

        Routing.RegisterRoute(nameof(MobileTopupSelectOperatorPage), typeof(MobileTopupSelectOperatorPage));
        Routing.RegisterRoute(nameof(MobileTopupSelectProductPage), typeof(MobileTopupSelectProductPage));
        Routing.RegisterRoute(nameof(MobileTopupPerformTopupPage), typeof(MobileTopupPerformTopupPage));
        Routing.RegisterRoute(nameof(MobileTopupSuccessPage), typeof(MobileTopupSuccessPage));
        Routing.RegisterRoute(nameof(MobileTopupFailedPage), typeof(MobileTopupFailedPage));

        Routing.RegisterRoute(nameof(VoucherSelectOperatorPage), typeof(VoucherSelectOperatorPage));
        Routing.RegisterRoute(nameof(VoucherSelectProductPage), typeof(VoucherSelectProductPage));
        Routing.RegisterRoute(nameof(VoucherPerformIssuePage), typeof(VoucherPerformIssuePage));
        Routing.RegisterRoute(nameof(VoucherIssueSuccessPage), typeof(VoucherIssueSuccessPage));
        Routing.RegisterRoute(nameof(VoucherIssueFailedPage), typeof(VoucherIssueFailedPage));

        Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
    }
}

