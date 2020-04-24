using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Domain.CommandHandlers;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.MutationClientHandlers.Void
{
    public interface IVoidMutationClient<TMutationResponse, TCommandDto, TCommand> : ICommandConverter<TCommandDto, TCommand>
        where TMutationResponse : class
    {
        Task<IOperationResult<TMutationResponse>> GetResultAsync(TCommandDto commandDto, CancellationToken token);
    }
}