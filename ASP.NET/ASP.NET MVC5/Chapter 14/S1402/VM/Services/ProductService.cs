using System;
using System.Collections.Generic;
using System.Linq;
using VM.Models;
using VM.Repositories;
using Microsoft.Practices.Unity.Utility;
using VM;

namespace VM.Services
{
public class ProductService : ServiceBase,IProductService
{
    //Repository都采用构造器注入的方式进行初始化
    public IProductRepository ProductRepository { get; private set; }
    public ProductService(IProductRepository productRepository)
    {
        this.ProductRepository = productRepository;
        this.AddDisposableObject(productRepository);
    }

    public IEnumerable<Product> GetMovies(int pageIndex, int pageSize, out int recordCount)
    {
        return this.ProductRepository.GetProducts(pageIndex, pageSize, out recordCount);
    }
    public IEnumerable<Product> GetMoviesByGenre(string genre, int pageIndex, int pageSize, out int recordCount)
    {
        Guard.ArgumentNotNullOrEmpty(genre, "genre");
        return this.ProductRepository.GetProductsByGenre(genre, pageIndex, pageSize, out recordCount);
    }
    public IEnumerable<Product> GetMoviesByActor(string actor, int pageIndex, int pageSize, out int recordCount)
    {
        Guard.ArgumentNotNullOrEmpty(actor, "actor");
        return this.ProductRepository.GetProductsByActor(actor, pageIndex, pageSize, out recordCount);
    }

    public Product GetMovie(string productId)
    {
        Guard.ArgumentNotNullOrEmpty(productId, "productId");
        return this.ProductRepository.GetProduct(productId);
    }
    public int GetStock(string productId)
    {
        Guard.ArgumentNotNullOrEmpty(productId, "productId");
        return this.ProductRepository.GetProduct(productId).Stock;
    }
}
}