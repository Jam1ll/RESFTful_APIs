using Ardalis.Specification.EntityFrameworkCore;
using core.application.Interfaces;
using infrastructure.persistence.Contexts;

namespace infrastructure.persistence.Repositories
{
    public class RepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public RepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
