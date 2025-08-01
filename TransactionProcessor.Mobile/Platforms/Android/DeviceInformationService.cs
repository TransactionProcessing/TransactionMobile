﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.OS;

namespace TransactionProcessor.Mobile.BusinessLogic.UIServices
{
    public static partial class DeviceInformationService
    {
        public static partial String Model() => Build.Model;
        
        public static partial String Platform()
        {
            return $"Android {Build.VERSION.Release} (API {AndroidSDK} - {AndroidCodename()})";
        }

        private static string AndroidCodename()
        {
            return (int)Build.VERSION.SdkInt switch
            {
                (int)BuildVersionCodes.Lollipop or (int)BuildVersionCodes.LollipopMr1 => "Lollipop",
                (int)BuildVersionCodes.M => "Marshmallow",
                (int)BuildVersionCodes.N or (int)BuildVersionCodes.NMr1 => "Nougat",
                (int)BuildVersionCodes.O or (int)BuildVersionCodes.OMr1 => "Oreo",
                (int)BuildVersionCodes.P => "Pie",
                (int)BuildVersionCodes.Q => "Q",
                (int)BuildVersionCodes.R => "R",
                (int)BuildVersionCodes.S => "S",
                32 => "Sv2",
                _ => "Unknown",
            };
        }

        private static int AndroidSDK => (int)Build.VERSION.SdkInt;
    }
}
