namespace MoqDemo
{
    public interface IClaimValidator
    {
        bool ValidateClaim(Claim claim);
    }
}