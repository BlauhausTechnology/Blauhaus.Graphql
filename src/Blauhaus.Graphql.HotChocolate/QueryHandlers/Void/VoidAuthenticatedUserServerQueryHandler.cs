﻿using System.Security.Claims;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Auth.Abstractions.Services;
using Blauhaus.Auth.Abstractions.User;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Void._Base;
using HotChocolate.Resolvers;

namespace Blauhaus.Graphql.HotChocolate.QueryHandlers.Void
{
    public class VoidAuthenticatedUserServerQueryHandler : BaseVoidAuthenticatedServerQueryHandler<IAuthenticatedUser>
    {
        private readonly IAzureAuthenticationServerService _authenticationServerService;

        public VoidAuthenticatedUserServerQueryHandler(
            IAnalyticsService analyticsService,
            IAzureAuthenticationServerService authenticationServerService) 
                : base(analyticsService)
        {
            _authenticationServerService = authenticationServerService;
        }

 
        protected override bool TryExtractUser(IResolverContext resolverContext, out IAuthenticatedUser user)
        {
            if (!resolverContext.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var claimsPrincipal) ||
                claimsPrincipal == null ||
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