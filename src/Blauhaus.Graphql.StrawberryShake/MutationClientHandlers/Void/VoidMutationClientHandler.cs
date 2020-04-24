using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.ValueObjects.Extensions;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers.Payload;
using CSharpFunctionalExtensions;

namespace Blauhaus.Graphql.StrawberryShake.MutationClientHandlers.Void
{
    public class VoidMutationClientHandler<TMutationResult, TCommandDto, TCommand> : IVoidCommandHandler<TCommandDto> where TMutationResult : class
    {
        private readonly IVoidMutationClient<TMutationResult, TCommandDto, TCommand> _mutationClient;

        public VoidMutationClientHandler(IVoidMutationClient<TMutationResult, TCommandDto, TCommand> mutationClient)
        {
            _mutationClient = mutationClient;
        }

        public async Task<Result> HandleAsync(TCommandDto commandInput, CancellationToken token)
        {
            var result = await _mutationClient.GetResultAsync(commandInput, token);
            var error = result.Errors.FirstOrDefault();
            if (error == null)
            {
                return Result.Success();
            }

            if (error.Exception == null)
            {
                if (error.Message.IsError())
                {
                    return Result.Failure(error.Message);
                }
                throw new GraphqlException(error);
            }

            throw error.Exception;
        }

    }
}