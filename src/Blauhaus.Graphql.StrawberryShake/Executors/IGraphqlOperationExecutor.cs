using System.Threading;
using System.Threading.Tasks;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.Executors
{
    public interface IGraphqlOperationExecutor<in TOperation, TPayload>
        where TOperation  : IOperation<TPayload>
        where TPayload : class
    {
        Task<IOperationResult<TPayload>> ExecuteAsync(TOperation operation, CancellationToken token);
    }
}