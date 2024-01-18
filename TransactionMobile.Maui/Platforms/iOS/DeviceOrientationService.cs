using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.Platforms.Services
{
    using Foundation;
    using Microsoft.Maui.Devices;
    using UIKit;

    public static partial class DeviceOrientationService
    {
        private static readonly IReadOnlyDictionary<DisplayOrientation, UIInterfaceOrientation> _iosDisplayOrientationMap =
            new Dictionary<DisplayOrientation, UIInterfaceOrientation>
            {
                [DisplayOrientation.Landscape] = UIInterfaceOrientation.LandscapeLeft,
                [DisplayOrientation.Portrait] = UIInterfaceOrientation.Portrait,
            };

        public static partial void SetDeviceOrientation(DisplayOrientation displayOrientation)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(16, 0)){

                var scene = (UIApplication.SharedApplication.ConnectedScenes.ToArray()[0] as UIWindowScene);
                if (scene != null){
                    var uiAppplication = UIApplication.SharedApplication;
                    var test = UIApplication.SharedApplication.KeyWindow?.RootViewController;
                    if (test != null){
                        test.SetNeedsUpdateOfSupportedInterfaceOrientations();
                        scene.RequestGeometryUpdate(
                                                    new UIWindowSceneGeometryPreferencesIOS(UIInterfaceOrientationMask.Portrait),
                                                    error => { });
                    }
                }
            }
            else{
                UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
            }
        }
    }
}
