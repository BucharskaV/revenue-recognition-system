namespace RevenueRecognitionSystem.Domain.Exceptions.Payment;

public class ContractIsFullyPaidException(int id) : Exception($"Contract with id {id} is already fully paid.");