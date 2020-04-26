using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Void._Base
{
    public interface IVoidMutationServerHandler
    {
        Task<bool> HandleAsync<TCommand>(IResolverContext context, CancellationToken token);
    }
}