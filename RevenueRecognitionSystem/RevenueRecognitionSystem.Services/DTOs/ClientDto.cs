namespace RevenueRecognitionSystem.Services.DTOs;

public class ClientDto
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}