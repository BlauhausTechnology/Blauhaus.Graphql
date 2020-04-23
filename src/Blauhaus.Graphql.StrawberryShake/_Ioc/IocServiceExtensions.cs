using Blauhaus.Common.Domain.CommandHandlers.Client;
using Blauhaus.Common.Domain.Entities;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class IocServiceExtensions
    {
        public static IIocService AddClientEntityCommandHandler<
                TModel, TModelDto, TMutationResponse, TMutationClient, TCommandDto, TCommand> 
            (this IIocService services) 
            where TModel : class, IClientEntity 
            where TModelDto : class
            where TMutationResponse : class
            where TMutationClient : class, IMutationClient<TModelDto, TMutationResponse, TCommandDto, TCommand>
        {
            services.RegisterImplementation<ICommandClientHandler<TModel, TCommand>, ClientEntityCommandHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.RegisterImplementation<IMutationClientHandler<TModelDto, TCommandDto>, MutationClientHandler<TModelDto, TMutationResponse, TCommandDto, TCommand>>();
            services.RegisterImplementation<IMutationClient<TModelDto, TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.RegisterImplementation<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}