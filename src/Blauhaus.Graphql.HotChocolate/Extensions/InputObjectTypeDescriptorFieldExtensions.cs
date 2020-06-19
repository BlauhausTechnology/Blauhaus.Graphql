using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using HotChocolate.Types;

namespace Blauhaus.Graphql.HotChocolate.Extensions
{
    public static class InputObjectTypeDescriptorFieldExtensions
    { 
        public static IInputFieldDescriptor AddField<T, TProperty, TInputType>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, TProperty>> expression) where TInputType : class, IInputType where TProperty : notnull
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<TInputType>>();
        }
        public static IInputFieldDescriptor AddNullableField<T, TProperty, TInputType>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, TProperty>> expression) where TInputType : class, IInputType 
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<TInputType>();
        }
        
        #region Bool
        public static IInputFieldDescriptor AddBoolField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, bool>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<BooleanType>>();
        } 
        public static IInputFieldDescriptor AddNullableBoolField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, bool?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<BooleanType>();
        } 
        #endregion

        #region Uuid
        public static IInputFieldDescriptor AddUuidField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<UuidType>>();
        } 
        public static IInputFieldDescriptor AddNullableUuidField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<UuidType>();
        } 
        #endregion

        #region Id
        public static IInputFieldDescriptor AddIdField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<IdType>>();
        } 
        public static IInputFieldDescriptor AddNullableIdField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, Guid?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<IdType>();
        } 
        #endregion
        
        #region String
        public static IInputFieldDescriptor AddStringField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, string>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<StringType>>();
        } 
        public static IInputFieldDescriptor AddNullableStringField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, string?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<StringType>();
        } 
        #endregion

        #region Int
        public static IInputFieldDescriptor AddIntField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, int>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<IntType>>();
        } 
        public static IInputFieldDescriptor AddNullableIntField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, int?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<IntType>();
        } 
        #endregion
        
        #region Long
        public static IInputFieldDescriptor AddLongField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, long>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<LongType>>();
        } 
        public static IInputFieldDescriptor AddNullableLongField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, long?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<LongType>();
        } 
        #endregion

        #region Float
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
        #endregion

        #region List
        public static IInputFieldDescriptor AddListField<T, TListItem, TType>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, IEnumerable<TListItem>>> expression) 
            where TType : IType
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<ListType<TType>>>();
        } 
        public static IInputFieldDescriptor AddNullableListField<T, TListItem, TType>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, IEnumerable<TListItem>?>> expression) 
            where TType : IType
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<ListType<TType>>();
        } 
        #endregion

        #region DateTime

        public static IInputFieldDescriptor AddDateTimeField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, DateTime>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<NonNullType<DateTimeType>>();
        } 
        public static IInputFieldDescriptor AddNullableDateTimeField<T>(this IInputObjectTypeDescriptor<T> descriptor, Expression<Func<T, DateTime?>> expression)
        {
            return descriptor.Field(expression).Name(expression.ToPropertyName()).Type<DateTimeType>();
        } 

        #endregion

    }
}