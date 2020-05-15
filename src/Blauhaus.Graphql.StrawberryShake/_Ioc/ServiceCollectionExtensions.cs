﻿using Blauhaus.Domain.Client.CommandHandlers;
using Blauhaus.Domain.Common.CommandHandlers;
using Blauhaus.Domain.Common.Entities;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Payload;
using Blauhaus.Graphql.StrawberryShake.QueryHandlers.Void;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.Graphql.StrawberryShake._Ioc
{
    public static class ServiceCollectionExtensions
    {
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
            services.AddTransient<ICommandHandler<TModel, TCommand>, EntityCommandClientHandler<TModel, TModelDto, TCommandDto, TCommand>>();
            services.AddTransient<ICommandHandler<TModelDto, TCommandDto>, ClientQueryHandler<TModelDto, TMutationResponse, TCommandDto, TCommand>>();
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
            services.AddTransient<IVoidCommandHandler<TCommand>, VoidCommandClientHandler<TCommandDto, TCommand>>();
            services.AddTransient<IVoidCommandHandler<TCommandDto>, VoidClientQueryHandler<TMutationResponse, TCommandDto, TCommand>>();
            services.AddTransient<IVoidQueryConverter<TMutationResponse, TCommandDto, TCommand>, TMutationClient>();
            services.AddTransient<ICommandConverter<TCommandDto, TCommand>, TMutationClient>();

            return services;
        }
    }
}