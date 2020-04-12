using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Blauhaus.Graphql.StrawberryShake.MutationExecutors
{
    public interface IMutationExecutor<TModelDto, in TCommandDto>
    {
        Task<Result<TModelDto>> ExecuteAsync(TCommandDto commandInput, CancellationToken token);
    }
}