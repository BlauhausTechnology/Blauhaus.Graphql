using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.Errors;
using Blauhaus.Responses;
using CSharpFunctionalExtensions;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Void._Base
{
    public abstract class BaseVoidServerQueryHandler : IVoidServerQueryHandler
    {
        protected readonly IAnalyticsService AnalyticsService;

        protected BaseVoidServerQueryHandler(
            IAnalyticsService analyticsService)
        {
            AnalyticsService = analyticsService;
        }

        public async Task<bool> HandleAsync<TCommand>(IResolverContext context, CancellationToken token)
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
                    
                    AnalyticsService.TraceVerbose(this, $"{typeof(TCommand).Name} received", command.ToObjectDictionary());

                    var commandResult = await HandleCommandAsync<TCommand>(context, command, token);
                    if (commandResult.IsFailure)
                    {
                        context.ReportError(new ErrorBuilder().SetMessage(commandResult.Error.ToString()).Build());
                        return default;
                    }
                    
                    return commandResult.Value;


                }
            }
            catch (UnauthorizedAccessException)
            {
                context.ReportError(new ErrorBuilder().SetMessage(AuthErrors.NotAuthorized.ToString()).Build());
                AnalyticsService.TraceError(this, AuthErrors.NotAuthorized);
                return false;
            }
            catch (Exception e)
            {
                AnalyticsService.LogException(this, e);
                throw;
            }
        }

        protected abstract Task<Response<bool>> HandleCommandAsync<TCommand>(IResolverContext context, TCommand command, CancellationToken token);

    }
}