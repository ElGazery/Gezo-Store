using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Gezo.Application.Specifications
{
    public abstract class BaseSpecification<Entity> : ISpecification<Entity>
    {
        public Expression<Func<Entity, bool>>? Criteria { get; protected set; }

        public List<Expression<Func<Entity, object>>> Includes { get; }= new();

        protected void AddInclude(Expression<Func<Entity ,object>> include)
        {
            Includes.Add(include);

        }

        public List<OrderExpression<Entity>> OrderExpressions { get; } = new();

        protected void AddOrderBy(Expression<Func<Entity, object>> expreession, bool isDescending =false)
        {
            OrderExpressions.Add(
                new OrderExpression<Entity>(expreession, isDescending)
                );
        }



        public int Skip { get; protected set; }

        public int Take { get; protected set; }

        public bool IsPagingEnabled { get; protected set; }
        protected void ApplyPagination(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
