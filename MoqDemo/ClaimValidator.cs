using System.Collections.Generic;

namespace MoqDemo
{
    public class ClaimValidator : IClaimValidator
    {
        public List<string> ValidateClaim(ClaimInput claim)
        {
            var result = new List<string>();
            if (!(claim.ServiceStartDate <= claim.ServiceEndDate))
            {
                result.Add("Claim Error: Service Date Not Valid");
            }

            return result;

        }
    }
}