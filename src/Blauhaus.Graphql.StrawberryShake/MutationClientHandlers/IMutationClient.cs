using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Domain.CommandHandlers.Client;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.MutationClientHandlers
{
    public interface IMutationClient<TModelDto, TMutationResponse, TCommandDto, TCommand> : ICommandConverter<TCommandDto, TCommand>
        where TModelDto : class
        where TMutationResponse : class
    {

        Task<IOperationResult<TMutationResponse>> GetResultAsync(TCommandDto commandDto, CancellationToken token);
        TModelDto? GetDtoFromResult(IOperationResult<TMutationResponse> operationResult);
    }
}