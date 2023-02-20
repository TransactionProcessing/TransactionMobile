using EmptyFiles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionMobile.Maui.UITests.Common;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UiTests.Features
{
    [TestFixture(MobileTestPlatform.Android, Category = "Android")]
    [TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
    [NonParallelizable]
    public partial class EndToEndTestsFeature : BaseTestFixture
    {
        #region Constructors

        public EndToEndTestsFeature(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform)
        {
        }

        #endregion
    }

}
