using DashLain.Models;
using DashLain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Handlers;

public sealed class LoginMasterPasswordCommand : IRequest<Result<LoginResult>> {
    public string Name { get; set; }

    public string Password { get; set; }
}

public sealed class LoginResult
{

}

public sealed class LoginMasterPasswordHandler : IRequestHandler<LoginMasterPasswordCommand, Result<LoginResult>> {
    readonly ProfileService _profileService;

    public LoginMasterPasswordHandler(ProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task<Result<LoginResult>> Handle(LoginMasterPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _profileService.LoginWithMasterPassword(request);
    }
}