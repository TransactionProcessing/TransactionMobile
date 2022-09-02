﻿namespace TransactionMobile.Maui.UiTests.Features;

using Drivers;
using NUnit.Framework;
using UITests.Common;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
[TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
[NonParallelizable]
public partial class LoginFeature : BaseTestFixture
{
    #region Constructors

    public LoginFeature(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform) {
    }

    #endregion
}