using System;
using System.Collections.Generic;
using FluentValidation;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership;
using VaBank.Services.Common.Validation;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.Services.Membership
{
    [ValidatorName("login")]
    public class LoginValidator : ObjectValidator<string>
    {
        private readonly IQueryRepository<User> _users; 

        public LoginValidator(IQueryRepository<User> userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }
            _users = userRepository;
        }

        public override IList<ValidationFault> Validate(string obj)
        {
            var value = new Container(obj);
            var inline = new InlineValidator<Container>();
            inline.RuleFor(x => x.Value)
                .NotEmpty()
                .Length(6, 20)
                .Must(LoginUnique)
                .WithName("Login");
            return inline.Validate(value).Errors.ToValidationFaults("login");
        }

        private bool LoginUnique(string login)
        {
            return _users.Count(DbQuery.For<User>().WithFilter(x => x.UserName == login)) == 0;
        }
    }
}
