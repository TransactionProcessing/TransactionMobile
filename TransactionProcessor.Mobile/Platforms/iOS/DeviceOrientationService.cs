using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionProcessor.Mobile.BusinessLogic.UIServices
{
    using Foundation;
    using Microsoft.Maui.Devices;
    using UIKit;

    public static partial class DeviceOrientationService
    {
        private static readonly IReadOnlyDictionary<DisplayOrientation, UIInterfaceOrientation> UIInterfaceOrientationMapping =
            new Dictionary<DisplayOrientation, UIInterfaceOrientation>
            {
                [DisplayOrientation.Landscape] = UIInterfaceOrientation.LandscapeLeft,
                [DisplayOrientation.Portrait] = UIInterfaceOrientation.Portrait,
            };

        private static readonly IReadOnlyDictionary<DisplayOrientation, UIInterfaceOrientationMask> UIInterfaceOrientationMaskMapping =
            new Dictionary<DisplayOrientation, UIInterfaceOrientationMask>
            {
                [DisplayOrientation.Landscape] = UIInterfaceOrientationMask.Landscape,
                [DisplayOrientation.Portrait] = UIInterfaceOrientationMask.Portrait,
            };

        public static partial void SetDeviceOrientation(DisplayOrientation displayOrientation){
            
            if (UIDevice.CurrentDevice.CheckSystemVersion(16, 0)){

                var scene = (UIApplication.SharedApplication.ConnectedScenes.ToArray()[0] as UIWindowScene);
                if (scene != null)
                {
                    var test = UIApplication.SharedApplication.KeyWindow?.RootViewController;
                    if (test != null)
                    {
                        test.SetNeedsUpdateOfSupportedInterfaceOrientations();
                        UIInterfaceOrientationMask mappingValue = DeviceOrientationService.UIInterfaceOrientationMaskMapping.Single(m => m.Key == displayOrientation).Value;
                        scene.RequestGeometryUpdate(
                                                    new UIWindowSceneGeometryPreferencesIOS(mappingValue),
                                                    error => { });
                    }
                }
            }
            else{
                UIInterfaceOrientation mappingValue = DeviceOrientationService.UIInterfaceOrientationMapping.Single(m => m.Key == displayOrientation).Value;
                UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)mappingValue), new NSString("orientation"));
            }
        }
    }
}
