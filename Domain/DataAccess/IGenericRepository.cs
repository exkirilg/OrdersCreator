﻿using Domain.Models;
using System.Linq.Expressions;

namespace Domain.DataAccess;

public interface IGenericRepository<TEntity> where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetAll(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");

    Task<TEntity> GetById(int id, string includeProperties = "");

    Task Insert(TEntity entity);

    void Update(TEntity entity);

    Task Delete(int id);
}
