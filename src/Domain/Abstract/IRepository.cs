using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ConsoleApplication_test.Entitys;

namespace Domain.Abstract
{
    public interface IRepository<T> where T : EntityBase
    {
        T GetById(int id);
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
    }
}