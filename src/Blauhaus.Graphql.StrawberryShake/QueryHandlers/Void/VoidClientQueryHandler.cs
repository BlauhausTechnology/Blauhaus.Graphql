using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Errors.Extensions;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using Blauhaus.Responses;

namespace Blauhaus.Graphql.StrawberryShake.QueryHandlers.Void
{
    public class VoidClientQueryHandler<TMutationResult, TCommandDto, TCommand> : IVoidCommandHandler<TCommandDto> 
        where TMutationResult : class
        where TCommandDto : notnull
    {
        private readonly IVoidQueryConverter<TMutationResult, TCommandDto, TCommand> _queryConverter;

        public VoidClientQueryHandler(IVoidQueryConverter<TMutationResult, TCommandDto, TCommand> queryConverter)
        {
            _queryConverter = queryConverter;
        }

        public async Task<Response> HandleAsync(TCommandDto commandInput, CancellationToken token)
        {
            var result = await _queryConverter.GetResultAsync(commandInput, token);
            var error = result.Errors.FirstOrDefault();
            if (error == null)
            {
                return Response.Success();
            }

            if (error.Exception == null)
            {
                if (error.Message.IsError())
                {
                    return Response.Failure(error.Message);
                }
                throw new GraphqlException(error);
            }

            throw error.Exception;
        }

    }
}