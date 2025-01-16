using System.Net;
using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.HttpResponce;
using Microsoft.Data.SqlClient;
using MyHTTPServer.models;
using MyHTTPServer.Sessions;
using MyORMLibrary;


namespace MyHTTPServer.EndPoints;

public class AdminEndPoint : BaseEndPoint
{
    private readonly ORMContext<Movies> _dbContext;

    public AdminEndPoint()
    {
        string connectionString =
            @"Server=localhost; Database=filmsDB; User Id=sa; Password=P@ssw0rd;TrustServerCertificate=true;";
        var connection = new SqlConnection(connectionString);
        _dbContext = new ORMContext<Movies>(connection);
    }

    // Admin Login
    [Get("admin")]
    public IHttpResponceResult GetAdminPage()
    {
        var filePath = @"Templates\Pages\Admin\AdminLogin.html";
        return Html(File.ReadAllText(filePath));
    }

    [Post("admin")]
    public IHttpResponceResult PostAdminLogin(string password)
    {
        if (password != "123")
            Redirect("admin");

        string token = Guid.NewGuid().ToString();
        Cookie cookie = new Cookie("session-token", token);
        Context.Response.SetCookie(cookie);

        SessionStorage.SaveSession(token, "admin");

        return Redirect("panel");
    }


    [Get("panel")]
    public IHttpResponceResult GetAdminPanel()
    {
        if (!SessionStorage.IsAuthorized(Context))
        {
            return Redirect("login");
        }

        var filePath = @"Templates\Pages\Admin\AdminPanel.html";
        return Html(File.ReadAllText(filePath));
    }

    [Post("panel")]
    public IHttpResponceResult PostAdminPanel(string title, string year, string rating, string description,
        string poster_url)
    {
        _dbContext.ExecuteQuerySingle(
            $"INSERT INTO Movies(title, year, rating, description, poster_url) VALUES ('{title}', '{year}', '{rating}', '{description}', '{poster_url}')");  //('{title}', {int.Parse(year)}, {float.Parse(rating)}, '{description}', '{poster_url}')");
        return Redirect("panel");
    }

    [Post("panel/del")]
    public IHttpResponceResult PostPanel(string id)
    {
        try
        {
            _dbContext.ExecuteQuerySingle($"DELETE FROM Movies WHERE id = {int.Parse(id)}");
            return Redirect("../panel");
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return Html($"Error occured: {e.Message}");
        }
    }
}