using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

[Route("api/[controller]")]
[ApiController]
public class DebugController : ControllerBase
{
    private readonly string _dbPath;

    public DebugController(IConfiguration configuration)
    {
        _dbPath = configuration
            .GetConnectionString("DefaultConnection")
            .Replace("Data Source=", "");
    }

    [HttpGet("testdb")]
    public IActionResult TestDatabaseConnection()
    {
        try
        {
            var dbExists = System.IO.File.Exists(_dbPath);
            if (!dbExists)
            {
                return NotFound($"Database file not found at path: {_dbPath}");
            }
            
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                return Ok("Database is accessible and can be connected to.");
            }
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Error when accessing the database.");
        }
    }
}
