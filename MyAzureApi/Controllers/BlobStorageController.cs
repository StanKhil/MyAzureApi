using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MyAzureApi.Controllers
{
    [Route("api/blob-storage")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly string connectionString;

        public BlobStorageController(IConfiguration configuration)
        {
            connectionString = configuration["azurestorageconnection"];
        }

        private readonly string containerName = "blobfiles"; 

       
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Non found");

            try
            {
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobClient = containerClient.GetBlobClient(file.FileName);

                await using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                return Ok(new { fileName = file.FileName, url = blobClient.Uri.ToString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpGet("files")]
        public async Task<IActionResult> GetFiles()
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                var files = new List<string>();
                await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                {
                    files.Add(blobItem.Name);
                }

                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return BadRequest("Not found");

            try
            {
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(filename);

                var downloadInfo = await blobClient.DownloadContentAsync();
                var contentType = downloadInfo.Value.Details.ContentType ?? "application/octet-stream";
                var fileBytes = downloadInfo.Value.Content.ToArray();

                return File(fileBytes, contentType, filename);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
