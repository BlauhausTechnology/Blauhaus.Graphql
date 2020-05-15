using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Domain.Client.CommandHandlers;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload
{
    public interface IQueryConverter<TModelDto, TMutationResponse, TCommandDto, TCommand> : ICommandConverter<TCommandDto, TCommand>
        where TModelDto : class
        where TMutationResponse : class
    {

        Task<IOperationResult<TMutationResponse>> GetResultAsync(TCommandDto commandDto, CancellationToken token);
        TModelDto? GetDtoFromResult(IOperationResult<TMutationResponse> operationResult);
    }
}