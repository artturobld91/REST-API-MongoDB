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
using RestAPINetMongoDB.Models;
using RestAPINetMongoDB.Services;

namespace RestAPINetMongoDB.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/MongoDBOps")]
    public class MongoDBOpsController : ControllerBase
    {
        private readonly ILogger<MongoDBOpsController> _logger;
        private readonly IConfiguration _configuration; // To read from configuration file
        private readonly MongoDBService _mongoDBService;

        public MongoDBOpsController(ILogger<MongoDBOpsController> logger,
                                    IConfiguration configuration,
                                    MongoDBService mongoDBService)
        {
            _logger = logger;
            _configuration = configuration;
            _mongoDBService = mongoDBService;
        }

        // TODO: Create method to delete document
        // TODO: Create method to update document


        [HttpGet("GetDatabases")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCollection()
        {
            try
            {
                var client = _mongoDBService._mongoClient;

                IAsyncCursor<string> databases = await client.ListDatabaseNamesAsync();
                List<string> databasesList = await databases.ToListAsync();

                return Ok(new { ok = true, databases = databasesList });
            }
            catch (Exception ex)
            {
                return NotFound(new { ok = false, error = ex.Message });
            }
        }

        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var client = _mongoDBService._mongoClient;
                    var mongoDatabase = client.GetDatabase("hospitaldb");
                    var usersCollection = mongoDatabase.GetCollection<Users>("users");

                    var user = await usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

                    return Ok(user);
                }
                else
                {
                    return BadRequest(new { ok = false, error = "User document is empty" });
                }
            }
            catch (Exception ex)
            {
                return NotFound(new { ok = false, error = ex.Message });
            }
        }


        [HttpPost("CreateDocument")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDocument(Users user)
        {
            try
            {
                if (user != null)
                {
                    var client = _mongoDBService._mongoClient;
                    var mongoDatabase = client.GetDatabase("hospitaldb");
                    var usersCollection = mongoDatabase.GetCollection<Users>("users");

                    await usersCollection.InsertOneAsync(user);

                    var newUser = await usersCollection.Find(x => x.email == user.email).FirstOrDefaultAsync();

                    return new CreatedAtRouteResult("GetUser", new { id = user.Id }, newUser);
                }
                else
                {
                    return BadRequest(new { ok = false, error = "User document is empty" });
                }
                

            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDocument(string id, Users user)
        {
            try
            {
                if (user != null)
                {
                    var client = _mongoDBService._mongoClient;
                    var mongoDatabase = client.GetDatabase("hospitaldb");
                    var usersCollection = mongoDatabase.GetCollection<Users>("users");

                    var newUser = await usersCollection.ReplaceOneAsync(x => x.Id == id, user);

                    return NoContent();
                }
                else
                {
                    return BadRequest(new { ok = false, error = "User document is empty" });
                }


            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var client = _mongoDBService._mongoClient;
                    var mongoDatabase = client.GetDatabase("hospitaldb");
                    var usersCollection = mongoDatabase.GetCollection<Users>("users");

                    await usersCollection.DeleteOneAsync(x => x.Id == id);

                    return NoContent();
                }
                else
                {
                    return BadRequest(new { ok = false, error = "User document is empty" });
                }


            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, error = ex.Message });
            }
        }

    }
}
