public class InterestRateCalculator
{
    public decimal GetAnnualInterestRate(decimal amount, int termMonths)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        if (termMonths <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(termMonths), "Term must be greater than zero months.");
        }

        decimal rate = amount switch
        {
            < 10_000m => 0.0300m,
            < 50_000m => 0.0350m,
            _ => 0.0400m
        };

        if (termMonths >= 60)
        {
            rate += 0.0100m;
        }
        else if (termMonths >= 36)
        {
            rate += 0.0075m;
        }
        else if (termMonths >= 12)
        {
            rate += 0.0050m;
        }

        return rate;
    }

    public decimal CalculateInterest(decimal amount, int termMonths)
    {
        var annualRate = GetAnnualInterestRate(amount, termMonths);
        var years = termMonths / 12m;
        return amount * annualRate * years;
    }

    public decimal CalculateTotalAmount(decimal amount, int termMonths)
    {
        return amount + CalculateInterest(amount, termMonths);
    }
}
