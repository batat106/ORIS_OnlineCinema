using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.HttpResponce;
using Microsoft.Data.SqlClient;
using MyHTTPServer.models;
using MyORMLibrary;

namespace MyHTTPServer.EndPoints;

public class TemplatorEndPoint : BaseEndPoint
{
    private readonly ORMContext<Movies> _dbContext;

    public TemplatorEndPoint()
    {
        string connectionString =
            @"Server=localhost; Database=filmsDB; User Id=sa; Password=P@ssw0rd;TrustServerCertificate=true;";
        var connection = new SqlConnection(connectionString);
        _dbContext = new ORMContext<Movies>(connection);
    }

    [Get("templator")]
    public IHttpResponceResult GetTemplate()
    {
        var films = _dbContext.GetByAll();

        var templatePath = @"Templates\Pages\Catalog\CatalogTemplate.html";

        var template = File.ReadAllText(templatePath);
        var templateEngine = new TemplateEngine.TemplateEngine();
        var data = new
        {
            Items = films.Select(f => new
            {
                f.id,
                f.title,
                f.year,
                f.description,
                f.rating,
                f.duration,
                f.country,
                f.studio,
                f.genre,
                f.poster_url
            }).ToList()
        };

        var htmlContent = templateEngine.Render(template, data);
        return Html(htmlContent);
    }
    
    
}