﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service; 
using Blauhaus.Domain.Common.CommandHandlers;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload._Base;
using CSharpFunctionalExtensions;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload
{
    public class AnonymousServerQueryHandler : BaseServerQueryHandler
    { 
        public AnonymousServerQueryHandler(IAnalyticsService analyticsService) : base(analyticsService)
        {
        }

        protected override Task<Result<TPayload>> HandleCommandAsync<TPayload, TCommand>(IResolverContext context, TCommand command, CancellationToken token)
        {
            var commandHandler = context.Service<ICommandHandler<TPayload, TCommand>>();
            if (commandHandler == null)
            {
                throw new ArgumentException("No command handler found for command");
            }

            return commandHandler.HandleAsync(command, token);
        }

    }
}