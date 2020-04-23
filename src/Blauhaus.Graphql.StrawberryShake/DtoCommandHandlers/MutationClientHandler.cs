﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.ValueObjects.Extensions;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.DtoCommandHandlers
{
    public class MutationClientHandler<TModelDto, TGraphqlResponse, TCommandDto, TCommand> : ICommandHandler<TModelDto, TCommandDto>
        where TModelDto : class 
        where TGraphqlResponse : class
    {
        private readonly IGraphqlClient<TModelDto, TGraphqlResponse, TCommandDto, TCommand> _graphqlClient;

        public MutationClientHandler(
            IGraphqlClient<TModelDto, TGraphqlResponse, TCommandDto, TCommand> graphqlClient)
        {
            _graphqlClient = graphqlClient;
        }

        public async Task<Result<TModelDto>> HandleAsync(TCommandDto commandInput, CancellationToken token)
        {
            var result = await _graphqlClient.GetResultAsync(commandInput, token);
            var error = result.Errors.FirstOrDefault();
            if (error == null)
            {
                return _graphqlClient.Convert(result);
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