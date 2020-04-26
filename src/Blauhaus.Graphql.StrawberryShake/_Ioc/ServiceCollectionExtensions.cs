using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Client;
using Blauhaus.Common.Domain.Entities;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers.Payload;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers.Void;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityMutationClientHandler<
            TModel, TModelDto, TMutationResponse, TCommandDto, TCommand, TMutationClient> 
                (this IServiceCollection services) 
            where TModel : class, IClientEntity 
            where TMutationResponse : class
            where TMutationClient : class, IMutationClient<TModelDto, TMutationResponse, TCommandDto, TCommand>
            where TModelDto : class
        {
            services.AddTransient<ICommandHandler<TModel, TCommand>, EntityCommandClientHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.AddTransient<ICommandHandler<TModelDto, TCommandDto>, MutationClientHandler<TModelDto, TMutationResponse, TCommandDto, TCommand>>();
            services.AddTransient<IMutationClient<TModelDto, TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.AddTransient<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }

        public static IServiceCollection AddVoidMutationClientHandler<TMutationResponse, TCommandDto, TCommand, TMutationClient> 
            (this IServiceCollection services) 
            where TMutationResponse : class
            where TMutationClient : class, IVoidMutationClient<TMutationResponse, TCommandDto, TCommand>
        {
            services.AddTransient<IVoidCommandHandler<TCommand>, VoidCommandClientHandler<TCommandDto, TCommand>>();
            services.AddTransient<IVoidCommandHandler<TCommandDto>, VoidMutationClientHandler<TMutationResponse, TCommandDto, TCommand>>();
            services.AddTransient<IVoidMutationClient<TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.AddTransient<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}