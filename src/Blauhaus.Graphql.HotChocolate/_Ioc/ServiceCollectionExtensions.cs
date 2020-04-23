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
        public static IServiceCollection AddAuthenticatedUserCommandHandler<TPayload, TCommand, TCommandHandler>(this IServiceCollection services) 
            where TCommandHandler : class, IAuthenticatedUserCommandHandler<TPayload, TCommand>
        {
            services.AddScoped<ICommandServerHandler<TPayload, TCommand, IAuthenticatedUser>, TCommandHandler>();
            services.AddScoped<IAuthenticatedUserCommandHandler<TPayload, TCommand>, TCommandHandler>();
            return services;
        }

        public static IServiceCollection AddCommandServerHandler<TPayload, TCommand, TUser, TCommandHandler>(this IServiceCollection services) 
            where TCommandHandler : class, ICommandServerHandler<TPayload, TCommand, TUser>
        {
            services.AddScoped<ICommandServerHandler<TPayload, TCommand, TUser>, TCommandHandler>();
            return services;
        }
    }
}