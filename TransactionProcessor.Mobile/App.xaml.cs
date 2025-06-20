using Microsoft.Maui.Handlers;
using TransactionProcessor.Mobile.Pages;
using TransactionProcessor.Mobile.Pages.AppHome;
#if ANDROID
using AndroidX.Core.View;
#endif
namespace TransactionProcessor.Mobile
{
    public partial class App : Application
    {
        public App()
        {
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
            info.ViewIdResourceName = $"{host.Context.PackageName}:id/{AutomationId}";
        }
    }
}
#endif
}