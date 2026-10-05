using Gezo.Application.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Gezo.Infrastructure.SpecificationEvaluator
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<Entity> GetQuery<Entity>(IQueryable<Entity> inputQuery,ISpecification<Entity> specification) where Entity : class
        {
            if(specification.Criteria !=null)
            {
                inputQuery = inputQuery.Where(specification.Criteria);
            }

            if(specification.Includes  is not null && specification.Includes.Any())
            {
                foreach(var include in specification.Includes)
                {
                    inputQuery = inputQuery.Include(include);
                }

            }
            if(specification.OrderExpressions.Any())
            {
                var firstOrder = specification.OrderExpressions[0];
                IOrderedQueryable<Entity> orderedQuery;
                if (firstOrder.IsDescending==false)
                {
                     orderedQuery = inputQuery.OrderBy(firstOrder.Expression);
                }
                else
                {
                     orderedQuery = inputQuery.OrderByDescending(firstOrder.Expression);
                }

                for(int i=1;i<specification.OrderExpressions.Count;i++)
                {
                    if (specification.OrderExpressions[i].IsDescending == false)
                    {
                        orderedQuery = orderedQuery.ThenBy(specification.OrderExpressions[i].Expression);
                    }
                    else
                    {
                        orderedQuery = orderedQuery.ThenByDescending(specification.OrderExpressions[i].Expression);
                    }

                }
                inputQuery = orderedQuery;

            }

            if(specification.IsPagingEnabled)
            {
                inputQuery = inputQuery.Skip(specification.Skip).Take(specification.Take);
            }


            return inputQuery;
        }
    }
}
