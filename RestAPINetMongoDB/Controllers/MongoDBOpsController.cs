using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using RestAPINetMongoDB.Configuration;

namespace RestAPINetMongoDB.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/MongoDBOps")]
    public class MongoDBOpsController : ControllerBase
    {
        private readonly ILogger<MongoDBOpsController> _logger;
        private readonly IConfiguration _configuration; // To read from configuration file

        public MongoDBOpsController(ILogger<MongoDBOpsController> logger,
                                   IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // TODO: Replace connection string with user secrets
        // TODO: Create method to create document
        // TODO: Create method to delete document
        // TODO: Create method to update document
        // TODO: Create method to get document
        // TODO: Check how to create service and use dependency injection to manage connections to MongoDB


        [HttpGet("GetDatabases")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCollection()
        {
            try
            {
                var connectionStrings = _configuration.GetSection("ConnectionStrings").Get<ConnectionStringsSettings>();
                // Replace the uri string with your MongoDB deployment's connection string.
                var client = new MongoClient(connectionStrings.MongoDB);

                IAsyncCursor<string> databases = await client.ListDatabaseNamesAsync();
                List<string> databasesList = await databases.ToListAsync();

                return Ok(new { ok = true, databases = databasesList });
            }
            catch (Exception ex)
            {
                return NotFound(new { ok = false, error = ex.Message });
            }
        }
    }
}
