using System;
using System.Threading;
using System.Threading.Tasks;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.QueryExecutors
{
    [Obsolete]
    public interface IQueryExecutor<in TOperation, TPayload>
        where TOperation  : IOperationExecutor<TPayload>
        where TPayload : class
    {
        Task<TPayload> ExecuteAsync(TOperation operation, CancellationToken token);
        
    }
}