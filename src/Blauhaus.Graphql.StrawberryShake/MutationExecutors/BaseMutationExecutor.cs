using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.Executors;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.MutationExecutors
{
    public abstract class BaseMutationExecutor<TOperation, TPayload> : IMutationExecutor<TOperation, TPayload>
        where TOperation  : IOperation<TPayload>
        where TPayload : class 
    {

        public async Task<Result<TPayload>> ExecuteAsync(TOperation operation, CancellationToken token)
        {
            var result = await GetResultAsync(operation, token);
            var error = result.Errors.FirstOrDefault();
            if (error == null)
            {
                return Result.Success(result.Data);
            }

            if (error.Exception == null)
            {
                return Result.Failure<TPayload>(error.Message);
            }

            throw error.Exception;
        }

        protected abstract Task<IOperationResult<TPayload>> GetResultAsync(TOperation operation, CancellationToken token);
    }
}