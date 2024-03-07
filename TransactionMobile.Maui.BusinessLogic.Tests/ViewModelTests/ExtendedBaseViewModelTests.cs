using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests
{
    using System.Threading;
    using Common;
    using Maui.UIServices;
    using Microsoft.Maui.Devices;
    using Moq;
    using RequestHandlers;
    using Services;
    using Shouldly;
    using SimpleResults;
    using UIServices;
    using ViewModels;
    using Xunit;

    public class ExtendedBaseViewModelTests{
        private Mock<IApplicationCache> ApplicationCache = null;

        private Mock<IDialogService> DialogService = null;

        private Mock<INavigationService> NavigationService = null;

        private Mock<IDeviceService> DeviceService = null;

        private ExtendedBaseViewModel ViewModel = null;
        public ExtendedBaseViewModelTests(){
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.DialogService = new Mock<IDialogService>();
            this.NavigationService = new Mock<INavigationService>();
            this.DeviceService = new Mock<IDeviceService>();
            this.ViewModel = new ExtendedBaseViewModel(this.ApplicationCache.Object,
                                                                        this.DialogService.Object,
                                                                        this.NavigationService.Object,
                                                                        this.DeviceService.Object);
        }

        [Fact]
        public async Task ExtendedBaseViewModel_HandleResult_ResultIsNull_ErrorThrown(){
            Result<String> result = null;
            ApplicationException exception= Should.Throw<ApplicationException>(() => {
                                                                                   this.ViewModel.HandleResult(result);
                                                                               });
            exception.Message.ShouldBe("Result from function call was null");
        }

        [Fact]
        public async Task ExtendedBaseViewModel_HandleResult_ResultIsAndError_ErrorThrown(){
            Result<String> result = Result.Failure("Error has been returned");
            
            ApplicationException exception = Should.Throw<ApplicationException>(() => {
                                                                                    this.ViewModel.HandleResult(result);
                                                                                });
            exception.Message.ShouldBe("Error has been returned");
        }

        [Theory]
        [InlineData(DisplayOrientation.Landscape)]
        [InlineData(DisplayOrientation.Portrait)]
        public async Task ExtendedBaseViewModel_Initialise_OrientationIsSet(DisplayOrientation orientation){
            var viewModel = new ExtendedBaseViewModel(this.ApplicationCache.Object,
                                                      this.DialogService.Object,
                                                      this.NavigationService.Object,
                                                      this.DeviceService.Object,
                                                      orientation);
            await this.ViewModel.Initialise(CancellationToken.None);
            this.DeviceService.Verify(v => v.SetOrientation(It.IsAny<DisplayOrientation>()), Times.Once);
            viewModel.Orientation.ShouldBe(orientation);
        }

    }
}
