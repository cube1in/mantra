using System;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// A helper for expressions
/// </summary>
public static class ExpressionHelpers
{
    /// <summary>
    /// 编译表达式并获取函数返回值
    /// </summary>
    /// <typeparam name="T">返回值的类型</typeparam>
    /// <param name="lambda">要编译的表达式</param>
    /// <returns></returns>
    public static T GetPropertyValue<T>(this Expression<Func<T>> lambda)
    {
        return lambda.Compile().Invoke();
    }

    /// <summary>
    /// 将属性值设置为给定值
    /// </summary>
    /// <typeparam name="T">返回值的类型</typeparam>
    /// <param name="lambda">要编译的表达式</param>
    /// <param name="value">要设置的值</param>
    public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
    {
        // 将 lambda () => some.Property 转为 some.Property
        var expression = lambda.Body as MemberExpression;

        // 获取属性信息，用于设置
        var propertyInfo = expression?.Member as PropertyInfo;

        // 获取目标类
        var target = (expression?.Expression as ConstantExpression)?.Value;

        // 设置
        propertyInfo?.SetValue(target, value);
    }
}