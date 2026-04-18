using Microsoft.Maui.Handlers;
using TransactionProcessor.Mobile.Pages;
using TransactionProcessor.Mobile.Pages.AppHome;
using TransactionProcessor.Mobile.Pages.MyAccount;
using TransactionProcessor.Mobile.Pages.Reports;
using TransactionProcessor.Mobile.Pages.Support;
using TransactionProcessor.Mobile.Pages.Transactions.Admin;
using TransactionProcessor.Mobile.Pages.Transactions.BillPayment;
using TransactionProcessor.Mobile.Pages.Transactions.MobileTopup;
using TransactionProcessor.Mobile.Pages.Transactions.Voucher;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using Microsoft.Maui.ApplicationModel;

#if ANDROID
using AndroidX.Core.View;
#endif
namespace TransactionProcessor.Mobile
{
    public partial class App : Application
    {
#if ANDROID
        void ApplyAutomationIdMapping<TVirtualView, THandlerInterface>(
            IPropertyMapper<TVirtualView, THandlerInterface> mapper)
            where TVirtualView : IView
            where THandlerInterface : IViewHandler
        {
            mapper.ModifyMapping("AutomationId", (handler, view, previous) =>
            {
                if (handler.PlatformView is Android.Views.View androidView)
                {
                    if (String.IsNullOrWhiteSpace(view.AutomationId))
                        return;

                    //androidView.ContentDescription = view.AutomationId;

                    var existingDelegate = ViewCompat.GetAccessibilityDelegate(androidView);
                    if (existingDelegate is not AutomationIdDelegate delegateCompat)
                    {
                        delegateCompat = new AutomationIdDelegate();
                        ViewCompat.SetAccessibilityDelegate(androidView, delegateCompat);
                    }

                    delegateCompat.AutomationId = view.AutomationId;
#pragma warning disable CS0618
                    ViewCompat.SetImportantForAccessibility(androidView,1);
#pragma warning restore CS0618
                }
            });
        }
#endif

