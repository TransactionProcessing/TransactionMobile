﻿using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public partial class VoucherSelectProductPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    public ProductDetails ProductDetails { get; private set; }

    #endregion

    #region Constructors
    
    public VoucherSelectProductPageViewModel(IMediator mediator,
                                             INavigationService navigationService,
                                             IApplicationCache applicationCache,
                                             IDialogService dialogService,
                                             IDeviceService deviceService,
                                             INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Mediator = mediator;
        this.Title = "Select a Product";
    }

    #endregion

    #region Properties

    public List<ContractProductModel> Products { get; private set; }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken)
    {
        IDictionary<String, Object> query = this.NavigationParameterService.GetParameters();
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;

        GetContractProductsRequest request = GetContractProductsRequest.Create(ProductType.Voucher);

        var result = await this.Mediator.Send(request, cancellationToken);
        var products = result.Data;
        products = products.Where(p => p.OperatorId == this.ProductDetails.OperatorId).ToList();

        this.Products = products;
    }

    [RelayCommand]
    private async Task ProductSelected(ItemSelected<ContractProductModel> e)
    {
        Logger.LogInformation("ProductSelected called");
        ProductDetails productDetails = new ProductDetails()
                                        {
                                            OperatorId = e.SelectedItem.OperatorId,
                                            ContractId = e.SelectedItem.ContractId,
                                            ProductId = e.SelectedItem.ProductId
                                        };
        await this.NavigationService.GoToVoucherIssueVoucherPage(productDetails, e.SelectedItem.Value);
    }

    #endregion
}