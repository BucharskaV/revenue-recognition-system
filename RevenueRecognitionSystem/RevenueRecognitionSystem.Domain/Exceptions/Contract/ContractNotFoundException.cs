namespace RevenueRecognitionSystem.Domain.Exceptions.Contract;

public class ContractNotFoundException(int id) : Exception($"Contract with id {id} not found");