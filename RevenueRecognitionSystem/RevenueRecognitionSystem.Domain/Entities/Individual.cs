namespace RevenueRecognitionSystem.Domain.Entities;

public class Individual : Client
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PESEL { get; set; } = string.Empty;
    
    public bool IsDeleted { get; set; }
}