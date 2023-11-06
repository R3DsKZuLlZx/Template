using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using ValueOf;

namespace Template.Application.Customers.Common;

public class Username : ValueOf<string, Username>
{
    private static readonly Regex UsernameRegex =
        new("^(?=.{1,30}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    protected override void Validate()
    {
        if (!UsernameRegex.IsMatch(Value))
        {
            var message = $"{Value} is not a valid username";
            throw new ValidationException(
                message,
                new[]
                {
                    new ValidationFailure(nameof(Username), message),
                });
        }
    }
}
