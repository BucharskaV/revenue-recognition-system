namespace RevenueRecognitionSystem.Domain.Exceptions;

public class IndividualAlreadyExistsException(string pesel) : Exception($"Individual with PESEL {pesel} already exists.");