using System;
using System.Collections.Generic;
using System.Linq;

namespace MoqDemo
{
    public class ClaimCalculator
    {
        private readonly IPlanGetter _planGetter;

        public ClaimCalculator(IPlanGetter planGetter)
        {
            _planGetter = planGetter;
        }

        public Claim Calculate(Claim c, int participantId)
        {
            if (!ValidateClaim(c))
            {
                return c;
            }

            var coverages = _planGetter.GetCoverages(participantId);
            if (!coverages.Any())
            {
                c.Log.Add("No Coverage");
                return c;
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
                c.Log.Add("No Coverage (Outside Date Range)");
            }
            else
            {
                 c.Log.Add("Coverage Found");
            }

            return c;
        }

        private bool ValidateClaim(Claim claim)
        {
            if (!(claim.ServiceStartDate <= claim.ServiceEndDate))
            {
                claim.Log.Add("Claim Error: Service Date Not Valid");
                return false;
            }

            return true;

        }
    }

    public class Claim
    {
        public long Id { get; set; }
        public List<string> Log { get; set; } = new List<string>();

        public DateTime ServiceStartDate { get; set; }
        public DateTime ServiceEndDate { get; set; }
    }
}