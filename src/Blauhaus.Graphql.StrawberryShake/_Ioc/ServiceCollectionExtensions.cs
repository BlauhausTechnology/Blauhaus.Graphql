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
            TModel, TModelDto, TCommandDto, TCommand, TCommandConverter, 
            TGraphqlClient, TGraphqlResponse, TOperationResultConverter>
                (this IServiceCollection services) 
            where TModel : class, IClientEntity 
            where TCommandConverter : class, ICommandConverter<TCommandDto, TCommand>
            where TGraphqlResponse : class
            where TGraphqlClient : class, IGraphqlClient<TGraphqlResponse, TCommandDto>
            where TOperationResultConverter : class, IOperationResultConverter<TModelDto, TGraphqlResponse>
            where TModelDto : class
        {
            services.AddTransient<ICommandClientHandler<TModel, TCommand>, ClientEntityCommandHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.AddTransient<ICommandConverter<TCommandDto, TCommand>, TCommandConverter>();
            services.AddTransient<ICommandHandler<TModelDto, TCommandDto>, MutationClientHandler<TModelDto, TGraphqlResponse, TCommandDto>>();
            services.AddTransient<IGraphqlClient<TGraphqlResponse, TCommandDto>, TGraphqlClient>();
            services.AddTransient<IOperationResultConverter<TModelDto, TGraphqlResponse>, TOperationResultConverter>();
            return services;
        }
    }
}