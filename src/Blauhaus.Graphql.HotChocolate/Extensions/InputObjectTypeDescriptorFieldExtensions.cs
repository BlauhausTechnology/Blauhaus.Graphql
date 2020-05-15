using System;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using HotChocolate.Types;

namespace Blauhaus.Graphql.HotChocolate.Extensions
{
    public static class InputObjectTypeDescriptorFieldExtensions
    {
        
        public static IInputFieldDescriptor AddField<TProperty, TOutputType, T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, TProperty>> expression) where TOutputType : class, IInputType
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<TOutputType>();
        }

        //Id
        public static IInputFieldDescriptor AddIdField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<IdType>>();
        }
        public static IInputFieldDescriptor AddIdField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<IdType>();
        }
        
        //String
        public static IInputFieldDescriptor AddStringField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, string>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<StringType>>();
        }
        public static IInputFieldDescriptor AddNullableStringField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, string?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<StringType>();
        }

        //Int
        public static IInputFieldDescriptor AddIntField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, int>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<IntType>>();
        }
        public static IInputFieldDescriptor AddIntField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, int?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<IntType>();
        }
        
        //Long
        public static IInputFieldDescriptor AddLongField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, long>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<LongType>>();
        }
        public static IInputFieldDescriptor AddNullableLongField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, long?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<LongType>();
        }

        //Float
        public static IInputFieldDescriptor AddFloatField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, float>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<FloatType>>();
        }
        public static IInputFieldDescriptor AddFloatField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, double>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<FloatType>>();
        }
        public static IInputFieldDescriptor AddNullableFloatField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, float?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<FloatType>();
        }
        public static IInputFieldDescriptor AddNullableFloatField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, double?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<FloatType>();
        }
    }
}