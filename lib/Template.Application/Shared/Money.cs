using FluentResults;
using Template.Application.Common;

namespace Template.Application.Shared;

public record Money : ValueObject
{
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    public decimal Amount { get; init; }

    public string Currency { get; init; }

    public static Result<Money> From(decimal amount, string currency)
    {
        var money = new Money(amount, currency);

        if (!SupportedCurrencies.Contains(money.Currency))
        {
            return Result.Fail<Money>($"{money.Currency} is not a supported currency");
        }

        return Result.Ok(money);
    }
    
    protected static IEnumerable<string> SupportedCurrencies
    {
        get
        {
            yield return "GBP";
            yield return "USD";
        }
    } 
}
