using System;
using System.Collections.Generic;
using System.Linq;

namespace MoqDemo
{
    public class ClaimCalculator
    {
        private readonly IPlanGetter _planGetter;
        private readonly IClaimValidator _claimValidator;

        public ClaimCalculator(IPlanGetter planGetter, IClaimValidator claimValidator)
        {
            _planGetter = planGetter;
            _claimValidator = claimValidator;
        }

        public ClaimOutput Calculate(ClaimInput c, int participantId)
        {
            var output = new ClaimOutput() { ClaimId = c.Id, Log = new List<string>() };

            var validateResults = _claimValidator.ValidateClaim(c);
            if (validateResults.Any())
            {
                output.Log = validateResults;
                return output;
            }

            var coverages = _planGetter.GetCoverages(participantId);
            if (!coverages.Any())
            {
                output.Log.Add("No Coverage");
                return output;
            }

            var claimInCoverage = false;
            foreach (var coverage in coverages)
            {
                if (coverage.EffectiveDate <= c.ServiceStartDate) //&& coverage.EffectiveDate <= c.ServiceEndDate)
                {
                    claimInCoverage = true;
                }
            }

            if (claimInCoverage == false)
            {
                output.Log.Add("No Coverage (Outside Date Range)");
            }
            else
            {
                output.Log.Add("Coverage Found");
            }

            return output;
        }


    }


    public class ClaimOutput
    {
        public long ClaimId { get; set; }
        public List<string> Log { get; set; } = new List<string>();

    }

    public class ClaimInput
    {
        public long Id { get; set; }

        public DateTime ServiceStartDate { get; set; }
        public DateTime ServiceEndDate { get; set; }
        public decimal ApprovedAmount { get; set; }
    }
}