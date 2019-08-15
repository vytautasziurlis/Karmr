namespace Karmr.Common.Contracts
{
    public interface ICommandHandler
    {
        void Handle(ICommand command);
    }
}