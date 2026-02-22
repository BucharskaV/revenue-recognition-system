namespace RevenueRecognitionSystem.Domain.Exceptions.Subscription;

public class SubscriptionNotFoundException(int id) : Exception($"Subscription with id {id} not found");