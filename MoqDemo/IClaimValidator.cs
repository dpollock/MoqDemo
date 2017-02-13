using System.Collections.Generic;

namespace MoqDemo
{
    public interface IClaimValidator
    {
        List<string>  ValidateClaim(ClaimInput claim);
    }
}