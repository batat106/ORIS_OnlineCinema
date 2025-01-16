using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.HttpResponce;
using Microsoft.Data.SqlClient;
using MyHTTPServer.models;
using MyHTTPServer.Sessions;
using MyORMLibrary;


namespace MyHTTPServer.EndPoints;

public class CatalogEndPoint : BaseEndPoint
{
    private readonly ORMContext<Movies> _dbContext;

    public CatalogEndPoint()
    {
        string connectionString =
            @"Server=localhost; Database=filmsDB; User Id=sa; Password=P@ssw0rd;TrustServerCertificate=true;";
        var connection = new SqlConnection(connectionString);
        _dbContext = new ORMContext<Movies>(connection);
    }

    [Get("catalog")]
    public IHttpResponceResult GetMoviesPage()
    {
        if (!SessionStorage.IsAuthorized(Context))
        {
            return Redirect("login");
        }

        var filePath = @"Templates\Pages\Catalog\index.html";
        return Html(File.ReadAllText(filePath));
    }

    [Post("catalog/filtered")]
    public IHttpResponceResult PostMoviesPage(string genre)
    {
        Console.WriteLine($"GENRE: {genre}");
        if (genre == "all")
            return Redirect("http://localhost:6529/catalog");


        var films = _dbContext.GetByAll().Where(x => x.genre == Translator(genre));

        var templatePath = @"Templates\Pages\Catalog\CatalogSorted.html";
        if (!File.Exists(templatePath))
        {
            return Html("<h1>Template was not found</h1>");
        }

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


    public static string Translator(string genre)
    {
        switch (genre)
        {
            case "drama":
                return "Драма";
            case "comedy":
                return "Комедия";
            case "music":
                return "Музикл";
            default:
                return null;
        }
    }
}