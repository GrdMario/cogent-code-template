namespace CogentCode.Template.Infrastructure.Db.MyDb.Internal.Repositories
{
    using CogentCode.Template.Application.Contracts;
    using CogentCode.Template.Blocks.Common.Exceptions;
    using CogentCode.Template.Domain;
    using Microsoft.EntityFrameworkCore;
    using CogentCode.Template.Infrastructure.Db.MyDb.Internal;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class CategoryRepository : ICategoryRepository
    {
        private readonly DbSet<Category> _categories;

        public CategoryRepository(MyDbContext context)
        {
            this._categories = context.Set<Category>();
        }
        public void Add(Category category)
        {
            this._categories.Add(category);
        }

        public void Delete(Category category)
        {
            this._categories.Remove(category);
        }

        public async Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await this._categories.FindAsync(id, cancellationToken);

            return category ?? throw new ServiceValidationException("Not found.");
        }

        public async Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            return await this._categories.ToListAsync(cancellationToken);
        }

        public void Update(Category category)
        {
            this._categories.Update(category);
        }
    }
}
