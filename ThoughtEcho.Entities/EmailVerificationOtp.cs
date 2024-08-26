using System;
using System.Collections.Generic;

namespace ThoughtEcho.Entities;

public partial class EmailVerificationOtp
{
    public string UserEmail { get; set; } = null!;

    public int Otp { get; set; }

    public DateTime Expires { get; set; }

    public virtual UserCred UserEmailNavigation { get; set; } = null!;
}
