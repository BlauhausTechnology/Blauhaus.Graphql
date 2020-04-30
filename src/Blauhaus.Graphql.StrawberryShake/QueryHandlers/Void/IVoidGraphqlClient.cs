using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Domain.CommandHandlers;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.QueryHandlers.Void
{
    public interface IVoidGraphqlClient<TMutationResponse, TCommandDto, TCommand> : ICommandConverter<TCommandDto, TCommand>
        where TMutationResponse : class
    {
        Task<IOperationResult<TMutationResponse>> GetResultAsync(TCommandDto commandDto, CancellationToken token);
    }
}