using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Client;
using Blauhaus.Common.ValueObjects.Extensions;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using CSharpFunctionalExtensions;

namespace Blauhaus.Graphql.StrawberryShake.MutationClientHandlers
{
    public class MutationClientHandler<TModelDto, TMutationResult, TCommandDto, TCommand> : ICommandHandler<TModelDto, TCommandDto>
        where TModelDto : class 
        where TMutationResult : class
    {
        private readonly IMutationClient<TModelDto, TMutationResult, TCommandDto, TCommand> _graphqlClient;

        public MutationClientHandler(
            IMutationClient<TModelDto, TMutationResult, TCommandDto, TCommand> graphqlClient)
        {
            _graphqlClient = graphqlClient;
        }

        public async Task<Result<TModelDto>> HandleAsync(TCommandDto commandInput, CancellationToken token)
        {
            var result = await _graphqlClient.GetResultAsync(commandInput, token);
            var error = result.Errors.FirstOrDefault();
            if (error == null)
            {
                return Result.Success(_graphqlClient.GetDtoFromResult(result));
            }

            if (error.Exception == null)
            {
                if (error.Message.IsError())
                {
                    return Result.Failure<TModelDto>(error.Message);
                }
                throw new GraphqlException(error);
            }

            throw error.Exception;
        }

    }
}