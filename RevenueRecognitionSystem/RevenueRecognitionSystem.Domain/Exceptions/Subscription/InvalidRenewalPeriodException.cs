namespace RevenueRecognitionSystem.Domain.Exceptions.Subscription;

public class InvalidRenewalPeriodException() : Exception("Renewal period must be between 1 and 24 months.");