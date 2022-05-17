using Microsoft.Maui.Platform;
using TransactionMobile.Maui.Pages.Reports;
using TransactionMobile.Maui.Pages.Transactions.MobileTopup;
using TransactionMobile.Maui.Pages.Transactions.Voucher;

namespace TransactionMobile.Maui;

using Microsoft.Maui.Handlers;
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

