using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.CommandHandler;
using Blauhaus.Auth.Abstractions.Services;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Graphql.HotChocolate.MutationHandlers._Base;
using CSharpFunctionalExtensions;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;

namespace Blauhaus.Graphql.HotChocolate.MutationHandlers
{
    public class AuthenticatedUserMutationServerHandler : BaseMutationServerHandler<IAuthenticatedUser>
    {
        private readonly IAzureAuthenticationServerService _authenticationServerService;

        public AuthenticatedUserMutationServerHandler(
            IAnalyticsService analyticsService,
            IAzureAuthenticationServerService authenticationServerService) 
                : base(analyticsService)
        {
            _authenticationServerService = authenticationServerService;
        }

 
        protected override bool TryExtractUser(IResolverContext resolverContext, out IAuthenticatedUser user)
        {
            if (!resolverContext.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var claimsPrincipal) ||
                !((ClaimsPrincipal) claimsPrincipal).Identity.IsAuthenticated)
            {
                user = null;
                return false;
            }

            user = _authenticationServerService.ExtractUserFromClaimsPrincipal((ClaimsPrincipal) claimsPrincipal);
            return true;
        }

    }
}