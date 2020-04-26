using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers;
using Blauhaus.Graphql.StrawberryShake.MutationClientHandlers.Payload;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.TestHelpers.MockBuilders;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.Tests.Suts
{
    public class TestGraphqlDtoCommandHandler : MutationClientHandler<TestModelDto, TestGraphqlResponse, TestCommandDto, TestCommand>
    {
        public TestGraphqlDtoCommandHandler(
            IMutationClient<TestModelDto,  TestGraphqlResponse, TestCommandDto, TestCommand> graphqlClient) 
                : base(graphqlClient)
        {
        }
    }
}