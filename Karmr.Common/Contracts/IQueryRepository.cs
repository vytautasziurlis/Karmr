using System.Collections.Generic;

namespace Karmr.Common.Contracts
{
    public interface IQueryRepository
    {
        T QuerySingle<T>(string query);

        T QuerySingle<T>(string query, object @params);

        IEnumerable<T> Query<T>(string query);

        IEnumerable<T> Query<T>(string query, object @params);
    }
}