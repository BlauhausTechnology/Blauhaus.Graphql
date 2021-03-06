﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Errors.Extensions;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using CSharpFunctionalExtensions;

namespace Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload
{
    public class ClientQueryHandler<TResultDto, TMutationResult, TCommandDto, TCommand> : ICommandHandler<TResultDto, TCommandDto>
        where TResultDto : class 
        where TMutationResult : class
        where TCommandDto : notnull
    {
        private readonly IQueryConverter<TResultDto, TMutationResult, TCommandDto, TCommand> _graphqlClient;

        public ClientQueryHandler(
            IQueryConverter<TResultDto, TMutationResult, TCommandDto, TCommand> graphqlClient)
        {
            _graphqlClient = graphqlClient;
        }

        public async Task<Result<TResultDto>> HandleAsync(TCommandDto commandInput, CancellationToken token)
        {
            var result = await _graphqlClient.GetResultAsync(commandInput, token);
            var error = result.Errors.FirstOrDefault();
            if (error == null)
            {
                //todo fail if null?
                return Result.Success(_graphqlClient.GetDtoFromResult(result));
            }

            if (error.Exception == null)
            {
                if (error.Message.IsError())
                {
                    return Result.Failure<TResultDto>(error.Message);
                }
                throw new GraphqlException(error);
            }

            throw error.Exception;
        }

    }
}