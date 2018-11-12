using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using VM;

namespace VM.Models
{
public class MovieInfo
{
    public string ProductId { get; private set; }
    [DisplayName("片名")]
    public string Name { get; private set; }
    [DisplayName("类型")]
    public IEnumerable<string> Genre { get; private set; }
    [DisplayName("领衔主演")]
    public IEnumerable<string> Starring { get; private set; }
    [DisplayName("主演")]
    public IEnumerable<string> SupportingActors { get; private set; }
    [DisplayName("导演")]
    public string Director { get; private set; }
    [DisplayName("编剧")]
    public string ScriptWriter { get; private set; }
    [DisplayName("制片国家")]
    public string ProductionCountry { get; private set; }
    [DisplayName("制片公司")]
    public string ProductionCompany { get; private set; }
    [DisplayName("发行年份")]
    public int ReleaseYear { get; private set; }
    [DisplayName("对白")]
    public string Language { get; private set; }
    [DisplayName("片长")]
    public int RunTime { get; private set; }
    [DisplayName("价格")]
    public decimal Price { get; private set; }
    [DisplayName("剧情介绍")]
    public string Story { get; private set; }
    public string Poster { get; private set; }

    public static MovieInfo FromProduct(Product product)
    {
        return new MovieInfo
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Genre = product.Genre.Split('|'),
            Starring = product.Starring.Split('|'),
            SupportingActors = product.SupportingActors.Split('|'),
            Director = product.Director,
            ScriptWriter = product.ScriptWriter,
            ProductionCountry = product.ProductionCountry,
            ProductionCompany = product.ProductionCompany,
            ReleaseYear = product.ReleaseYear,
            Language = product.Language,
            Poster = string.Format("~/images/poster/{0}", product.Poster),
            RunTime = product.RunTime,
            Price = product.Price,
            Story = product.Story
        };
    }
}
}