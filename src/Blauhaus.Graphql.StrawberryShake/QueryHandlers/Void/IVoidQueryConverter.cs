using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Domain.Client.CommandHandlers;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.QueryHandlers.Void
{
    public interface IVoidQueryConverter<TMutationResponse, TCommandDto, TCommand> : ICommandConverter<TCommandDto, TCommand>
        where TMutationResponse : class
    {
        Task<IOperationResult<TMutationResponse>> GetResultAsync(TCommandDto commandDto);
    }
}