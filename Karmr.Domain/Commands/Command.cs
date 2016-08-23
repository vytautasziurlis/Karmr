using Karmr.Contracts.Commands;

namespace Karmr.Domain.Commands
{
    public abstract class Command : ICommand
    {
        public long Sequence { get; }

        public Command()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}: ", nameof(this.GetType));
        }
    }
}