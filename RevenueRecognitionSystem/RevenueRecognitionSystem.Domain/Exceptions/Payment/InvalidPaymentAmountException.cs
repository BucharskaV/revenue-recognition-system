namespace RevenueRecognitionSystem.Domain.Exceptions.Payment;

public class InvalidPaymentAmountException() : Exception($"The amount must be greater than zero.");