using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.OrderDtos;
using ChinarAz.Application.DTOs.OrderProductDtos;
using ChinarAz.Application.Events;
using ChinarAz.Application.Shared;
using ChinarAz.Domain.Entities;
using ChinarAz.Domain.Enums;
using Elastic.Clients.Elasticsearch.Security;
using Hangfire;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace ChinarAz.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderService(
        IOrderRepository orderRepository,
        IOrderProductRepository orderProductRepository,
        IHttpContextAccessor httpContextAccessor,
        IProductRepository productRepository,
        IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _orderProductRepository = orderProductRepository;
        _httpContextAccessor = httpContextAccessor;
        _productRepository = productRepository;
        _publishEndpoint = publishEndpoint;
    }

    // =======================
    // Helper: Token-dən UserId alma
    // =======================
    private Guid GetUserIdFromToken()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
            throw new UnauthorizedAccessException("User token etibarlı deyil.");
        return Guid.Parse(claim.Value);
    }

    // =======================
    // Müştəri metodları
    // =======================
    public async Task<BaseResponse<string>> CreateAsync(OrderCreateDto dto)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

            if (dto.Products == null || !dto.Products.Any())
                return new BaseResponse<string>("No products in the order", HttpStatusCode.BadRequest);

            var order = new Order
            {
                UserId = userId.ToString(),
                Status = OrderStatus.Pending,
            };

            decimal totalPrice = 0;
            foreach (var p in dto.Products)
            {
                var product = await _productRepository.GetByIdAsync(p.ProductId);
                if (product == null)
                    return new BaseResponse<string>($"Product {p.ProductId} not found", HttpStatusCode.NotFound);

                if (product.IsWeighted)
                {
                    if (p.WeightGram <= 0 || p.WeightGram > 2)
                        return new BaseResponse<string>(
                            $"Product {product.Name} requires WeightKg > 0 and <= 2",
                            HttpStatusCode.BadRequest);

                    if (p.Quantity > 0)
                        return new BaseResponse<string>(
                            $"Product {product.Name} is weighted. Do not enter Quantity.",
                            HttpStatusCode.BadRequest);
                }
                else
                {
                    if (p.Quantity <= 0)
                        return new BaseResponse<string>($"Product {product.Name} requires Quantity > 0", HttpStatusCode.BadRequest);

                    if (p.WeightGram > 0)
                        return new BaseResponse<string>(
                            $"Product {product.Name} is not weighted. Do not enter WeightKg.",
                            HttpStatusCode.BadRequest);
                }

                var orderProduct = new OrderProduct
                {
                    ProductId = product.Id,
                    UnitPrice = product.Price,
                    Quantity = product.IsWeighted ? 0 : p.Quantity,
                    WeightGram = product.IsWeighted ? p.WeightGram : 0,
                    Order = order
                };

                totalPrice += product.IsWeighted
                    ? product.Price * p.WeightGram
                    : product.Price * p.Quantity;

                order.OrderProducts.Add(orderProduct);
            }

            order.TotalPrice = totalPrice;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangeAsync();

            await _publishEndpoint.Publish(
                new OrderCreatedEvent(order.Id, userEmail, order.TotalPrice, userId)
            );


            return new BaseResponse<string>("Order created successfully", HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>($"Xəta: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            return new BaseResponse<string>("Order tapılmadı", HttpStatusCode.NotFound);

        var userId = GetUserIdFromToken();
        if (order.UserId != userId.ToString())
            return new BaseResponse<string>("Bu sifarişi silmək icazəniz yoxdur", HttpStatusCode.Forbidden);

        _orderRepository.Delete(order);
        await _orderRepository.SaveChangeAsync();
        return new BaseResponse<string>("Order deleted successfully", HttpStatusCode.OK);
    }

    public async Task<BaseResponse<OrderGetDto>> GetByIdAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            return new BaseResponse<OrderGetDto>("Order tapılmadı", HttpStatusCode.NotFound);

        var userId = GetUserIdFromToken();
        if (order.UserId != userId.ToString())
            return new BaseResponse<OrderGetDto>("Bu sifarişi görmək icazəniz yoxdur", HttpStatusCode.Forbidden);

        var dto = MapOrderToDto(order);
        return new BaseResponse<OrderGetDto>("", dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<OrderGetDto>>> GetMyOrdersAsync()
    {
        var userId = GetUserIdFromToken();
        var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
        var dtos = orders.Select(MapOrderToDto).ToList();
        return new BaseResponse<List<OrderGetDto>>("", dtos, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> UpdateAsync(OrderUpdateDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(dto.Id);
        if (order == null)
            return new BaseResponse<string>("Order tapılmadı", HttpStatusCode.NotFound);

        var userId = GetUserIdFromToken();
        if (order.UserId != userId.ToString())
            return new BaseResponse<string>("Bu sifarişi yeniləmək icazəniz yoxdur", HttpStatusCode.Forbidden);

        order.Status = dto.Status;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();

        return new BaseResponse<string>("Order updated successfully", HttpStatusCode.OK);
    }

    // =======================
    // Admin metodları
    // =======================
    public async Task<BaseResponse<List<OrderGetDto>>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAll()
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .ToListAsync();

        var dtos = orders.Select(MapOrderToDto).ToList();
        return new BaseResponse<List<OrderGetDto>>("", dtos, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<OrderGetDto>> GetByIdAdminAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            return new BaseResponse<OrderGetDto>("Order tapılmadı", HttpStatusCode.NotFound);

        var dto = MapOrderToDto(order);
        return new BaseResponse<OrderGetDto>("",dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> UpdateAdminAsync(OrderUpdateDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(dto.Id);
        if (order == null)
            return new BaseResponse<string>("Order tapılmadı", HttpStatusCode.NotFound);

        order.Status = dto.Status;
        _orderRepository.Update(order);
        await _orderRepository.SaveChangeAsync();

        return new BaseResponse<string>("Order updated successfully", HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> DeleteAdminAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            return new BaseResponse<string>("Order tapılmadı", HttpStatusCode.NotFound);

        _orderRepository.Delete(order);
        await _orderRepository.SaveChangeAsync();
        return new BaseResponse<string>("Order deleted successfully", HttpStatusCode.OK);
    }

    // =======================
    // Helper: Order -> DTO
    // =======================
    private OrderGetDto MapOrderToDto(Order order)
    {
        return new OrderGetDto
        {
            Id = order.Id,
            UserId = Guid.Parse(order.UserId),
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            Products = order.OrderProducts.Select(op => new OrderProductGetDto
            {
                ProductId = op.ProductId,
                ProductName = op.Product.Name,
                Quantity = op.Quantity,
                UnitPrice = op.UnitPrice,
                WeightGram = op.WeightGram
            }).ToList()
        };
    }

    public async Task UpdatePendingOrdersAsync()
    {
        var threshold = DateTime.UtcNow.AddMinutes(-15);
        var pendingOrders = await _orderRepository.GetAll()
            .Where(o => o.Status == OrderStatus.Pending && o.CreatedAt <= threshold)
            .ToListAsync();

        if (!pendingOrders.Any())
            return;

        foreach (var order in pendingOrders)
        {
            order.Status = OrderStatus.Cancelled;
            _orderRepository.Update(order);
        }

        await _orderRepository.SaveChangeAsync();
    }
}