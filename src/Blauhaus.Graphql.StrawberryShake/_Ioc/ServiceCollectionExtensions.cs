using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Client;
using Blauhaus.Common.Domain.Entities;
using Blauhaus.Graphql.StrawberryShake.DtoCommandHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientEntityCommandHandler<
            TModel, TModelDto, TGraphqlResponse, TCommandDto, TCommand, TGraphqlClient> 
                (this IServiceCollection services) 
            where TModel : class, IClientEntity 
            where TGraphqlResponse : class
            where TGraphqlClient : class, IGraphqlClient<TModelDto, TGraphqlResponse, TCommandDto, TCommand>
            where TModelDto : class
        {
            services.AddTransient<ICommandClientHandler<TModel, TCommand>, ClientEntityCommandHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.AddTransient<ICommandConverter<TCommandDto, TCommand>, TGraphqlClient>();
            services.AddTransient<ICommandHandler<TModelDto, TCommandDto>, MutationClientHandler<TModelDto, TGraphqlResponse, TCommandDto, TCommand>>();
            services.AddTransient<IGraphqlClient<TModelDto, TGraphqlResponse, TCommandDto, TCommand>, TGraphqlClient>();
            return services;
        }
    }
}