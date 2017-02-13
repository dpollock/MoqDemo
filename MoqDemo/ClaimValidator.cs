namespace MoqDemo
{
    public class ClaimValidator : IClaimValidator
    {
        public bool ValidateClaim(Claim claim)
        {
            if (!(claim.ServiceStartDate <= claim.ServiceEndDate))
            {
                claim.Log.Add("Claim Error: Service Date Not Valid");
                return false;
            }

            return true;

        }
    }
}