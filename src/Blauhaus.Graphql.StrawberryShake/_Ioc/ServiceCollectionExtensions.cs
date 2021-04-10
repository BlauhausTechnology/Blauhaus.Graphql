using System.Net.Http;
using Blauhaus.Auth.Abstractions.Ioc;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Domain.Abstractions.Entities;
using Blauhaus.Domain.Abstractions.Sync;
using Blauhaus.Domain.Client.CommandHandlers;
using Blauhaus.Domain.Client.Sync.CommandHandler;
using Blauhaus.Graphql.StrawberryShake.Config;
using Blauhaus.Graphql.StrawberryShake.HttpClients;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Void;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddHttpClientHolder<TConfig>(this IServiceCollection services) where TConfig : class, IGraphqlClientConfig
        {
            
            services.RegisterAccessToken();
            services.AddScoped<IGraphqlClientConfig, TConfig>();
            services.AddSingleton<IHttpClientFactory, HttpClientHolder>();

            return services;
        }

        
        public static IServiceCollection AddSyncClientQueryHandler<TModelDto, TMutationResponse, TSyncCommandDto, TSyncCommand, TQueryConverter>(this IServiceCollection services)
            where TMutationResponse : class
            where TQueryConverter : class, IQueryConverter<DtoSyncResult<TModelDto>, TMutationResponse, TSyncCommandDto, TSyncCommand>
            where TModelDto : class
            where TSyncCommand : SyncCommand
            where TSyncCommandDto : notnull
        { 
            services.AddTransient<ICommandConverter<TSyncCommandDto, TSyncCommand>, TQueryConverter>();
            services.AddTransient<IQueryConverter<DtoSyncResult<TModelDto>, TMutationResponse, TSyncCommandDto, TSyncCommand>, TQueryConverter>();
            services.AddTransient<ICommandHandler<DtoSyncResult<TModelDto>, TSyncCommandDto>, ClientQueryHandler<DtoSyncResult<TModelDto>, TMutationResponse, TSyncCommandDto, TSyncCommand>>();

            return services;
        }

        public static IServiceCollection AddEntityClientQueryHandler<
            TModel, TModelDto, TMutationResponse, TCommandDto, TCommand, TMutationClient> 
                (this IServiceCollection services) 
            where TModel : class, IClientEntity 
            where TMutationResponse : class
            where TMutationClient : class, IQueryConverter<TModelDto, TMutationResponse, TCommandDto, TCommand>
            where TModelDto : class
            where TCommand : notnull
            where TCommandDto : notnull
        {
            services.AddScoped<ICommandHandler<TModel, TCommand>, EntityCommandClientHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.AddScoped<ICommandHandler<TModelDto, TCommandDto>, ClientQueryHandler<TModelDto, TMutationResponse, TCommandDto, TCommand>>();
            services.AddTransient<IQueryConverter<TModelDto, TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.AddTransient<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }

        public static IServiceCollection AddVoidClientQueryHandler<TMutationResponse, TCommandDto, TCommand, TMutationClient> 
            (this IServiceCollection services) 
            where TMutationResponse : class
            where TMutationClient : class, IVoidQueryConverter<TMutationResponse, TCommandDto, TCommand>
            where TCommand : notnull
            where TCommandDto : notnull
        {
            services.AddScoped<IVoidCommandHandler<TCommand>, VoidCommandClientHandler<TCommandDto, TCommand>>();
            services.AddScoped<IVoidCommandHandler<TCommandDto>, VoidClientQueryHandler<TMutationResponse, TCommandDto, TCommand>>();
            services.AddTransient<IVoidQueryConverter<TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.AddTransient<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}