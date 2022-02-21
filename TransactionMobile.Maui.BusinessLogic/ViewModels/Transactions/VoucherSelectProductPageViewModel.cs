namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Web;
using System.Windows.Input;
using MediatR;
using Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Requests;
using UIServices;

public class VoucherSelectProductPageViewModel : BaseViewModel, IQueryAttributable
{
    #region Fields

    private readonly IMediator Mediator;

    private readonly INavigationService NavigationService;

    #endregion

    #region Constructors

    public void ApplyQueryAttributes(IDictionary<string, Object> query)
    {
        this.OperatorIdentifier = HttpUtility.UrlDecode(query[nameof(this.OperatorIdentifier)].ToString());
    }

    public VoucherSelectProductPageViewModel(IMediator mediator, INavigationService navigationService)
    {
        this.Mediator = mediator;
        this.NavigationService = navigationService;
        this.ProductSelectedCommand = new AsyncCommand<ItemSelected<ContractProductModel>>(this.ProductSelectedCommandExecute);
        this.Title = "Select a Product";
    }

    #endregion

    #region Properties

    public String OperatorIdentifier { get; private set; }

    public List<ContractProductModel> Products { get; private set; }

    public ICommand ProductSelectedCommand { get; }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken)
    {
        GetContractProductsRequest request = GetContractProductsRequest.Create("", Guid.Empty, Guid.Empty, ProductType.Voucher);

        List<ContractProductModel> products = await this.Mediator.Send(request, cancellationToken);

        products = products.Where(p => p.OperatorIdentfier == this.OperatorIdentifier).ToList();

        this.Products = products;
    }

    private async Task ProductSelectedCommandExecute(ItemSelected<ContractProductModel> e)
    {
        await this.NavigationService.GoToVoucherIssueVoucherPage(e.SelectedItem.OperatorIdentfier, e.SelectedItem.ContractId, e.SelectedItem.ProductId, e.SelectedItem.Value);
    }

    #endregion
}