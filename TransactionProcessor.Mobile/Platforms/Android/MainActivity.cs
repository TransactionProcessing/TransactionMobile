using Android.App;
using Android.Content.PM;
using Android.OS;
using System.Reflection;
using AndroidX.Activity;
using Microsoft.Maui.Controls;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels;
using Android.Window;

namespace TransactionProcessor.Mobile
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private OnBackPressedCallback backPressedCallback;
        private IOnBackInvokedCallback? backInvokedCallback;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.backPressedCallback = new BackPressedHandler(this);
            this.OnBackPressedDispatcher.AddCallback(this, this.backPressedCallback);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                this.backInvokedCallback = new BackInvokedHandler(this);
                this.OnBackInvokedDispatcher.RegisterOnBackInvokedCallback(0, this.backInvokedCallback);
            }
        }

        private void HandleBackPressed()
        {
            Shell currentPage = ResolveCurrentShellPage();
            
            if (currentPage.CurrentPage?.BindingContext is ExtendedBaseViewModel viewModel)
            {
                viewModel.BackButtonCommand.Execute(null);
                return;
            }
            

            Finish();
        }

        private static Shell ResolveCurrentShellPage() => Shell.Current is Shell shell ? ResolveShellPage(shell) : null;

        private static Shell ResolveShellPage(Shell shell)
        {
            object current = shell;

            for (Int32 i = 0; i < 4 && current != null; i++)
            {
                if (current is Shell shellPage)
                {
                    return shellPage;
                }

                object next = GetPropertyValue(current, "CurrentPage")
                              ?? GetPropertyValue(current, "CurrentItem")
                              ?? GetPropertyValue(current, "Content");

                if (next == null || ReferenceEquals(next, current))
                {
                    break;
                }

                current = next;
            }

            return null;
        }

        private static object GetPropertyValue(object instance, String propertyName)
        {
            PropertyInfo property = instance.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return property?.GetValue(instance);
        }

        private sealed class BackPressedHandler : OnBackPressedCallback
        {
            private readonly MainActivity activity;

            public BackPressedHandler(MainActivity activity)
                : base(true)
            {
                this.activity = activity;
            }

            public override void HandleOnBackPressed()
            {
                this.activity.HandleBackPressed();
            }
        }

        private sealed class BackInvokedHandler : Java.Lang.Object, IOnBackInvokedCallback
        {
            private readonly MainActivity activity;

            public BackInvokedHandler(MainActivity activity)
            {
                this.activity = activity;
            }

            public void OnBackInvoked()
            {
                this.activity.HandleBackPressed();
            }
        }
    }
}
