using FluentValidation;

namespace Ducode.Wolk.Application.Users.Queries.SignIn
{
    public class SignInQueryValidator : AbstractValidator<SignInQuery>
    {
        public SignInQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
