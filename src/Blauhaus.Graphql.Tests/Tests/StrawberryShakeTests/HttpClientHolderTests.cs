using System;
using System.Collections.Generic;
using System.Linq;
using Blauhaus.Auth.Abstractions.AccessToken;
using Blauhaus.Graphql.StrawberryShake.Config;
using Blauhaus.Graphql.StrawberryShake.HttpClients;
using Blauhaus.Graphql.Tests.Tests._Base;
using Blauhaus.TestHelpers.MockBuilders;
using NUnit.Framework;

namespace Blauhaus.Graphql.Tests.Tests.StrawberryShakeTests
{
    public class HttpClientHolderTests : BaseGraphqlTest<HttpClientHolder>
    {

        protected MockBuilder<IGraphqlClientConfig> MockGraphqlClientConfig => AddMock<IGraphqlClientConfig>().Invoke();
        protected MockBuilder<IAuthenticatedAccessToken> MockAuthenticatedAccessToken => AddMock<IAuthenticatedAccessToken>().Invoke();

        public override void Setup()
        {
            base.Setup();
            
            MockGraphqlClientConfig.With(x => x.GraphqlEndpoint, "http://www.google.com");
            MockAnalyticsService.With(x => x.AnalyticsOperationHeaders, new Dictionary<string, string>());
            MockAuthenticatedAccessToken.With(x => x.AdditionalHeaders, new Dictionary<string, string>());

            AddService(x => MockGraphqlClientConfig.Object);
            AddService(x => MockAuthenticatedAccessToken.Object);
        }

        [Test]
        public void SHOULD_Add_Endpoint()
        {
            //Arrange
            MockGraphqlClientConfig.With(x => x.GraphqlEndpoint, "http://www.google.com/graphql");

            //Act
            var result = Sut.CreateClient("");

            //Assert
            Assert.That(result.BaseAddress, Is.EqualTo(new Uri("http://www.google.com/graphql")));
        }
        
        [Test]
        public void SHOULD_Add_AnalyticsHeaders()
        {
            //Act
            MockAnalyticsService.With(x => x.AnalyticsOperationHeaders, new Dictionary<string, string>
            {
                ["headerKey"] = "headerValue"
            });
            
            //Act
            var result = Sut.CreateClient("");

            //Assert
            Assert.That(result.DefaultRequestHeaders.First(x => x.Key == "headerKey").Value.First(), Is.EqualTo("headerValue"));
        }
        
        [Test]
        public void SHOULD_Add_Authorization_scheme_and_token()
        {
            //Act
            MockAuthenticatedAccessToken.With(x => x.Scheme, "Bonkers");
            MockAuthenticatedAccessToken.With(x => x.Token, "BonkersToken");
            
            //Act
            var result = Sut.CreateClient("");

            //Assert
            Assert.That(result.DefaultRequestHeaders.Authorization.Scheme, Is.EqualTo("Bonkers"));
            Assert.That(result.DefaultRequestHeaders.Authorization.Parameter, Is.EqualTo("BonkersToken"));
        }

        
        [Test]
        public void SHOULD_Add_Additional_Authorization_headers()
        {
            //Act
            MockAuthenticatedAccessToken.With(x => x.AdditionalHeaders, new Dictionary<string, string>
            {
                {"AuthMe", "Baby"}
            });
            
            //Act
            var result = Sut.CreateClient("");

            //Assert
            Assert.That(result.DefaultRequestHeaders.First(x => x.Key == "AuthMe").Value.First(), Is.EqualTo("Baby"));
        }
    }
}