using System.Threading;
using System.Threading.Tasks;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.Executors
{
    public interface IGraphqlOperationExecutor<TOperationResult, in TOperation, TResult>
        where TOperationResult : class, IOperationResult<TResult>
        where TOperation  : IOperation<TResult>
        where TResult : class
    {
        Task<TOperationResult> ExecuteAsync(TOperation operation, CancellationToken token);
    }
}