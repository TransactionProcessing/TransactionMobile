using TransactionMobile.Maui.Pages.Transactions.MobileTopup;
using TransactionMobile.Maui.Pages.Transactions.Voucher;
using TransactionMobile.Maui.Pages.Transactions.BillPayment;
namespace TransactionMobile.Maui;

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Pages;
using Pages.AppHome;
using Pages.MyAccount;
using Pages.Support;
using Pages.Transactions.Admin;

public partial class App : Application
{
	public App()
    {
        Console.WriteLine("In App Ctor");
        InitializeComponent();

#if ANDROID
        ViewHandler.ViewMapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
        {
            if (handler.PlatformView is Android.Views.View androidView)
            {
                if (String.IsNullOrWhiteSpace(view.AutomationId))
                    return;

                if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
                    AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


                if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
                    td.AutomationId = view.AutomationId;

                androidView.ContentDescription = view.AutomationId;
            }
        });

        EntryHandler.Mapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
                                                          {
                                                              if (handler.PlatformView is Android.Views.View androidView)
                                                              {
                                                                  if (String.IsNullOrWhiteSpace(view.AutomationId))
                                                                      return;

                                                                  if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
                                                                      AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


                                                                  if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
                                                                      td.AutomationId = view.AutomationId;

                                                                  androidView.ContentDescription = view.AutomationId;
                                                              }
                                                          });

        SwitchHandler.Mapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
                                                          {
                                                              if (handler.PlatformView is Android.Views.View androidView)
                                                              {
                                                                  if (String.IsNullOrWhiteSpace(view.AutomationId))
                                                                      return;

                                                                  if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
                                                                      AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


                                                                  if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
                                                                      td.AutomationId = view.AutomationId;

                                                                  androidView.ContentDescription = view.AutomationId;
                                                              }
                                                          });

        LabelHandler.Mapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
        {
            if (handler.PlatformView is Android.Views.View androidView)
            {
                if (String.IsNullOrWhiteSpace(view.AutomationId))
                    return;

                if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
                    AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


                if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
                    td.AutomationId = view.AutomationId;

                androidView.ContentDescription = view.AutomationId;
            }
        });

        ButtonHandler.Mapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
        {
            if (handler.PlatformView is Android.Views.View androidView)
            {
                if (String.IsNullOrWhiteSpace(view.AutomationId))
                    return;

                if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
                    AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


                if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
                    td.AutomationId = view.AutomationId;

                androidView.ContentDescription = view.AutomationId;
            }
        });
#endif
        MainPage = new AppShell();
        
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("home", typeof(HomePage));

        // TODO: Investigate if this could be done automatically (maybe with exclusions for top level pages)
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

        Routing.RegisterRoute(nameof(BillPaymentSelectOperatorPage), typeof(BillPaymentSelectOperatorPage));
        Routing.RegisterRoute(nameof(BillPaymentSelectProductPage), typeof(BillPaymentSelectProductPage));
        Routing.RegisterRoute(nameof(BillPaymentGetAccountPage), typeof(BillPaymentGetAccountPage));
        Routing.RegisterRoute(nameof(BillPaymentPayBillPage), typeof(BillPaymentPayBillPage));
        Routing.RegisterRoute(nameof(BillPaymentSuccessPage), typeof(BillPaymentSuccessPage));
        Routing.RegisterRoute(nameof(BillPaymentFailedPage), typeof(BillPaymentFailedPage));

        Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

        Routing.RegisterRoute(nameof(ViewLogsPage), typeof(ViewLogsPage));

        Routing.RegisterRoute(nameof(MyAccountAddressesPage), typeof(MyAccountAddressesPage));
        Routing.RegisterRoute(nameof(MyAccountContactPage), typeof(MyAccountContactPage));
        Routing.RegisterRoute(nameof(MyAccountDetailsPage), typeof(MyAccountDetailsPage));
    }
}

#if ANDROID
public class AutomationIdDelegate : MauiAccessibilityDelegateCompat
{
    public string AutomationId { get; internal set; }

    public override void OnInitializeAccessibilityNodeInfo(Android.Views.View host, AndroidX.Core.View.Accessibility.AccessibilityNodeInfoCompat info)
    {
        base.OnInitializeAccessibilityNodeInfo(host, info);

        if (!String.IsNullOrWhiteSpace(AutomationId))
        {
            info.ViewIdResourceName = $"{host.Context.PackageName}:id/{AutomationId}";
        }
    }
}
#endif

