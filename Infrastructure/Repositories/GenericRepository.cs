using Application.Interfaces;
using Domain.Models.ResultTypes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<OperationResult<T>> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return OperationResult<T>.Success(entity);
        }

        public async Task<OperationResult<bool>> DeleteAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return OperationResult<bool>.Failure("Entity not found");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<IEnumerable<T>>> GetAllAsync()
        {
            var list = await _dbSet.ToListAsync();
            if (list == null || !list.Any())
                return OperationResult<IEnumerable<T>>.Failure("No entities found");

            return OperationResult<IEnumerable<T>>.Success(list);
        }

        public async Task<OperationResult<T>> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return OperationResult<T>.Failure("Entity not found");

            return OperationResult<T>.Success(entity);
        }

        public async Task<OperationResult<T>> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return OperationResult<T>.Success(entity);
        }
    }
}
