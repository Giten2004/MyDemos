using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VM.Services;
using VM.Models;
using VM.Mvc.Extensions;
using VM;

namespace VM.Controllers
{
    public class ProductController : VmController
    {
        /// <summary>
        /// 用于获取商品信息的ProductService
        /// </summary>
        /// <remarks>
        /// 1、属性类型是接口IProductService
        /// 2、属性通过构造器注入（Constructor Injection）的方式进行初始化
        /// </remarks>
        public IProductService ProductService { get; private set; }
        public ProductController(IProductService productService)
        {
            this.ProductService = productService;
            this.AddDisposableObject(productService);
        }

        /// <summary>
        /// 显示所有影片列表（分页）
        /// </summary>      
        public ActionResult Index(int pageIndex = 1)
        {
            int recordCount;
            IEnumerable<GeneralMovieInfo> movies = this.ProductService.GetMovies(pageIndex, PagingInfo.PageSize, out recordCount)
                .Select(p => GeneralMovieInfo.FromProduct(p));
            Func<int, UrlHelper, string> pageUrlAccessor = (currentPage, helper) => helper.RouteUrl("Page", new { PageIndex = currentPage }).ToString();
            ViewBag.Title = "Video Mall";
            return RenderMovieList(movies, recordCount, pageIndex, pageUrlAccessor);
        }

        /// <summary>
        /// 显示由指定演员参演的影片列表（分页）
        /// </summary>
        public ActionResult Actor(string actor, int pageIndex = 1)
        {
            int recordCount;
            IEnumerable<GeneralMovieInfo> movies = this.ProductService.GetMoviesByActor(actor, pageIndex, PagingInfo.PageSize, out recordCount)
                .Select(p => GeneralMovieInfo.FromProduct(p));
            Func<int, UrlHelper, string> pageUrlAccessor = (currentPage, helper) => helper.RouteUrl("ActorPage", new { PageIndex = currentPage }).ToString();
            ViewBag.Title = actor;
            return RenderMovieList(movies, recordCount, pageIndex, pageUrlAccessor);
        }

        /// <summary>
        /// 显示由指定类型的影片列表（分页）
        /// </summary>
        public ActionResult Genre(string genre, int pageIndex = 1)
        {
            int recordCount;
            IEnumerable<GeneralMovieInfo> movies = this.ProductService.GetMoviesByGenre(genre, pageIndex, PagingInfo.PageSize, out recordCount)
                    .Select(p => GeneralMovieInfo.FromProduct(p));
            Func<int, UrlHelper, string> pageUrlAccessor = (currentPage, helper) => helper.RouteUrl("GenrePage", new { PageIndex = currentPage }).ToString();
            ViewBag.Title = genre;
            return RenderMovieList(movies, recordCount, pageIndex, pageUrlAccessor);
        }

        /// <summary>
        /// 显示指定影片详细信息
        /// </summary>
        public ActionResult Detail(string productId)
        {
            Product product = this.ProductService.GetMovie(productId);
            if (null == product)
            {
                return new HttpNotFoundResult(string.Format("指定的产品\"{0}\"不存在", productId));
            }
            return View(MovieInfo.FromProduct(product));
        }

        private ActionResult RenderMovieList(IEnumerable<GeneralMovieInfo> movies, int recordCount, int pageIndex,
            Func<int, UrlHelper, string> pageUrlAccessor)
        {
            ViewResult result = View("MovieList", movies);
            ViewBag.RecordCount = recordCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageUrlAccessor = pageUrlAccessor;
            return result;
        }
    }
}