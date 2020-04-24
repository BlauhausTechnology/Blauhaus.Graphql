using Blauhaus.Graphql.HotChocolate.MutationHandlers;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload._Base;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Void;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Void._Base;
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

        public static IServiceCollection AddVoidMutationServerHandler<TCommand>(this IServiceCollection services)
        {
            services.TryAddScoped<IVoidMutationServerHandler, VoidMutationServerHandler>();
            return services;
        } 
    }
}