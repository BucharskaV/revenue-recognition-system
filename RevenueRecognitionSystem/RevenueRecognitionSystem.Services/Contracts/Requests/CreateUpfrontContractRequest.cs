namespace RevenueRecognitionSystem.Services.Contracts.Requests;

public class CreateUpfrontContractRequest
{
    public int ClientId { get; set; }
    public int SoftwareSystemId { get; set; }
    public string SoftwareVersion { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ExtraSupportYears { get; set; } //0-3 
}