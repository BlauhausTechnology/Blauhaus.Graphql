using Blauhaus.Graphql.HotChocolate.MutationHandlers.Payload;
using Blauhaus.Graphql.HotChocolate.MutationHandlers.Void;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.Graphql.HotChocolate._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMutationServerHandlers(this IServiceCollection services)
        {
            services.TryAddScoped<AuthenticatedUserMutationServerHandler>();
            services.TryAddScoped<VoidAuthenticatedUserMutationServerHandler>();
            services.TryAddScoped<MutationServerHandler>();
            services.TryAddScoped<VoidMutationServerHandler>();
            return services;
        } 

    }
}