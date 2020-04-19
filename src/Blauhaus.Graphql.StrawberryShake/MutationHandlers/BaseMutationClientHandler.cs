﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.ValueObjects.Extensions;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.MutationHandlers
{
    public abstract class BaseMutationClientHandler<TModelTDto, TGraphqlResponse, TCommandDto> : IMutationClientHandler<TModelTDto, TCommandDto>
        where TModelTDto : class 
        where TGraphqlResponse : class
    {
        
        public async Task<Result<TModelTDto>> ExecuteAsync(TCommandDto commandInput, CancellationToken token)
        {
            var result = await GetResultAsync(commandInput, token);
            var error = result.Errors.FirstOrDefault();
            if (error == null)
            {
                return ExtractDto(result);
            }

            if (error.Exception == null)
            {
                if (error.Message.IsError())
                {
                    return Result.Failure<TModelTDto>(error.Message);
                }
                throw new GraphqlException(error);
            }

            throw error.Exception;
        }

        protected abstract Task<IOperationResult<TGraphqlResponse>> GetResultAsync(TCommandDto commandDto, CancellationToken token);
        protected abstract Result<TModelTDto> ExtractDto(IOperationResult<TGraphqlResponse> operationResult);
    }
}