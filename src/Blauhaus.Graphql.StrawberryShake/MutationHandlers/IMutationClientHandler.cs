using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Blauhaus.Graphql.StrawberryShake.MutationHandlers
{
    public interface IMutationClientHandler<TModelDto, in TCommandDto>
    {
        Task<Result<TModelDto>> ExecuteAsync(TCommandDto commandInput, CancellationToken token);
    }
}