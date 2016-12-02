using System;
using System.Linq;


//Credit for the code : 
//https://gist.github.com/Grinderofl/2767155
//http://codereview.stackexchange.com/questions/3560/dynamic-filtering-and-sorting-with-entity-framework

namespace Test_Solution1.Common.Strategy
{
    public delegate IQueryable<TEntity> QueryMutator<TEntity, TCriteria>(IQueryable<TEntity> items, TCriteria criteria);

    public class GenericFilterStrategy<TEntity, TCriteria>
        where TCriteria : class
        where TEntity : class
    {
        public Predicate<TCriteria> Criteria { get; set; }
        public QueryMutator<TEntity, TCriteria> Mutator { get; set; }

        public GenericFilterStrategy(Predicate<TCriteria> criteria, QueryMutator<TEntity, TCriteria> mutator)
        {
            Criteria = criteria;
            Mutator = mutator;
        }

        public IQueryable<TEntity> Apply(TCriteria search, IQueryable<TEntity> query)
        {
            return Criteria(search) ? Mutator(query, search) : query;
        }
    }
}
