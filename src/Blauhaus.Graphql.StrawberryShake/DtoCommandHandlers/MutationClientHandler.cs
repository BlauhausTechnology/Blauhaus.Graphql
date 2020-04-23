using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.ValueObjects.Extensions;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.DtoCommandHandlers
{
    public class MutationClientHandler<TResponseDto, TGraphqlResponse, TCommandDto> : ICommandHandler<TResponseDto, TCommandDto>
        where TResponseDto : class 
        where TGraphqlResponse : class
    {
        private readonly IGraphqlClient<TGraphqlResponse, TCommandDto> _graphqlClient;
        private readonly IOperationResultConverter<TResponseDto, TGraphqlResponse> _operationResultConverter;

        public MutationClientHandler(
            IGraphqlClient<TGraphqlResponse, TCommandDto> graphqlClient,
            IOperationResultConverter<TResponseDto, TGraphqlResponse> operationResultConverter)
        {
            _graphqlClient = graphqlClient;
            _operationResultConverter = operationResultConverter;
        }

        public async Task<Result<TResponseDto>> HandleAsync(TCommandDto commandInput, CancellationToken token)
        {
            var result = await _graphqlClient.GetResultAsync(commandInput, token);
            var error = result.Errors.FirstOrDefault();
            if (error == null)
            {
                return _operationResultConverter.Convert(result);
            }

            if (error.Exception == null)
            {
                if (error.Message.IsError())
                {
                    return Result.Failure<TResponseDto>(error.Message);
                }
                throw new GraphqlException(error);
            }

            throw error.Exception;
        }

    }
}