namespace RevenueRecognitionSystem.Domain.Exceptions.Payment;

public class ContractIsCancelledException(int id) : Exception($"Contract with id {id} is already cancelled.");