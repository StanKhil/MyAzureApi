using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MyAzureApi.Controllers
{
    [Route("api/queue-storage")]
    [ApiController]
    public class QueueStorageController : ControllerBase
    {
        private readonly string connectionString;

        public QueueStorageController(IConfiguration configuration)
        {
            connectionString = configuration["azurestorageconnection"];
        }

        private readonly string queueName = "messages";


        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            try
            {
                var queueClient = new QueueClient(connectionString, queueName);
                await queueClient.CreateIfNotExistsAsync();

                if (!await queueClient.ExistsAsync())
                    return StatusCode(500, "Not found");

                await queueClient.SendMessageAsync(message);

                return Ok(new { message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("receive")]
        public async Task<IActionResult> ReceiveMessage()
        {
            try
            {
                var queueClient = new QueueClient(connectionString, queueName);
                await queueClient.CreateIfNotExistsAsync();

                var message = await queueClient.ReceiveMessageAsync();
                if (message.Value != null)
                {
                    await queueClient.DeleteMessageAsync(message.Value.MessageId, message.Value.PopReceipt);
                    return Ok(message.Value.MessageText);
                }
                return Ok("Queue is empty.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
