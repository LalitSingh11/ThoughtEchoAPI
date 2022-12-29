using System.ComponentModel.DataAnnotations;

namespace HRSquared.Entities;

public partial class RefreshToken
{
    [Key]
    public int UserId { get; set; }

    public string Token { get; set; }

    public DateTime? Expires { get; set; }

    public virtual UserCred User { get; set; } = null!;
}
