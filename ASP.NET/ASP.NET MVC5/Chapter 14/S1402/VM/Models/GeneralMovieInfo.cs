using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using VM;

namespace VM.Models
{
public class GeneralMovieInfo
{
    public string ProductId { get; set; }
    [DisplayName("片名")]
    public string Name { get; private set; }
    [DisplayName("类型/流派")]
    public IEnumerable<string> Genre { get; private set; }
    [DisplayName("领衔主演")]
    public IEnumerable<string> Starring { get; private set; }
    [DisplayName("导演")]
    public string Director { get; set; }
    [DisplayName("发行年份")]
    public int ReleaseYear { get; set; }
    [DisplayName("对白")]
    public string Language { get; set; }
    [DisplayName("价格")]
    public decimal Price { get; set; }
    [DisplayName("剧情介绍")]
    public string StoryAbstract { get; set; }
    public string Poster { get; set; }

    public static GeneralMovieInfo FromProduct(Product product)
    {
        return new GeneralMovieInfo
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Genre = product.Genre.Split('|'),
            Starring = product.Starring.Split('|'),
            Director = product.Director,
            ReleaseYear = product.ReleaseYear,
            Language = product.Language,
            Poster = string.Format("~/images/poster/{0}", product.Poster),
            Price = product.Price,
            StoryAbstract = product.StoryAbstract
        };
    }
}
}