using CSharpFunctionalExtensions;
using StrawberryShake;

namespace Blauhaus.Graphql.StrawberryShake.DtoCommandHandlers
{
    public interface IOperationResultConverter<TDto, TGraphqlResponse> where TGraphqlResponse : class
    {
        Result<TDto> Convert(IOperationResult<TGraphqlResponse> operationResult);
    }
}