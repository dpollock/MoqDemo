using System.Collections.Generic;

namespace MoqDemo
{
    public interface IPlanGetter
    {
        List<Coverage> GetCoverages(int participantId);
    }
}