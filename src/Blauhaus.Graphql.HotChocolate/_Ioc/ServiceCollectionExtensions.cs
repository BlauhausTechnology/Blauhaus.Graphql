using Blauhaus.Graphql.HotChocolate.MutationHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Graphql.HotChocolate._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticatedUserMutationHandler(this IServiceCollection services)
        {
            services.AddScoped<IMutationServerHandler, AuthenticatedUserMutationServerHandler>();
            return services;
        }
    }
}