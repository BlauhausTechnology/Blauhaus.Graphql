using System.Net.Http;
using Blauhaus.Auth.Abstractions._Ioc;
using Blauhaus.Domain.Client.CommandHandlers;
using Blauhaus.Domain.Common.CommandHandlers;
using Blauhaus.Domain.Common.Entities;
using Blauhaus.Graphql.StrawberryShake.Config;
using Blauhaus.Graphql.StrawberryShake.HttpClients;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Void;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class IocServiceExtensions
    {
        public static IIocService AddHttpClientHolder<TConfig>(this IIocService iocService) where TConfig : class, IGraphqlClientConfig
        {
            
            iocService.RegisterAccessToken();
            iocService.RegisterImplementation<IGraphqlClientConfig, TConfig>(IocLifetime.Singleton);
            iocService.RegisterImplementation<IHttpClientFactory, HttpClientHolder>(IocLifetime.Singleton);

            return iocService;
        }

        public static IIocService AddEntityMutationClientHandler<
            TModel, TModelDto, TMutationResponse, TCommandDto, TCommand, TMutationClient> 
                (this IIocService services) 
            where TModel : class, IClientEntity 
            where TMutationResponse : class
            where TMutationClient : class, IQueryConverter<TModelDto, TMutationResponse, TCommandDto, TCommand>
            where TModelDto : class
            where TCommand : notnull
            where TCommandDto : notnull
        {
            services.RegisterImplementation<ICommandHandler<TModel, TCommand>, EntityCommandClientHandler<TModel, TModelDto, TCommandDto, TCommand>>(IocLifetime.Singleton);
            services.RegisterImplementation<ICommandHandler<TModelDto, TCommandDto>, ClientQueryHandler<TModelDto, TMutationResponse, TCommandDto, TCommand>>(IocLifetime.Singleton);
            services.RegisterImplementation<IQueryConverter<TModelDto, TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.RegisterImplementation<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }

        public static IIocService AddVoidMutationClientHandler<TMutationResponse, TCommandDto, TCommand, TMutationClient> 
            (this IIocService services) 
            where TMutationResponse : class
            where TMutationClient : class, IVoidQueryConverter<TMutationResponse, TCommandDto, TCommand>
            where TCommand : notnull
            where TCommandDto : notnull
        {
            services.RegisterImplementation<IVoidCommandHandler<TCommand>, VoidCommandClientHandler<TCommandDto, TCommand>>(IocLifetime.Singleton);
            services.RegisterImplementation<IVoidCommandHandler<TCommandDto>, VoidClientQueryHandler<TMutationResponse, TCommandDto, TCommand>>(IocLifetime.Singleton);
            services.RegisterImplementation<IVoidQueryConverter<TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.RegisterImplementation<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}