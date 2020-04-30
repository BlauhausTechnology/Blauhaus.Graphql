using Blauhaus.Graphql.HotChocolate.MutationHandlers.Payload;
using Blauhaus.Graphql.HotChocolate.MutationHandlers.Void;
using HotChocolate.Types;

namespace Blauhaus.Graphql.HotChocolate.Extensions
{
    public static class ObjectTypeDescriptorExtensions
    {
        //Unauthenticated with return type
        public static IObjectTypeDescriptor AddMutation<TPayload, TPayloadType, TInputType, TCommand>(this IObjectTypeDescriptor descriptor, string name)
            where TInputType : InputObjectType<TCommand>
            where TPayloadType : ObjectType<TPayload>
        {
            descriptor.Field(name)
                .Argument("command", d => d.Type<TInputType>())
                .Type<TPayloadType>()
                .Resolver((context, token) => context.Service<MutationServerHandler>()
                    .HandleAsync<TPayload, TCommand>(context, token));

            return descriptor;
        }
        
        //Unauthenticated with no return type
        public static IObjectTypeDescriptor AddVoidMutation<TInputType, TCommand>(this IObjectTypeDescriptor descriptor, string name)
            where TInputType : InputObjectType<TCommand>
        {
            descriptor.Field(name)
                .Argument("command", d => d.Type<TInputType>())
                .Type<BooleanType>()
                .Resolver((context, token) => context.Service<VoidMutationServerHandler>()
                    .HandleAsync<TCommand>(context, token));

            return descriptor;
        }
        
        //Authenticated with return type
        public static IObjectTypeDescriptor AddAuthenticatedUserMutation<TPayload, TPayloadType, TInputType, TCommand>(this IObjectTypeDescriptor descriptor, string name)
            where TInputType : InputObjectType<TCommand>
            where TPayloadType : ObjectType<TPayload>
        {
            descriptor.Field(name)
                .Argument("command", d => d.Type<TInputType>())
                .Type<TPayloadType>()
                .Authorize()
                .Resolver((context, token) => context.Service<AuthenticatedUserMutationServerHandler>()
                    .HandleAsync<TPayload, TCommand>(context, token));
            return descriptor;
        }
        
        //Authenticated with no return type
        public static IObjectTypeDescriptor AddVoidAuthenticatedUserMutation<TInputType, TCommand>(this IObjectTypeDescriptor descriptor, string name)
            where TInputType : InputObjectType<TCommand>
        {
            descriptor.Field(name)
                .Argument("command", d => d.Type<TInputType>())
                .Type<BooleanType>()
                .Authorize()
                .Resolver((context, token) => context.Service<VoidAuthenticatedUserMutationServerHandler>()
                    .HandleAsync<TCommand>(context, token));

            return descriptor;
        }

         

    }
}