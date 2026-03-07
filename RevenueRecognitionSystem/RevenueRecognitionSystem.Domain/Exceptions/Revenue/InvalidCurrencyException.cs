namespace RevenueRecognitionSystem.Domain.Exceptions.Revenue;

public class InvalidCurrencyException(string m) : Exception($"{m}");