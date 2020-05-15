using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.ValueObjects.Extensions;
using Blauhaus.Domain.Common.CommandHandlers;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using CSharpFunctionalExtensions;

namespace Blauhaus.Graphql.StrawberryShake.QueryHandlers.Void
{
    public class VoidClientQueryHandler<TMutationResult, TCommandDto, TCommand> : IVoidCommandHandler<TCommandDto> where TMutationResult : class
    {
        private readonly IVoidQueryConverter<TMutationResult, TCommandDto, TCommand> _mutationClient;

        public VoidClientQueryHandler(IVoidQueryConverter<TMutationResult, TCommandDto, TCommand> mutationClient)
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