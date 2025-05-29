using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Handlers;

public sealed class AddVaultEntryCommand {
    public string Title { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}

public sealed class AddVaultEntryRule : AbstractValidator<AddVaultEntryCommand>
{
    public AddVaultEntryRule()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Username).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(12).WithMessage("Password must be at least 12 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain a digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain a special character.");
    }
}