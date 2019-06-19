namespace Karmr.Common.Contracts
{
    public interface IDenormalizerRepository
    {
        void Execute(string query, object @params);
    }
}