using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.OrderDtos;
using ChinarAz.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinarAz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // ========================================
    // Müştəri metodları
    // ========================================

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
    {
        var result = await _orderService.CreateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("my-orders")]
    [Authorize]
    [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetMyOrders()
    {
        var result = await _orderService.GetMyOrdersAsync();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("my-orders/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _orderService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Update([FromBody] OrderUpdateDto dto)
    {
        var result = await _orderService.UpdateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _orderService.DeleteAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    // ========================================
    // Admin metodları
    // ========================================

    [HttpGet("all")]
    [Authorize(Policy = Permissions.Order.GetAll)]
    [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _orderService.GetAllAsync();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("admin/{id}")]
    [Authorize(Policy = Permissions.Order.GetByIdAdmin)]
    [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetByIdAdmin([FromRoute] Guid id)
    {
        var result = await _orderService.GetByIdAdminAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("admin")]
    [Authorize(Roles = "Order.UpdateAdmin")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateAdmin([FromBody] OrderUpdateDto dto)
    {
        var result = await _orderService.UpdateAdminAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("admin/{id}")]
    [Authorize(Policy = Permissions.Order.DeleteAdmin)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteAdmin([FromRoute] Guid id)
    {
        var result = await _orderService.DeleteAdminAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }
}
