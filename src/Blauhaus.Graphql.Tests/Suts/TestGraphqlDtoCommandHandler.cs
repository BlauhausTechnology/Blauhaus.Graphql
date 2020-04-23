using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.DtoCommandHandlers;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.TestHelpers.MockBuilders;
using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.Tests.Suts
{
    public class TestGraphqlDtoCommandHandler : MutationClientHandler<TestDto, TestGraphqlResponse, TestCommandDto>
    {
        public TestGraphqlDtoCommandHandler(
            IGraphqlClient<TestGraphqlResponse, TestCommandDto> graphqlClient, 
            IOperationResultConverter<TestDto, TestGraphqlResponse> operationResultConverter) 
                : base(graphqlClient, operationResultConverter)
        {
        }
    }
}