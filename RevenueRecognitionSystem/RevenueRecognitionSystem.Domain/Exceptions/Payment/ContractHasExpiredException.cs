namespace RevenueRecognitionSystem.Domain.Exceptions.Payment;

public class ContractHasExpiredException(DateTime date) : Exception($"Contract is expired in {date}");