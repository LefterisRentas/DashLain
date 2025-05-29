using DashLain.Models;
using DashLain.Services;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Handlers;

public sealed class LoginMasterPasswordCommand : IRequest<UIResult<bool>> {
    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public sealed class LoginMasterPasswordHandler(ProfileService profileService) : IRequestHandler<LoginMasterPasswordCommand, UIResult<bool>> {
    public async Task<UIResult<bool>> Handle(LoginMasterPasswordCommand request, CancellationToken cancellationToken)
    {
        return await profileService.LoginWithMasterPassword(request);
    }
}

public sealed class LoginMasterPasswordRule : AbstractValidator<LoginMasterPasswordCommand>
{
    public LoginMasterPasswordRule()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}