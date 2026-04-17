using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public record MerchantQueries {
    public record GetContractProductsQuery(ProductType? ProductType = null) : IRequest<Result<List<ContractProductModel>>>;
    public record GetMerchantBalanceQuery() : IRequest<Result<Decimal>>;
    public record GetMerchantDetailsQuery() : IRequest<Result<MerchantDetailsModel>>;
    public record GetProductOperatorsQuery(ProductType? ProductType) : IRequest<Result<List<ContractOperatorModel>>>;
}