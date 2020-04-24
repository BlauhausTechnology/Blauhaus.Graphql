using Blauhaus.Auth.Abstractions.CommandHandler;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Server;
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