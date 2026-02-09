namespace RevenueRecognitionSystem.Domain.Exceptions.Contract;

public class ClientAlreadyHasContractException(): Exception($"Client already has active contract with this software system");