using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity.Utility;
using VM;

namespace VM.Repositories
{
public class ProductRepository : VmRepository<Product>, IProductRepository
{
    public ProductRepository(VmDbContext dbContext)
        : base(dbContext)
    { }

    public IEnumerable<Product> GetProducts(int pageIndex, int pageSize, out int recordCount)
    {
        recordCount = this.DbSet.Count();
        return this.Get(p => true, pageIndex, pageSize, p => p.ReleaseYear, false);
    }

    public Product GetProduct(string productId)
    {
        Guard.ArgumentNotNullOrEmpty(productId, "productId");
        return this.Get(p => p.ProductId == productId).FirstOrDefault();
    }

    public IEnumerable<Product> GetProductsByGenre(string genre, int pageIndex, int pageSize, out int recordCount)
    {
        Guard.ArgumentNotNullOrEmpty(genre, "genre");
        recordCount = this.Count(p => p.Genre.Contains(genre));
        return this.Get<int>(p => p.Genre.Contains(genre), pageIndex, pageSize, p => p.ReleaseYear, false);
    }

    public IEnumerable<Product> GetProductsByActor(string actor, int pageIndex, int pageSize, out int recordCount)
    {
        Guard.ArgumentNotNullOrEmpty(actor, "actor");
        recordCount = this.Count(p => p.Starring.Contains(actor) || p.SupportingActors.Contains(actor));
        return this.Get<int>(p => p.Starring.Contains(actor) || p.SupportingActors.Contains(actor), pageIndex, pageSize, p => p.ReleaseYear, false);
    }
}
}