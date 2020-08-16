using Blauhaus.Auth.Server.Azure._Ioc;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Void;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.Graphql.HotChocolate._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServerQueryHandlers(this IServiceCollection services)
        {

            services.AddAzureUserFactory();

            services.TryAddScoped<AuthenticatedUserServerQueryHandler>();
            services.TryAddScoped<VoidAuthenticatedUserServerQueryHandler>();
            services.TryAddScoped<AnonymousServerQueryHandler>();
            services.TryAddScoped<VoidAnonymousServerQueryHandler>();
            return services;
        } 

    }
}