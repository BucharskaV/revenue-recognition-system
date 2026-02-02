namespace RevenueRecognitionSystem.Domain.Exceptions;

public class CompanyAlreadyExistsException(string krs) : Exception($"Company with KRS number {krs} already exists.");