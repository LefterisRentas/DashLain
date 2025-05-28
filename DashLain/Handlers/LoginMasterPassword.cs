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
    public string Name { get; set; }

    public string Password { get; set; }
}

public sealed class LoginMasterPasswordHandler : IRequestHandler<LoginMasterPasswordCommand, UIResult<bool>> {
    readonly ProfileService _profileService;

    public LoginMasterPasswordHandler(ProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task<UIResult<bool>> Handle(LoginMasterPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _profileService.LoginWithMasterPassword(request);
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