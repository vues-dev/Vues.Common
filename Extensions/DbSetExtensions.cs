using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Vues.Common.Extensions;

/// <summary>
/// DbSet Extensions
/// </summary>
public static class DbSetExtensions
{
    /// <summary>
    /// Adding Entity / Добавление сущности
    /// </summary>
    /// <typeparam name="T">Entity type / Тип сущности</typeparam>
    /// <param name="dbSet">Context / Контекст</param>
    /// <param name="entity">Entity / Сущность</param>
    public static void Insert<T>(this DbSet<T> dbSet, T entity) where T : class
    {
        var DbContext = GetDbContext(dbSet);
        try
        {
            dbSet.Add(entity);
            DbContext.SaveChanges();
        }
        finally
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Adding Entity async / Асинхронное добавление сущности
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dbSet">Context / Контекст</param>
    /// <param name="entity">Entity / Сущность</param>
    public static async Task InsertAsync<T>(this DbSet<T> dbSet, T entity, CancellationToken cancellationToken = default) where T : class
    {
        var DbContext = GetDbContext(dbSet);
        try
        {
            await dbSet.AddAsync(entity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Update entity / Обновление сущности
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="dbSet">Context / Контекст</param>
    /// <param name="entity">Entity / Сущность</param>
    public static void Update<T>(this DbSet<T> dbSet, T entity) where T : class
    {
        var DbContext = GetDbContext(dbSet);
        try
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
        finally
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Update entity async / Асинхронное обновление сущности
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="dbSet">Context / Контекст</param>
    /// <param name="entity">Entity / Сущность</param>
    public static async Task UpdateAsync<T>(this DbSet<T> dbSet, T entity, CancellationToken cancellationToken = default) where T : class
    {
        var DbContext = GetDbContext(dbSet);
        try
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }
    }


    /// <summary>
    /// Delete entity / Удаление сущности
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dbSet">Context / Контекст</param>
    /// <param name="entity">Entity / Сущность</param>
    public static void Delete<T>(this DbSet<T> dbSet, T entity) where T : class
    {
        var DbContext = GetDbContext(dbSet);
        try
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
            dbSet.Remove(entity);
            DbContext.SaveChanges();
        }
        finally
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Delete entity async / Асинхронное удаление сущности
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <param name="dbSet">Context / Контекст</param>
    /// <param name="entity">Entity / Сущность</param>
    public static async Task DeleteAsync<T>(this DbSet<T> dbSet, T entity) where T : class
    {
        var DbContext = GetDbContext(dbSet);
        try
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
            dbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
        }
        finally
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Check if entity is not null, then delete it
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="dbSet">DbSet</param>
    /// <param name="entity">Entity</param>
    /// <returns>void</returns>
    public static async Task DeleteIfNotNullAsync<T>(this DbSet<T> dbSet, T entity) where T : class
    { 
        if (entity is null)
            return;

        await dbSet.DeleteAsync(entity);
    }

    /// <summary>
    /// Получение контекста из dbSet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dbSet"></param>
    /// <returns></returns>
    private static DbContext GetDbContext<T>(DbSet<T> dbSet) where T : class
    {
        var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
        var serviceProvider = infrastructure.Instance;
        var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext))
                                   as ICurrentDbContext;
        return currentDbContext!.Context;
    }
}