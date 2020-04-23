using Blauhaus.Common.Domain.CommandHandlers.Client;
using Blauhaus.Common.Domain.Entities;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class IocServiceExtensions
    {
        public static IIocService AddClientEntityCommandHandler<
                TModel, TModelDto, TGraphqlResponse, TCommandDto, TCommand, TMutationClient> 
            (this IIocService services) 
            where TModel : class, IClientEntity 
            where TGraphqlResponse : class
            where TMutationClient : class, IMutationClient<TModelDto, TGraphqlResponse, TCommandDto, TCommand>
            where TModelDto : class
        {
            services.RegisterImplementation<ICommandClientHandler<TModel, TCommand>, ClientEntityCommandHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.RegisterImplementation<IMutationClientHandler<TModelDto, TCommandDto>, MutationClientHandler<TModelDto, TGraphqlResponse, TCommandDto, TCommand>>();
            services.RegisterImplementation<IMutationClient<TModelDto, TGraphqlResponse, TCommandDto, TCommand>, TMutationClient>();
            services.RegisterImplementation<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}