        public App(IApplicationThemeService applicationThemeService)
        {
            InitializeComponent();
            MainThread.BeginInvokeOnMainThread(async () => await applicationThemeService.ApplyConfiguredTheme());

#if ANDROID
            ApplyAutomationIdMapping(ViewHandler.ViewMapper);
            ApplyAutomationIdMapping(EntryHandler.Mapper);
            ApplyAutomationIdMapping(SwitchHandler.Mapper);
            ApplyAutomationIdMapping(LabelHandler.Mapper);
            ApplyAutomationIdMapping(ButtonHandler.Mapper);

            //ViewHandler.ViewMapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
            //{
            //    if (handler.PlatformView is Android.Views.View androidView)
            //    {
            //        if (String.IsNullOrWhiteSpace(view.AutomationId))
            //            return;

            //        if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
            //            AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


            //        if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
            //            td.AutomationId = view.AutomationId;

            //        androidView.ContentDescription = view.AutomationId;
            //    }
            //});

            //EntryHandler.Mapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
            //                                                  {
            //                                                      if (handler.PlatformView is Android.Views.View androidView)
            //                                                      {
            //                                                          if (String.IsNullOrWhiteSpace(view.AutomationId))
            //                                                              return;

            //                                                          if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
            //                                                              AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


            //                                                          if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
            //                                                              td.AutomationId = view.AutomationId;

            //                                                          androidView.ContentDescription = view.AutomationId;
            //                                                      }
            //                                                  });

            //SwitchHandler.Mapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
            //                                                  {
            //                                                      if (handler.PlatformView is Android.Views.View androidView)
            //                                                      {
            //                                                          if (String.IsNullOrWhiteSpace(view.AutomationId))
            //                                                              return;

            //                                                          if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
            //                                                              AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


            //                                                          if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
            //                                                              td.AutomationId = view.AutomationId;

            //                                                          androidView.ContentDescription = view.AutomationId;
            //                                                      }
            //                                                  });

            //LabelHandler.Mapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
            //{
            //    if (handler.PlatformView is Android.Views.View androidView)
            //    {
            //        if (String.IsNullOrWhiteSpace(view.AutomationId))
            //            return;

            //        if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
            //            AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


            //        if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
            //            td.AutomationId = view.AutomationId;

            //        androidView.ContentDescription = view.AutomationId;
            //    }
            //});

            //ButtonHandler.Mapper.ModifyMapping("AutomationId", (handler, view, previousAction) =>
            //{
            //    if (handler.PlatformView is Android.Views.View androidView)
            //    {
            //        if (String.IsNullOrWhiteSpace(view.AutomationId))
            //            return;

            //        if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is not AutomationIdDelegate)
            //            AndroidX.Core.View.ViewCompat.SetAccessibilityDelegate(androidView, new AutomationIdDelegate());


            //        if (AndroidX.Core.View.ViewCompat.GetAccessibilityDelegate(androidView) is AutomationIdDelegate td)
            //            td.AutomationId = view.AutomationId;

            //        androidView.ContentDescription = view.AutomationId;
            //    }
            //});
#endif
            MainPage = new AppShell();

            //RegisterRouteOnce("login", typeof(LoginPage));
            //RegisterRouteOnce("home", typeof(HomePage));

            RegisterRouteOnce(nameof(MobileTopupSelectOperatorPage), typeof(MobileTopupSelectOperatorPage));
            RegisterRouteOnce(nameof(MobileTopupSelectProductPage), typeof(MobileTopupSelectProductPage));
            RegisterRouteOnce(nameof(MobileTopupPerformTopupPage), typeof(MobileTopupPerformTopupPage));
            RegisterRouteOnce(nameof(MobileTopupSuccessPage), typeof(MobileTopupSuccessPage));
            RegisterRouteOnce(nameof(MobileTopupFailedPage), typeof(MobileTopupFailedPage));

            RegisterRouteOnce(nameof(VoucherSelectOperatorPage), typeof(VoucherSelectOperatorPage));
            RegisterRouteOnce(nameof(VoucherSelectProductPage), typeof(VoucherSelectProductPage));
            RegisterRouteOnce(nameof(VoucherPerformIssuePage), typeof(VoucherPerformIssuePage));
            RegisterRouteOnce(nameof(VoucherIssueSuccessPage), typeof(VoucherIssueSuccessPage));
            RegisterRouteOnce(nameof(VoucherIssueFailedPage), typeof(VoucherIssueFailedPage));

            RegisterRouteOnce(nameof(BillPaymentSelectOperatorPage), typeof(BillPaymentSelectOperatorPage));
            RegisterRouteOnce(nameof(BillPaymentSelectProductPage), typeof(BillPaymentSelectProductPage));
            RegisterRouteOnce(nameof(BillPaymentGetAccountPage), typeof(BillPaymentGetAccountPage));
            RegisterRouteOnce(nameof(BillPaymentGetMeterPage), typeof(BillPaymentGetMeterPage));
            RegisterRouteOnce(nameof(BillPaymentPayBillPage), typeof(BillPaymentPayBillPage));
            RegisterRouteOnce(nameof(BillPaymentSuccessPage), typeof(BillPaymentSuccessPage));
            RegisterRouteOnce(nameof(BillPaymentFailedPage), typeof(BillPaymentFailedPage));
            RegisterRouteOnce(nameof(AdminPage), typeof(AdminPage));
            //RegisterRouteOnce(nameof(LoginPage), typeof(LoginPage));

            RegisterRouteOnce(nameof(ViewLogsPage), typeof(ViewLogsPage));

            RegisterRouteOnce(nameof(ReportsBalanceAnalysisPage), typeof(ReportsBalanceAnalysisPage));
            RegisterRouteOnce(nameof(ReportsSalesAnalysisPage), typeof(ReportsSalesAnalysisPage));

            RegisterRouteOnce(nameof(MyAccountAddressesPage), typeof(MyAccountAddressesPage));
            RegisterRouteOnce(nameof(MyAccountContactPage), typeof(MyAccountContactPage));
            RegisterRouteOnce(nameof(MyAccountDetailsPage), typeof(MyAccountDetailsPage));
        }

        static readonly HashSet<string> s_registeredRoutes = new();

        static void RegisterRouteOnce(string route, Type pageType)
        {
            if (s_registeredRoutes.Add(route))
                Routing.RegisterRoute(route, pageType);
        }
    }

#if ANDROID
public class AutomationIdDelegate : AccessibilityDelegateCompat
    {
    public string AutomationId { get; internal set; }

    public override void OnInitializeAccessibilityNodeInfo(Android.Views.View host, AndroidX.Core.View.Accessibility.AccessibilityNodeInfoCompat info)
    {
        base.OnInitializeAccessibilityNodeInfo(host, info);

        if (!String.IsNullOrWhiteSpace(AutomationId))
        {
            info.ViewIdResourceName = $"{host.Context.PackageName}:id/{AutomationId.Replace(" ", "")}";
            // Ensure content-desc is set as well
            info.ContentDescription = AutomationId.Replace(" ", "");
            // Force the native view's ContentDescription property as well
            host.ContentDescription = AutomationId.Replace(" ", "");
            }
    }
}


#endif
}
