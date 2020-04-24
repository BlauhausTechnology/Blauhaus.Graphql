using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using CSharpFunctionalExtensions;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers._Base.Payload._Base
{
    public abstract class BaseMutationServerHandler : IMutationServerHandler
    {
        protected readonly IAnalyticsService AnalyticsService;

        protected BaseMutationServerHandler(
            IAnalyticsService analyticsService)
        {
            AnalyticsService = analyticsService;
        }

        public async Task<TPayload> HandleAsync<TPayload, TCommand>(IResolverContext context, CancellationToken token)
        {
            try
            {
                var httpContext = (HttpContext)context.ContextData[nameof(HttpContext)];

                var headers = new Dictionary<string, string>();
                foreach (var header in  httpContext.Request.Headers)
                {
                    headers.Add(header.Key, header.Value.FirstOrDefault());
                }

                using (var _ = AnalyticsService.StartRequestOperation(this, typeof(TCommand).Name, headers))
                {
                    var command = context.Argument<TCommand>("command");
                    if (command == null)
                    {
                        throw new ArgumentException("Unable to extract command from resolver context");
                    }

                    var commandResult = await HandleCommandAsync<TPayload, TCommand>(context, command, token);
                    if (commandResult.IsFailure)
                    {
                        
                        context.ReportError(new ErrorBuilder().SetMessage(commandResult.Error).Build());
                        return default;
                    }
                    
                    return commandResult.Value;


                }
            }
            catch (Exception e)
            {
                AnalyticsService.LogException(this, e);
                throw;
            }
        }

        protected abstract Task<Result<TPayload>> HandleCommandAsync<TPayload, TCommand>(IResolverContext context, TCommand command, CancellationToken token);

    }
}