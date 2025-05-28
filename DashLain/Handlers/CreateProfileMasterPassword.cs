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

public sealed class CreateProfileMasterPasswordCommand : IRequest<Result<Profile>> {
    public string Name { get; set; }

    public string Password { get; set; }
}

public sealed class Profile
{
    public string Name { get; set; }
}

public sealed class CreateProfileMasterPasswordHandler : IRequestHandler<CreateProfileMasterPasswordCommand, Result<Profile>> {
    readonly ProfileService _profileService;

    public CreateProfileMasterPasswordHandler(ProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task<Result<Profile>> Handle(CreateProfileMasterPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _profileService.CreateProfileWithMasterPassword(request);
    }
}

public sealed class CreateProfileMasterPasswordRule : AbstractValidator<CreateProfileMasterPasswordCommand>
{
    public CreateProfileMasterPasswordRule()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Profile name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(12).WithMessage("Password must be at least 12 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain a digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain a special character.");
    }
}