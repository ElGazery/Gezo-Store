using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Gezo.Application.Specifications
{
    public interface ISpecification<Entity>
    {
        Expression<Func<Entity,bool>>? Criteria { get; }
        List<Expression<Func<Entity,object>>> Includes { get; }
        List<OrderExpression<Entity>> OrderExpressions { get; }
        
        int Skip { get; }
        int Take { get; }
        bool IsPagingEnabled { get; }


    }
}
