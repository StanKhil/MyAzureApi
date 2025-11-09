using MyAzureApi.Entities;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using MyAzureApi.Dto;

namespace MyAzureApi.Controllers
{
    [Route("api/table-storage")]
    [ApiController]
    public class TableStorageController : ControllerBase
    {
        private readonly string connectionString;

        public TableStorageController(IConfiguration configuration)
        {
            connectionString = configuration["azurestorageconnection"];
        }
        private readonly string tableName = "students";

        private readonly TableClient _table;

        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var serviceClient = new TableServiceClient(connectionString);

                var tableClient = serviceClient.GetTableClient(tableName);

                await tableClient.CreateIfNotExistsAsync();

                var students = tableClient.Query<StudentEntity>().ToList();

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewStudent([FromBody] StudentDto dto)
        {
            try
            {
                var serviceClient = new TableServiceClient(connectionString);
                var tableClient = serviceClient.GetTableClient(tableName);

                await tableClient.CreateIfNotExistsAsync();

                var student = new StudentEntity
                {
                    PartitionKey = "students",
                    RowKey = Guid.NewGuid().ToString(),
                    Name = dto.Name,
                    Grade = dto.Grade
                };

                await tableClient.AddEntityAsync(student);

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error {ex.Message}");
            }
        }

    }
}
