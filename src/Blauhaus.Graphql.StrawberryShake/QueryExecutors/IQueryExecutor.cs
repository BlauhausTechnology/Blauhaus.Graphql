using System.Threading;
using System.Threading.Tasks;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.QueryExecutors
{
    public interface IQueryExecutor<in TOperation, TPayload>
        where TOperation  : IOperation<TPayload>
        where TPayload : class
    {
        Task<TPayload> ExecuteAsync(TOperation operation, CancellationToken token);
        
    }
}