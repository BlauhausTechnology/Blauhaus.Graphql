using System;
using System.Security.Claims;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.TestHelpers;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.Auth.Abstractions.Builders;
using Blauhaus.Auth.Abstractions.Services;
using Blauhaus.Auth.TestHelpers.MockBuilders;
using Blauhaus.Graphql.HotChocolate.TestHelpers.MockBuilders;
using Blauhaus.Graphql.Tests.MockBuilders;
using Blauhaus.TestHelpers.BaseTests;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Blauhaus.Graphql.Tests.Tests._Base
{
    public abstract class BaseGraphqlTest<TSut> : BaseServiceTest<TSut> where TSut : class
    {
        [SetUp]
        public virtual void Setup()
        {
            Cleanup();

            Services.AddSingleton(x => MockResolverContext.Object);
            Services.AddSingleton(x => MockAnalyticsService.Object);
            Services.AddSingleton(x => MockAuthenticatedUserFactory.Object);

            ClaimsPrincipal = new ClaimsPrincipalBuilder()
                .WithIsAuthenticatedTrue()
                .With_UserObjectId(Guid.NewGuid()).Build();
        }

        protected ClaimsPrincipal ClaimsPrincipal;

        protected ResolverContextMockBuilder MockResolverContext => Mocks.AddMock<ResolverContextMockBuilder, IResolverContext>().Invoke();
        protected AnalyticsServiceMockBuilder MockAnalyticsService => Mocks.AddMock<AnalyticsServiceMockBuilder, IAnalyticsService>().Invoke();
        protected AuthenticatedUserFactoryMockBuilder MockAuthenticatedUserFactory => Mocks.AddMock<AuthenticatedUserFactoryMockBuilder, IAuthenticatedUserFactory>().Invoke();

    }
}