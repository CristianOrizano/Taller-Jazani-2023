﻿using Jazani.Taller.Domain.Cores.Repositories;
using Jazani.Taller.Infrastructure.Cores.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.Taller.Infrastructure.Cores.Persistences
{
    public abstract class CrudRepository<T, ID> : ICrudRepository<T, ID> where T : class
    {
        private readonly AplicationDbContext _dbContext;
        protected CrudRepository(AplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<IReadOnlyList<T>> FindAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> FindByIdAsync(ID id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> SaveAsync(T entity)
        {
            EntityState state = _dbContext.Entry(entity).State;

            if (state == EntityState.Detached)
            {
                _dbContext.Set<T>().Add(entity);
            }
            else if (state == EntityState.Modified)
            {
                _dbContext.Set<T>().Update(entity);
            }
            else
            {
                _dbContext.Entry(entity).State = EntityState.Modified; // Por defecto, establece el estado como modificado
            }


            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
