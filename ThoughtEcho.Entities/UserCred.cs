using System;
using System.Collections.Generic;

namespace ThoughtEcho.Entities;

public partial class UserCred
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsEmailVerified { get; set; }

    public virtual EmailVerificationOtp? EmailVerificationOtp { get; set; }

    public virtual RefreshToken? RefreshToken { get; set; }
}
