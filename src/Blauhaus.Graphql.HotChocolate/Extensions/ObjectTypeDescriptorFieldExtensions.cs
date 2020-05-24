using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace Blauhaus.Graphql.HotChocolate.Extensions
{
    public static class ObjectTypeDescriptorFieldExtensions
    {
        
        public static IObjectFieldDescriptor AddField<TProperty, TOutputType, T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, TProperty>> expression) where TOutputType : class, IOutputType
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<TOutputType>();
        }
        
        #region Bool
        public static IObjectFieldDescriptor AddBoolField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, bool>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<BooleanType>>();
        }
        public static IObjectFieldDescriptor AddBoolField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, bool>> resolverExpression)
        {
            return descriptor.Field(name).Type<NonNullType<BooleanType>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        public static IObjectFieldDescriptor AddNullableBoolField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, bool?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<BooleanType>();
        }
        public static IObjectFieldDescriptor AddNullableBoolField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, bool?>> resolverExpression)
        {
            return descriptor.Field(name).Type<BooleanType>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        #endregion

        #region Uuid
        public static IObjectFieldDescriptor AddUuidField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<UuidType>>();
        }
        public static IObjectFieldDescriptor AddUuidField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, Guid>> resolverExpression)
        {
            return descriptor.Field(name).Type<NonNullType<UuidType>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        public static IObjectFieldDescriptor AddNullableUuidField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<UuidType>();
        }
        public static IObjectFieldDescriptor AddNullableUuidField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, Guid?>> resolverExpression)
        {
            return descriptor.Field(name).Type<UuidType>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        #endregion

        #region Id
        public static IObjectFieldDescriptor AddIdField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<IdType>>();
        }
        public static IObjectFieldDescriptor AddIdField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, Guid>> resolverExpression)
        {
            return descriptor.Field(name).Type<NonNullType<IdType>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        public static IObjectFieldDescriptor AddNullableIdField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<IdType>();
        }
        public static IObjectFieldDescriptor AddNullableIdField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, Guid?>> resolverExpression)
        {
            return descriptor.Field(name).Type<IdType>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        #endregion
        
        #region String
        public static IObjectFieldDescriptor AddStringField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, string>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<StringType>>();
        }
        public static IObjectFieldDescriptor AddStringField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, string>> resolverExpression)
        {
            return descriptor.Field(name).Type<NonNullType<StringType>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        public static IObjectFieldDescriptor AddNullableStringField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, string?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<StringType>();
        }
        public static IObjectFieldDescriptor AddNullableStringField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, string>> resolverExpression)
        {
            return descriptor.Field(name).Type<StringType>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        #endregion

        #region Int
        public static IObjectFieldDescriptor AddIntField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, int>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<IntType>>();
        }
        public static IObjectFieldDescriptor AddIntField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, int>> resolverExpression)
        {
            return descriptor.Field(name).Type<NonNullType<IntType>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        public static IObjectFieldDescriptor AddNullableIntField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, int?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<IntType>();
        }
        public static IObjectFieldDescriptor AddNullableIntField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, int?>> resolverExpression)
        {
            return descriptor.Field(name).Type<IntType>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        #endregion
        
        #region Long
        public static IObjectFieldDescriptor AddLongField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, long>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<LongType>>();
        }
        public static IObjectFieldDescriptor AddLongField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, long>> resolverExpression)
        {
            return descriptor.Field(name).Type<NonNullType<LongType>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        public static IObjectFieldDescriptor AddNullableLongField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, long?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<LongType>();
        }
        public static IObjectFieldDescriptor AddNullableLongField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, long?>> resolverExpression)
        {
            return descriptor.Field(name).Type<LongType>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        #endregion

        #region Float
        public static IObjectFieldDescriptor AddFloatField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, float>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<FloatType>>();
        }
        public static IObjectFieldDescriptor AddFloatField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, double>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<FloatType>>();
        }
        public static IObjectFieldDescriptor AddFloatField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, double>> resolverExpression)
        {
            return descriptor.Field(name).Type<NonNullType<FloatType>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        public static IObjectFieldDescriptor AddFloatField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, float>> resolverExpression)
        {
            return descriptor.Field(name).Type<NonNullType<FloatType>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }

        public static IObjectFieldDescriptor AddNullableFloatField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, float?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<FloatType>();
        }
        public static IObjectFieldDescriptor AddNullableFloatField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, double?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<FloatType>();
        }
        public static IObjectFieldDescriptor AddNullableFloatField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, double?>> resolverExpression)
        {
            return descriptor.Field(name).Type<FloatType>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        public static IObjectFieldDescriptor AddNullableFloatField<T>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, float?>> resolverExpression)
        {
            return descriptor.Field(name).Type<FloatType>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        #endregion

        #region List
        public static IObjectFieldDescriptor AddListField<T, TListItem, TType>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, IEnumerable<TListItem>>> expression) 
            where TType : IType
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<ListType<TType>>>();
        }
        public static IObjectFieldDescriptor AddListField<T, TListItem, TType>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, IEnumerable<TListItem>>> resolverExpression) where TType : IType
        {
            return descriptor.Field(name).Type<NonNullType<ListType<TType>>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        public static IObjectFieldDescriptor AddNullableListField<T, TListItem, TType>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, IEnumerable<TListItem>?>> expression) 
            where TType : IType
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<ListType<TType>>();
        }
        public static IObjectFieldDescriptor AddNullableListField<T, TListItem, TType>(this IObjectTypeDescriptor<T> descriptor, string name, Expression<Func<IResolverContext, IEnumerable<TListItem>?>> resolverExpression) where TType : IType
        {
            return descriptor.Field(name).Type<ListType<TType>>().Resolver(context => resolverExpression.Compile().Invoke(context));
        }
        #endregion
    }
}