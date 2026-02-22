namespace RevenueRecognitionSystem.Domain.Exceptions.Subscription;

public class SubscriptionIsCancelledException(int id) : Exception($"Subscription with id {id} is cancelled");