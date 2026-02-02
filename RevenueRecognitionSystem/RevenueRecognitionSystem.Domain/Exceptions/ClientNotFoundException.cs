namespace RevenueRecognitionSystem.Domain.Exceptions;

public class ClientNotFoundException(int id) : Exception($"Client with id {id} not found");