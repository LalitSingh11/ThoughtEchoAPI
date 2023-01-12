using System;
using System.Collections.Generic;

namespace ThoughtEcho.Entities;

public partial class RefreshToken
{
    public int UserId { get; set; }

    public string? Token { get; set; }

    public DateTime Expires { get; set; }

    public virtual UserCred User { get; set; } = null!;
}
