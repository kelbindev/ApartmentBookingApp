using Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class Repository<T> 
    where T:Entity
{
    protected readonly ApplicationDbContext _dbContext;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<T>()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        _dbContext.Add(entity);
    }
}
