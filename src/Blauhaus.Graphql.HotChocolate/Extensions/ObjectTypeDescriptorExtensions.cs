using Blauhaus.Auth.Abstractions.CommandHandler;
using Blauhaus.Graphql.HotChocolate.MutationHandlers;
using HotChocolate.Types;

namespace Blauhaus.Graphql.HotChocolate.Extensions
{
    public static class ObjectTypeDescriptorExtensions
    {
        public static IObjectTypeDescriptor AddMutation<TPayload, TPayloadType, TInputType, TCommand>(this IObjectTypeDescriptor descriptor, string name)
            where TInputType : InputObjectType<TCommand>
            where TPayloadType : ObjectType<TPayload>
        {
            descriptor.Field(name)
                .Authorize()
                .Argument("command", d => d.Type<TInputType>())
                .Type<TPayloadType>()
                .Resolver((context, token) => context.Service<IMutationServerHandler>()
                    .HandleAsync<TPayload, TCommand>(context, token));

            return descriptor;
        }
    }
}