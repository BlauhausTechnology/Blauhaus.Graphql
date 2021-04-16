using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload._Base
{
    public interface IServerQueryHandler
    {
        Task<TPayload> HandleAsync<TPayload, TCommand>(IResolverContext context, CancellationToken token) where TCommand : notnull;
    }
}