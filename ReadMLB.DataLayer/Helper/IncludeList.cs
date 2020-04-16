using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ReadMLB.DataLayer.Helper
{
    //public class IncludeList<TEntity>
    //{
    //        private readonly List<LambdaExpression> _list = new List<LambdaExpression>();

    //        public void Add<TProperty>(Expression<Func<TEntity, TProperty>> include)
    //        {
    //            _list.Add(include);
    //        }

    //        public IEnumerable<LambdaExpression> Includes
    //        {
    //            get { return _list; }
    //        }
    //}

    //public class IncludeList<TEntity>
    //{
    //    private Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> _list;

    //    public void Add<TProperty>(Expression<Func<TEntity, TProperty>> include)
    //    {
    //        _list.Add(include);
    //    }

    //    public IEnumerable<LambdaExpression> Includes
    //    {
    //        get { return _list; }
    //    }
    //}
}
