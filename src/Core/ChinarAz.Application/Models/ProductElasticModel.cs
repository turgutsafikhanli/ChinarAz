namespace ChinarAz.Application.Models;

public class ProductElasticModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public decimal Price { get; set; }
    public bool IsWeighted { get; set; }
}
