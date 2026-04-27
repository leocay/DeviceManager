namespace DeviceManagerFE.Features.Devices.Presentation.ViewModels;

public sealed class CreateDeviceFormViewModel
{
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Ma thiet bi la bat buoc.")]
    [System.ComponentModel.DataAnnotations.StringLength(50, ErrorMessage = "Ma thiet bi toi da 50 ky tu.")]
    public string DeviceCode { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Ten thiet bi la bat buoc.")]
    [System.ComponentModel.DataAnnotations.StringLength(150, ErrorMessage = "Ten thiet bi toi da 150 ky tu.")]
    public string DeviceName { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Loai thiet bi la bat buoc.")]
    public int? CategoryId { get; set; }

    [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "Hang san xuat toi da 100 ky tu.")]
    public string? Brand { get; set; }

    [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "Model toi da 100 ky tu.")]
    public string? Model { get; set; }

    [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "So serial toi da 100 ky tu.")]
    public string? SerialNumber { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public DateTime? WarrantyExpiryDate { get; set; }

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Trang thai la bat buoc.")]
    public string Status { get; set; } = "Available";

    public int? EmployeeId { get; set; }

    [System.ComponentModel.DataAnnotations.StringLength(500, ErrorMessage = "Ghi chu toi da 500 ky tu.")]
    public string? Note { get; set; }
}
