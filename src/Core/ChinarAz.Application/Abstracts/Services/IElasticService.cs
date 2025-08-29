
using ChinarAz.Application.Models;

namespace ChinarAz.Application.Abstracts.Services;

public interface IElasticService
{
    Task IndexProductAsync(ProductElasticModel model);
    Task UpdateProductAsync(ProductElasticModel model);
    Task DeleteProductAsync(Guid id);
    Task<List<ProductElasticModel>> SearchProductsAsync(string keyword);
}
