using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.Executors;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.MutationExecutors
{
    public abstract class BaseMutationExecutor<TDto, TResponse, TCommandInput> : IMutationExecutor<TDto, TCommandInput>
        where TDto : class 
        where TResponse : class
    {
        
        public async Task<Result<TDto>> ExecuteAsync(TCommandInput commandInput, CancellationToken token)
        {
            var result = await GetResultAsync(commandInput, token);
            var error = result.Errors.FirstOrDefault();
            if (error == null)
            {
                return ExtractDto(result);
            }

            if (error.Exception == null)
            {
                return Result.Failure<TDto>(error.Message);
            }

            throw error.Exception;
        }

        protected abstract Task<IOperationResult<TResponse>> GetResultAsync(TCommandInput operation, CancellationToken token);
        protected abstract Result<TDto> ExtractDto(IOperationResult<TResponse> operationResult);
    }
}