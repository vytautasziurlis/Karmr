using System.Collections.Generic;

namespace Karmr.Common.Contracts
{
    public interface IDenormalizerHandler
    {
        void Handle(IEnumerable<IEvent> events);
    }
}