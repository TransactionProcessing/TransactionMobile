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

        public App()
        {
            InitializeComponent();

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

            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("home", typeof(HomePage));

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
            Routing.RegisterRoute(nameof(BillPaymentGetMeterPage), typeof(BillPaymentGetMeterPage));
            Routing.RegisterRoute(nameof(BillPaymentPayBillPage), typeof(BillPaymentPayBillPage));
            Routing.RegisterRoute(nameof(BillPaymentSuccessPage), typeof(BillPaymentSuccessPage));
            Routing.RegisterRoute(nameof(BillPaymentFailedPage), typeof(BillPaymentFailedPage));

            Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            Routing.RegisterRoute(nameof(ViewLogsPage), typeof(ViewLogsPage));

            Routing.RegisterRoute(nameof(ReportsBalanceAnalysisPage), typeof(ReportsBalanceAnalysisPage));
            Routing.RegisterRoute(nameof(ReportsSalesAnalysisPage), typeof(ReportsSalesAnalysisPage));

            Routing.RegisterRoute(nameof(MyAccountAddressesPage), typeof(MyAccountAddressesPage));
            Routing.RegisterRoute(nameof(MyAccountContactPage), typeof(MyAccountContactPage));
            Routing.RegisterRoute(nameof(MyAccountDetailsPage), typeof(MyAccountDetailsPage));
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