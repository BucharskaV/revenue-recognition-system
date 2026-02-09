namespace RevenueRecognitionSystem.Domain.Exceptions.SoftwareSystem;

public class SoftwareSystemNotFoundException(int id) : Exception($"The software system with id {id} was not found.");