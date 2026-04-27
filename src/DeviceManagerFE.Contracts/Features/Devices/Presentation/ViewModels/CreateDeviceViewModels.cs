namespace DeviceManagerFE.Features.Devices.Presentation.ViewModels;

public sealed class CreateDeviceFormViewModel
{
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Mã thiết bị là bắt buộc.")]
    [System.ComponentModel.DataAnnotations.StringLength(50, ErrorMessage = "Mã thiết bị tối đa 50 ký tự.")]
    public string DeviceCode { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Tên thiết bị là bắt buộc.")]
    [System.ComponentModel.DataAnnotations.StringLength(150, ErrorMessage = "Tên thiết bị tối đa 150 ký tự.")]
    public string DeviceName { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Loại thiết bị là bắt buộc.")]
    public int? CategoryId { get; set; }

    [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "Hãng sản xuất tối đa 100 ký tự.")]
    public string? Brand { get; set; }

    [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "Model tối đa 100 ký tự.")]
    public string? Model { get; set; }

    [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "Số serial tối đa 100 ký tự.")]
    public string? SerialNumber { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public DateTime? WarrantyExpiryDate { get; set; }

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Trạng thái là bắt buộc.")]
    public string Status { get; set; } = "Available";

    public int? EmployeeId { get; set; }

    [System.ComponentModel.DataAnnotations.StringLength(500, ErrorMessage = "Ghi chú tối đa 500 ký tự.")]
    public string? Note { get; set; }
}
