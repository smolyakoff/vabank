using System;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using FluentValidation.Validators;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership;
using VaBank.Services.Common.Validation;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Queries;

namespace VaBank.Services.Membership
{
    [ValidatorName("login")]
    [StaticValidator]
    internal class LoginValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty).Length(4, 50)
                .WithLocalizedMessage(() => Messages.FieldLength, 4);
        }
    }

    [ValidatorName("password")]
    [StaticValidator]
    internal class PasswordValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty).Length(6, 256)
                .WithLocalizedMessage(() => Messages.FieldLength, 6);
        }
    }

    [ValidatorName("phone")]
    [StaticValidator]
    internal class PhoneNumberValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty).Matches(@"\+375 *\(?(29|33|44|25)\)? *\d{7}")
                .WithLocalizedMessage(() => Messages.CheckNumberPhone);
        }
    }

    [StaticValidator]
    internal class LoginCommandValidator : AbstractValidator<LoginCommand>
    {        
        public LoginCommandValidator()
        {
            RuleFor(x => x.Login).UseValidator(new LoginValidator());
            RuleFor(x => x.Password).NotEmpty().Length(6, 256);
        }
    }

    [StaticValidator]
    internal class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
    {
        public CreateTokenCommandValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().Length(1, 256);
            RuleFor(x => x.Id).NotEmpty().Length(1, 256);
            RuleFor(x => x.ExpiresUtc).Must((command, expireUtc) => expireUtc >= command.IssuedUtc)
                .WithLocalizedMessage(() => "ExpreUtc value can't be less than IssuedUtc value.");
            RuleFor(x => x.ProtectedTicket).NotEmpty().Length(1, 512);
            RuleFor(x => x.UserId).Must(x => x != Guid.Empty);
        }
    }

    internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IQueryRepository<User> _userRepository; 
        public CreateUserCommandValidator(IQueryRepository<User> userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }
            _userRepository = userRepository;
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty);
            RuleFor(x => x.LastName).NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty);
            RuleFor(x => x.UserName).UseValidator(new LoginValidator()).Must(IsUserNameUnique)
                .WithLocalizedMessage(() => Messages.UserNameUnique);
            RuleFor(x => x.Password).UseValidator(new PasswordValidator());
            RuleFor(x => x.PasswordConfirmation).NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty)
                .Equal(x => x.Password).WithLocalizedMessage(() => Messages.PasswordConfirmation);
            RuleFor(x => x.PhoneNumber).UseValidator(new PhoneNumberValidator());
        }
        private bool IsUserNameUnique(CreateUserCommand command, string userName, PropertyValidatorContext context)
        {
            var query = DbQuery.For<User>().FilterBy(x => x.Deleted == false && x.UserName == userName);
            var existingUser = _userRepository.QueryOne(query);
            return existingUser == null;
        }
    }

    internal class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword).UseValidator(new PasswordValidator());
            RuleFor(x => x.NewPassword).UseValidator(new PasswordValidator());
            RuleFor(x => x.NewPasswordConfirmation).UseValidator(new PasswordValidator()).Equal(x => x.NewPassword)
                .WithLocalizedMessage(() => Messages.PasswordConfirmation);
        }
    }

    internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
         private readonly IQueryRepository<User> _userRepository; 
        public UpdateUserCommandValidator(IQueryRepository<User> userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }
            _userRepository = userRepository;
            RuleFor(x => x.UserId).Must(x => x != Guid.Empty);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty);
            RuleFor(x => x.LastName).NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty);
            RuleFor(x => x.UserName).UseValidator(new LoginValidator()).Must(IsUserNameUnique)
                .WithLocalizedMessage(() => Messages.UserNameUnique);
            RuleFor(x => x.Password).NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty)
                .UseValidator(new PasswordValidator()).When(x => x.ChangePassword);
            RuleFor(x => x.PasswordConfirmation).NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty)
                .Equal(x => x.Password).WithLocalizedMessage(() => Messages.PasswordConfirmation)
                .When(x => x.ChangePassword);
            RuleFor(x => x.PhoneNumber).UseValidator(new PhoneNumberValidator());
        }
        private bool IsUserNameUnique(CreateUserCommand command, string userName, PropertyValidatorContext context)
        {
            var query = DbQuery.For<User>().FilterBy(x => x.Deleted == false && x.UserName == userName);
            var existingUser = _userRepository.QueryOne(query);
            return existingUser == null;
        }
    }

    internal class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty);
            RuleFor(x => x.LastName).NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty);
            RuleFor(x => x.PhoneNumber).UseValidator(new PhoneNumberValidator());
            RuleFor(x => x.UserId).Must(x => x != Guid.Empty);
        }
    }

    internal class UsersQueryValidator : AbstractValidator<UsersQuery>
    {
        public UsersQueryValidator()
        {
            RuleFor(x => x.ClientFilter).NotEmpty();
            RuleFor(x => x.ClientPage).NotEmpty();
        }
    }
}
