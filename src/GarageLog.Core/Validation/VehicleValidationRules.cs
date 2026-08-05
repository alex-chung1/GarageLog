namespace GarageLog.Core.Validation;

public static class VehicleValidationRules
{
    private const int MinVehicleYear = 1886;
    private const int MaxReasonableMileage = 5_000_000;

    public static void ValidateYear(int year)
    {
        if (year < MinVehicleYear || year > DateTime.UtcNow.Year + 1)
        {
            throw new ArgumentException("Year is not a valid vehicle year.");
        }
    }

    public static void ValidateMileage(int? mileage)
    {
        if (mileage is < 0 or > MaxReasonableMileage)
        {
            throw new ArgumentException(
                $"Mileage must be between 0 and {MaxReasonableMileage:N0}."
            );
        }
    }

    public static void ValidateMakeModel(string make, string model)
    {
        if (string.IsNullOrWhiteSpace(make))
            throw new ArgumentException("Make is required.");

        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model is required.");
    }
}
