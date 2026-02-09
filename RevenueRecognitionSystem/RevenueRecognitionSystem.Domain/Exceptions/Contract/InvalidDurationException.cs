namespace RevenueRecognitionSystem.Domain.Exceptions.Contract;

public class InvalidDurationException() : Exception("Invalid contract duration. The time range should be at least 3 days and at most 30 days.");