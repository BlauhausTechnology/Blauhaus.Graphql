using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Void._Base
{
    public interface IVoidServerQueryHandler
    {
        Task<bool> HandleAsync<TCommand>(IResolverContext context, CancellationToken token);
    }
}