using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.ProductDtos;
using ChinarAz.Application.Models;
using ChinarAz.Application.Shared;
using Elastic.Clients.Elasticsearch;
using System.Net;

namespace ChinarAz.Persistence.Services;

public class ElasticService : IElasticService
{
    private readonly ElasticsearchClient _client;
    private const string IndexName = "products";

    public ElasticService(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task IndexProductAsync(ProductElasticModel model)
    {
        await _client.IndexAsync(model, idx => idx.Index(IndexName).Id(model.Id));
    }

    public async Task UpdateProductAsync(ProductElasticModel model)
    {
        await _client.UpdateAsync<ProductElasticModel, object>(model.Id, u => u
            .Index(IndexName)
            .Doc(model));
    }

    public async Task DeleteProductAsync(Guid id)
    {
        await _client.DeleteAsync<ProductElasticModel>(id, d => d.Index(IndexName));
    }

    public async Task<List<ProductElasticModel>> SearchProductsAsync(string keyword)
    {
        var response = await _client.SearchAsync<ProductElasticModel>(s => s
            .Index(IndexName)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Name)
                    .Query(keyword)
                    .Fuzziness(new Fuzziness(2))
                )
            )
            .Size(50)
        );
        if (!response.IsValidResponse || response.Hits == null)
            return new List<ProductElasticModel>();

        return response.Hits
            .Where(h => h?.Source != null)
            .Select(h => h.Source!)
            .ToList();
    }
}
