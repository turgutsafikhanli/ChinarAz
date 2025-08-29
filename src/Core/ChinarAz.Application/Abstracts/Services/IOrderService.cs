using ChinarAz.Application.DTOs.OrderDtos;
using ChinarAz.Application.Shared;

namespace ChinarAz.Application.Abstracts.Services;

public interface IOrderService
{
    // =======================
    // Müştəri üçün metodlar
    // =======================
    Task<BaseResponse<string>> CreateAsync(OrderCreateDto dto);              // Yeni sifariş yaratmaq
    Task<BaseResponse<string>> DeleteAsync(Guid orderId);                    // Öz sifarişini silmək
    Task<BaseResponse<OrderGetDto>> GetByIdAsync(Guid orderId);              // Öz sifarişinin detallarını görmək
    Task<BaseResponse<List<OrderGetDto>>> GetMyOrdersAsync();                // Token-dən istifadə edərək öz sifarişlərini gətirmək
    Task<BaseResponse<string>> UpdateAsync(OrderUpdateDto dto);              // Öz sifarişini yeniləmək

    // =======================
    // Admin üçün metodlar
    // =======================
    Task<BaseResponse<List<OrderGetDto>>> GetAllAsync();                     // Bütün sifarişləri görmək
    Task<BaseResponse<OrderGetDto>> GetByIdAdminAsync(Guid orderId);         // İstənilən sifarişi görmək
    Task<BaseResponse<string>> UpdateAdminAsync(OrderUpdateDto dto);         // İstənilən sifarişi yeniləmək
    Task<BaseResponse<string>> DeleteAdminAsync(Guid orderId);               // İstənilən sifarişi silmək
    Task UpdatePendingOrdersAsync();
}