﻿using Blauhaus.Common.Domain.CommandHandlers;
using Blauhaus.Common.Domain.CommandHandlers.Client;
using Blauhaus.Common.Domain.Entities;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientEntityCommandHandler<
            TModel, TModelDto, TMutationResponse, TCommandDto, TCommand, TMutationClient> 
                (this IServiceCollection services) 
            where TModel : class, IClientEntity 
            where TMutationResponse : class
            where TMutationClient : class, IMutationClient<TModelDto, TMutationResponse, TCommandDto, TCommand>
            where TModelDto : class
        {
            services.AddTransient<ICommandClientHandler<TModel, TCommand>, ClientEntityCommandHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.AddTransient<ICommandClientHandler<TModelDto, TCommandDto>, MutationClientHandler<TModelDto, TMutationResponse, TCommandDto, TCommand>>();
            services.AddTransient<IMutationClient<TModelDto, TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.AddTransient<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}