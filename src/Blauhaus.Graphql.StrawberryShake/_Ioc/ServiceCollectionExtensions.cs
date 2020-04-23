using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Client;
using Blauhaus.Common.Domain.Entities;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientEntityCommandHandler<
            TModel, TModelDto, TGraphqlResponse, TCommandDto, TCommand, TMutationClient> 
                (this IServiceCollection services) 
            where TModel : class, IClientEntity 
            where TGraphqlResponse : class
            where TMutationClient : class, IMutationClient<TModelDto, TGraphqlResponse, TCommandDto, TCommand>
            where TModelDto : class
        {
            services.AddTransient<ICommandClientHandler<TModel, TCommand>, ClientEntityCommandHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.AddTransient<IMutationClientHandler<TModelDto, TCommandDto>, MutationClientHandler<TModelDto, TGraphqlResponse, TCommandDto, TCommand>>();
            services.AddTransient<IMutationClient<TModelDto, TGraphqlResponse, TCommandDto, TCommand>, TMutationClient>();
            services.AddTransient<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}