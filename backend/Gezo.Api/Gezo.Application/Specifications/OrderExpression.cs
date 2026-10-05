using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Gezo.Application.Specifications
{
    public class OrderExpression<Entity>
    {
        public Expression<Func<Entity, object>> Expression { get; }

        public bool IsDescending { get; }

        public OrderExpression(Expression<Func<Entity, object>> expression, bool isDescending)
        {
            Expression = expression;
            IsDescending = isDescending;
        }

    }
}
