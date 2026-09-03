using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests
{
    public class ExtendedBaseViewModelTests{
        private IApplicationCacheImposter ApplicationCache = null;

        private IDialogServiceImposter DialogService = null;

        private INavigationServiceImposter NavigationService = null;
        private INavigationParameterServiceImposter NavigationParameterService = null;

        private IDeviceServiceImposter DeviceService = null;

        private ExtendedBaseViewModel ViewModel = null;
        public ExtendedBaseViewModelTests(){
            this.ApplicationCache = new IApplicationCacheImposter();
            this.DialogService = new IDialogServiceImposter();
            this.NavigationService = new INavigationServiceImposter();
            this.DeviceService = new IDeviceServiceImposter();
            this.NavigationParameterService = new INavigationParameterServiceImposter();
            this.ViewModel = new ExtendedBaseViewModel(this.ApplicationCache.Instance(),
                                                                        this.DialogService.Instance(),
                                                                        this.NavigationService.Instance(),
                                                                        this.DeviceService.Instance(),
                                                                        this.NavigationParameterService.Instance());
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
        [InlineData(Orientation.Landscape)]
        [InlineData(Orientation.Portrait)]
        public async Task ExtendedBaseViewModel_Initialise_OrientationIsSet(Orientation orientation){
            var viewModel = new ExtendedBaseViewModel(this.ApplicationCache.Instance(),
                                                      this.DialogService.Instance(),
                                                      this.NavigationService.Instance(),
                                                      this.DeviceService.Instance(),
                                                      this.NavigationParameterService.Instance(),
                                                      orientation);
            await this.ViewModel.Initialise(CancellationToken.None);
            this.DeviceService.SetOrientation(Arg<Orientation>.Any()).Called(Count.Once());
            viewModel.Orientation.ShouldBe(orientation);
        }

    }
}
