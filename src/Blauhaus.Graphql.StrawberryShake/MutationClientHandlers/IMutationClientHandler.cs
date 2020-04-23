using Blauhaus.Common.Domain.CommandHandlers;

namespace Blauhaus.Graphql.StrawberryShake.MutationClientHandlers
{
    public interface IMutationClientHandler<TModelDto, TCommandDto>: ICommandHandler<TModelDto, TCommandDto>
    {
        
    }
}