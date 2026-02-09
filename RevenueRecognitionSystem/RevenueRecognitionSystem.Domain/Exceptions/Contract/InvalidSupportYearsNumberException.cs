namespace RevenueRecognitionSystem.Domain.Exceptions.Contract;

public class InvalidSupportYearsNumberException():Exception("Invalid contract support years number. We can only extend the support by 1, 2 or 3 years. In case of not willing of additional support - enter 0.");