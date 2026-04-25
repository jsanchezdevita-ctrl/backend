using System.Linq.Expressions;

namespace Application.Abstractions.Paginations;

public static class QueryableExtensions
{
    public static IQueryable<T> SearchByTerm<T>(
        this IQueryable<T> source,
        string? searchTerm,
        params Expression<Func<T, string>>[] properties)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || properties.Length == 0)
            return source;

        // Crear el parámetro para la expresión lambda
        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? orExpression = null;

        foreach (var propertySelector in properties)
        {
            // Reemplazar el parámetro del selector original con nuestro parámetro
            var body = ReplaceParameter(propertySelector.Body, propertySelector.Parameters[0], parameter);

            var containsExpression = Expression.Call(
                body,
                nameof(string.Contains),
                Type.EmptyTypes,
                Expression.Constant(searchTerm));

            orExpression = orExpression == null
                ? containsExpression
                : Expression.OrElse(orExpression, containsExpression);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(orExpression!, parameter);
        return source.Where(lambda);
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}