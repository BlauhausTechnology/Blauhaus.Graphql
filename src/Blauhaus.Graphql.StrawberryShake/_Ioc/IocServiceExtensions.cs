using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Client;
using Blauhaus.Common.Domain.Entities;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers.Payload;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers.Void;
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
            where TMutationClient : class, IMutationClient<TModelDto, TMutationResponse, TCommandDto, TCommand>
            where TModelDto : class
        {
            services.RegisterImplementation<ICommandHandler<TModel, TCommand>, EntityCommandClientHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.RegisterImplementation<ICommandHandler<TModelDto, TCommandDto>, MutationClientHandler<TModelDto, TMutationResponse, TCommandDto, TCommand>>();
            services.RegisterImplementation<IMutationClient<TModelDto, TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.RegisterImplementation<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }

        public static IIocService AddVoidMutationClientHandler<TMutationResponse, TCommandDto, TCommand, TMutationClient> 
            (this IIocService services) 
            where TMutationResponse : class
            where TMutationClient : class, IVoidMutationClient<TMutationResponse, TCommandDto, TCommand>
        {
            services.RegisterImplementation<IVoidCommandHandler<TCommand>, VoidCommandClientHandler<TCommandDto, TCommand>>();
            services.RegisterImplementation<IVoidCommandHandler<TCommandDto>, VoidMutationClientHandler<TMutationResponse, TCommandDto, TCommand>>();
            services.RegisterImplementation<IVoidMutationClient<TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.RegisterImplementation<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}