namespace Karmr.Contracts.Commands
{
    public interface ICommand
    {
        long Sequence { get; }
    }
}