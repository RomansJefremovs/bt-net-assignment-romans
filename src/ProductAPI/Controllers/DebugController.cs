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
        // Assuming "DefaultConnection" is the key for your connection string in appsettings.json
        _dbPath = configuration
            .GetConnectionString("DefaultConnection")
            .Replace("Data Source=", "");
    }

    [HttpGet("testdb")]
    public IActionResult TestDatabaseConnection()
    {
        try
        {
            // Check if the database file exists
            var dbExists = System.IO.File.Exists(_dbPath);
            if (!dbExists)
            {
                return NotFound($"Database file not found at path: {_dbPath}");
            }

            // Attempt to open the SQLite database
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                // If no exception is thrown, the database is accessible
                return Ok("Database is accessible and can be connected to.");
            }
        }
        catch (Exception ex)
        {
            // If an exception is thrown, return the exception message
            return Problem(detail: ex.Message, title: "Error when accessing the database.");
        }
    }
}
