using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using HotChocolate.Types;

namespace Blauhaus.Graphql.HotChocolate.Extensions
{
    public static class ObjectTypeDescriptorFieldExtensions
    {
        
        public static IObjectFieldDescriptor AddField<TProperty, TOutputType, T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, TProperty>> expression) where TOutputType : class, IOutputType
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<TOutputType>();
        }

        //Id
        public static IObjectFieldDescriptor AddIdField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<IdType>>();
        }
        
        //String
        public static IObjectFieldDescriptor AddStringField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, string>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<StringType>>();
        }
        public static IObjectFieldDescriptor AddNullableStringField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, string?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<StringType>();
        }

        //Int
        public static IObjectFieldDescriptor AddIntField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, int>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<IntType>>();
        }
        public static IObjectFieldDescriptor AddNullableIntField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, int?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<IntType>();
        }
        
        //Long
        public static IObjectFieldDescriptor AddLongField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, long>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<LongType>>();
        }
        public static IObjectFieldDescriptor AddNullableLongField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, long?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<LongType>();
        }

        //Float
        public static IObjectFieldDescriptor AddFloatField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, float>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<FloatType>>();
        }
        public static IObjectFieldDescriptor AddFloatField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, double>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<FloatType>>();
        }
        public static IObjectFieldDescriptor AddNullableFloatField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, float?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<FloatType>();
        }
        public static IObjectFieldDescriptor AddNullableFloatField<T>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, double?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<FloatType>();
        }

        //List
        public static IObjectFieldDescriptor AddListField<T, TListItem, TType>(this IObjectTypeDescriptor<T> descriptor, Expression<Func<T, IEnumerable<TListItem>>> expression) 
            where TType : IType
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<ListType<TType>>>();
        }
    }
}