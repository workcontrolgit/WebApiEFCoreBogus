using Nest;
using Newtonsoft.Json;
using OnionApiUpgradeBogus.WebApi.Models;

namespace OnionApiUpgradeBogus.WebApi.Controllers
{
    static public class Import
    {
        static public void ImportJson(IWebHostEnvironment _hostingEnvironment, IElasticClient _elasticClient)
        {
            try
            {
                var rootPath = _hostingEnvironment.ContentRootPath; //get the root path

                var fullPath =
                    Path.Combine(rootPath,
                        "articles.json"); //combine the root path with that of our json file inside mydata directory

                var jsonData = System.IO.File.ReadAllText(fullPath); //read all the content inside the file

                var articleList = JsonConvert.DeserializeObject<List<ArticleModel>>(jsonData);
                if (articleList != null)
                {
                    foreach (var article in articleList)
                    {
                        _elasticClient.IndexDocumentAsync(article);
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }
    }
}
