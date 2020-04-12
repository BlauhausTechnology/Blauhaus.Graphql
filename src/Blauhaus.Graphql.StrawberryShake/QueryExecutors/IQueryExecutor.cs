using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.Executors
{
    public interface IQueryExecutor<in TOperation, TPayload>
        where TOperation  : IOperation<TPayload>
        where TPayload : class
    {
        Task<TPayload> ExecuteAsync(TOperation operation, CancellationToken token);
        
    }
}