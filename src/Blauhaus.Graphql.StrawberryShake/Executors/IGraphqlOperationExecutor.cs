using System.Threading;
using System.Threading.Tasks;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.Executors
{
    public interface IGraphqlOperationExecutor<in TOperation, TResult>
        where TOperation  : IOperation<TResult>
        where TResult : class
    {
        Task<IOperationResult<TResult>> ExecuteAsync(TOperation operation, CancellationToken token);
    }
}