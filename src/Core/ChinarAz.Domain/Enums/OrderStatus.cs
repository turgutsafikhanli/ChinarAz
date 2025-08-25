namespace ChinarAz.Domain.Enums;

public enum OrderStatus
{
    Pending = 0,     // Sifariş yaradılıb, ödəniş gözlənilir
    Paid = 1,        // Ödəniş uğurla alınıb
    Confirmed = 2,   // Admin tərəfindən təsdiqlənib
    Shipped = 3,     // Göndərilib
    Delivered = 4,   // İstifadəçiyə çatıb
    Cancelled = 5,   // Ləğv olunub
    Failed = 6      // Ödəniş uğursuz olub
}
