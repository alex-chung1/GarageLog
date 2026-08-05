namespace GarageLog.Core.Validation;

public static class ServiceRecordValidationRules
{
    public static void ValidateServiceDate(DateOnly serviceDate)
    {
        if (serviceDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("Service date cannot be in the future.");
    }

    public static void ValidateTotalCost(decimal? totalCost)
    {
        if (totalCost is < 0)
            throw new ArgumentException("Total cost cannot be negative.");
    }

    public static string? NormalizeShopName(bool isSelfService, string? shopName)
    {
        var normalized =
            isSelfService ? null
            : string.IsNullOrWhiteSpace(shopName) ? null
            : shopName.Trim();

        if (!isSelfService && normalized is null)
            throw new ArgumentException("Shop name is required for non-self-service records.");

        return normalized;
    }
}
