using HotChocolate.Resolvers;
using System.Threading;
using System.Threading.Tasks;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers
{
    public interface IMutationServerHandler
    {
        Task<TPayload> HandleAsync<TPayload, TCommand>(IResolverContext context, CancellationToken token);
    }
}