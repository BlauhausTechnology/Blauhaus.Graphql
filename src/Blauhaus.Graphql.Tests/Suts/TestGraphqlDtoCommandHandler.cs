using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.DtoCommandHandlers;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.TestHelpers.MockBuilders;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.Tests.Suts
{
    public class TestGraphqlDtoCommandHandler : MutationClientHandler<TestModelDto, TestGraphqlResponse, TestCommandDto, TestCommand>
    {
        public TestGraphqlDtoCommandHandler(
            IGraphqlClient<TestModelDto,  TestGraphqlResponse, TestCommandDto, TestCommand> graphqlClient) 
                : base(graphqlClient)
        {
        }
    }
}