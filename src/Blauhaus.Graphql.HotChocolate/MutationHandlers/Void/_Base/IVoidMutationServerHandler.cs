using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers.Void._Base
{
    public interface IVoidMutationServerHandler
    {
        Task<bool> HandleAsync<TCommand>(IResolverContext context, CancellationToken token);
    }
}