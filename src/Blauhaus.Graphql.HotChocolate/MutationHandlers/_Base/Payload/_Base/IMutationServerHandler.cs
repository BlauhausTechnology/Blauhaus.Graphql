using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload._Base
{
    public interface IMutationServerHandler
    {
        Task<TPayload> HandleAsync<TPayload, TCommand>(IResolverContext context, CancellationToken token);
    }
}