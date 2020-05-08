using Blauhaus.Graphql.HotChocolate.QueryHandlers.Payload;
using Blauhaus.Graphql.HotChocolate.QueryHandlers.Void;
using HotChocolate.Types;

namespace Blauhaus.Graphql.HotChocolate.Extensions
{
    public static class ObjectTypeDescriptorExtensions
    {
        //Unauthenticated with return type
        public static IObjectTypeDescriptor AddAnonymousQueryHandler<TPayload, TPayloadType, TInputType, TCommand>(this IObjectTypeDescriptor descriptor, string name)
            where TInputType : InputObjectType<TCommand>
            where TPayloadType : ObjectType<TPayload>
        {
            descriptor.Field(name)
                .Argument("command", d => d.Type<TInputType>())
                .Type<TPayloadType>()
                .Resolver((context, token) => context.Service<AnonymousServerQueryHandler>()
                    .HandleAsync<TPayload, TCommand>(context, token));

            return descriptor;
        }
        
        //Unauthenticated with no return type
        public static IObjectTypeDescriptor AddVoidAnonymousQueryHandler<TInputType, TCommand>(this IObjectTypeDescriptor descriptor, string name)
            where TInputType : InputObjectType<TCommand>
        {
            descriptor.Field(name)
                .Argument("command", d => d.Type<TInputType>())
                .Type<BooleanType>()
                .Resolver((context, token) => context.Service<VoidAnonymousServerQueryHandler>()
                    .HandleAsync<TCommand>(context, token));

            return descriptor;
        }
        
        //Authenticated with return type
        public static IObjectTypeDescriptor AddAuthenticatedUserQueryHandler<TPayload, TPayloadType, TInputType, TCommand>(this IObjectTypeDescriptor descriptor, string name)
            where TInputType : InputObjectType<TCommand>
            where TPayloadType : ObjectType<TPayload>
        {
            descriptor.Field(name)
                .Argument("command", d => d.Type<TInputType>())
                .Type<TPayloadType>()
                .Authorize()
                .Resolver((context, token) => context.Service<AuthenticatedUserServerQueryHandler>()
                    .HandleAsync<TPayload, TCommand>(context, token));
            return descriptor;
        }
        
        //Authenticated with no return type
        public static IObjectTypeDescriptor AddVoidAuthenticatedUserQueryHandler<TInputType, TCommand>(this IObjectTypeDescriptor descriptor, string name)
            where TInputType : InputObjectType<TCommand>
        {
            descriptor.Field(name)
                .Argument("command", d => d.Type<TInputType>())
                .Type<BooleanType>()
                .Authorize()
                .Resolver((context, token) => context.Service<VoidAuthenticatedUserServerQueryHandler>()
                    .HandleAsync<TCommand>(context, token));

            return descriptor;
        }

         

    }
}