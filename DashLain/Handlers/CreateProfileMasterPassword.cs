using DashLain.Models;
using DashLain.Resources;
using DashLain.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;

namespace DashLain.Handlers;

public sealed class CreateProfileMasterPasswordCommand : IRequest<UIResult<Profile>> {
    public required string Name { get; set; }

    public required string Password { get; set; }
}

public sealed class Profile
{
    public required string Name { get; set; }
}

public sealed class CreateProfileMasterPasswordHandler : IRequestHandler<CreateProfileMasterPasswordCommand, UIResult<Profile>> {
    readonly ProfileService _profileService;

    public CreateProfileMasterPasswordHandler(ProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task<UIResult<Profile>> Handle(CreateProfileMasterPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _profileService.CreateProfileWithMasterPassword(request);
    }
}

public sealed class CreateProfileMasterPasswordRule : AbstractValidator<CreateProfileMasterPasswordCommand>
{
    public CreateProfileMasterPasswordRule(IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(x => localizer["Profile name is required."])
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(x => localizer["Password is required."])
            .MinimumLength(12).WithMessage(x => localizer["Password must be at least 12 characters."])
            .Matches("[A-Z]").WithMessage(x => localizer["Password must contain an uppercase letter."])
            .Matches("[a-z]").WithMessage(x => localizer["Password must contain a lowercase letter."])
            .Matches("[0-9]").WithMessage(x => localizer["Password must contain a digit."])
            .Matches("[^a-zA-Z0-9]").WithMessage(x => localizer["Password must contain a special character."]);
    }
}
