#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PowerKit.Extensions;

#if !POWERKIT_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class NotifyPropertyChangedExtensions
{
    extension<TOwner>(TOwner owner)
        where TOwner : INotifyPropertyChanged
    {
        /// <summary>
        /// Subscribes to changes of the specified property on the owner object.
        /// The returned <see cref="IDisposable" /> can be disposed to unsubscribe.
        /// </summary>
        public IDisposable WatchProperty<TProperty>(
            Expression<Func<TOwner, TProperty>> propertyExpression,
            Action<TProperty> callback,
            bool watchInitialValue = false
        )
        {
            var memberExpression =
                propertyExpression.Body as MemberExpression
                // The compiler implicitly wraps value types in a conversion expression when
                // the expression is typed to return a reference type.
                ?? (propertyExpression.Body as UnaryExpression)?.Operand as MemberExpression;

            if (
                memberExpression?.Member is not PropertyInfo property
                || !property.DeclaringType!.IsAssignableFrom(typeof(TOwner))
            )
            {
                throw new ArgumentException(
                    "Provided expression must reference a property of the owner type.",
                    nameof(propertyExpression)
                );
            }

            var getValue = propertyExpression.Compile();

            void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
            {
                if (
                    string.IsNullOrWhiteSpace(args.PropertyName)
                    || string.Equals(args.PropertyName, property.Name, StringComparison.Ordinal)
                )
                {
                    callback(getValue(owner));
                }
            }

            owner.PropertyChanged += OnPropertyChanged;

            if (watchInitialValue)
            {
                callback(getValue(owner));
            }

            return Disposable.Create(() => owner.PropertyChanged -= OnPropertyChanged);
        }

        /// <summary>
        /// Subscribes to changes of the specified properties on the owner object.
        /// The returned <see cref="IDisposable" /> can be disposed to unsubscribe.
        /// </summary>
        public IDisposable WatchProperties(
            IEnumerable<Expression<Func<TOwner, object?>>> propertyExpressions,
            Action callback,
            bool watchInitialValue = false
        )
        {
            var properties = propertyExpressions
                .Select(expression =>
                {
                    var memberExpression =
                        expression.Body as MemberExpression
                        // Because the expression is typed to return an object, the compiler will
                        // implicitly wrap it in a conversion unary expression if it's of any other type.
                        ?? (expression.Body as UnaryExpression)?.Operand as MemberExpression;

                    if (
                        memberExpression?.Member is not PropertyInfo property
                        || !property.DeclaringType!.IsAssignableFrom(typeof(TOwner))
                    )
                    {
                        throw new ArgumentException(
                            "Provided expression must reference a property of the owner type.",
                            nameof(propertyExpressions)
                        );
                    }

                    return property;
                })
                .ToArray();

            void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
            {
                if (
                    string.IsNullOrWhiteSpace(args.PropertyName)
                    || properties.Any(p =>
                        string.Equals(args.PropertyName, p.Name, StringComparison.Ordinal)
                    )
                )
                {
                    callback();
                }
            }

            owner.PropertyChanged += OnPropertyChanged;

            if (watchInitialValue)
            {
                callback();
            }

            return Disposable.Create(() => owner.PropertyChanged -= OnPropertyChanged);
        }

        /// <summary>
        /// Subscribes to changes of all properties on the owner object.
        /// The returned <see cref="IDisposable" /> can be disposed to unsubscribe.
        /// </summary>
        public IDisposable WatchAllProperties(Action callback, bool watchInitialValue = false)
        {
            void OnPropertyChanged(object? sender, PropertyChangedEventArgs args) => callback();
            owner.PropertyChanged += OnPropertyChanged;

            if (watchInitialValue)
            {
                callback();
            }

            return Disposable.Create(() => owner.PropertyChanged -= OnPropertyChanged);
        }
    }
}
