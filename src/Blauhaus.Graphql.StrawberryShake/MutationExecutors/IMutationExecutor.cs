using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Blauhaus.Graphql.StrawberryShake.MutationExecutors
{
    public interface IMutationExecutor<TDto, in TCommandInput>
    {
        Task<Result<TDto>> ExecuteAsync(TCommandInput commandInput, CancellationToken token);
    }
}