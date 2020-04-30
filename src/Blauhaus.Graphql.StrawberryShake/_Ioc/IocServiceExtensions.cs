using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Client;
using Blauhaus.Common.Domain.Entities;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Void;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class IocServiceExtensions
    {
        public static IIocService AddEntityMutationClientHandler<
            TModel, TModelDto, TMutationResponse, TCommandDto, TCommand, TMutationClient> 
                (this IIocService services) 
            where TModel : class, IClientEntity 
            where TMutationResponse : class
            where TMutationClient : class, IGraphqlClient<TModelDto, TMutationResponse, TCommandDto, TCommand>
            where TModelDto : class
        {
            services.RegisterImplementation<ICommandHandler<TModel, TCommand>, EntityCommandClientHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.RegisterImplementation<ICommandHandler<TModelDto, TCommandDto>, ClientQueryHandler<TModelDto, TMutationResponse, TCommandDto, TCommand>>();
            services.RegisterImplementation<IGraphqlClient<TModelDto, TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.RegisterImplementation<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }

        public static IIocService AddVoidMutationClientHandler<TMutationResponse, TCommandDto, TCommand, TMutationClient> 
            (this IIocService services) 
            where TMutationResponse : class
            where TMutationClient : class, IVoidGraphqlClient<TMutationResponse, TCommandDto, TCommand>
        {
            services.RegisterImplementation<IVoidCommandHandler<TCommand>, VoidCommandClientHandler<TCommandDto, TCommand>>();
            services.RegisterImplementation<IVoidCommandHandler<TCommandDto>, VoidClientQueryHandler<TMutationResponse, TCommandDto, TCommand>>();
            services.RegisterImplementation<IVoidGraphqlClient<TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.RegisterImplementation<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}