namespace RevenueRecognitionSystem.Domain.Exceptions.Subscription;

public class PaymentOutsidePeriodException() : Exception("Payment not allowed outside renewal period.");