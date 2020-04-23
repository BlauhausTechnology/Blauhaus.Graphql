﻿using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Domain.CommandHandlers.Client;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.DtoCommandHandlers
{
    public interface IGraphqlClient<TModelDto, TGraphqlResponse, TCommandDto, TCommand> : ICommandConverter<TCommandDto, TCommand>
        where TGraphqlResponse : class
    {

        Task<IOperationResult<TGraphqlResponse>> GetResultAsync(TCommandDto commandDto, CancellationToken token);
        Result<TModelDto> Convert(IOperationResult<TGraphqlResponse> operationResult);
    }
}