using System.Threading;
using System.Threading.Tasks;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.DtoCommandHandlers
{
    public interface IGraphqlClient<TGraphqlResponse, TCommandDto> where TGraphqlResponse : class
    {
        Task<IOperationResult<TGraphqlResponse>> GetResultAsync(TCommandDto commandDto, CancellationToken token);
    }
}