using Ardalis.ApiEndpoints;
using Azure.Storage;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace ConeConnect.Calendar.API.Controllers.WorkSitePhotosEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetSASForUploadEndPoint : EndpointBase
{
  private readonly IWorkSitePhotosService _workSitePhotosService;
  private readonly IConfiguration _configuration;
  private string azureStorageAccount;
  private string azureStorageAccessKey;
  private string blobContainerName;
  private string blobConnectionString;

  public GetSASForUploadEndPoint(IWorkSitePhotosService workSitePhotosService, IConfiguration configuration)
  {
    _workSitePhotosService = workSitePhotosService;
    azureStorageAccount = configuration.GetSection("AzureStorage:AzureAccount").Value;
    azureStorageAccessKey = configuration.GetSection("AzureStorage:AccessKey").Value;
    blobContainerName = configuration.GetSection("AzureStorage:blobContainerName").Value;
    blobConnectionString = configuration.GetSection("BlobStorageSettings:BlobConnectionString").Value;
  }

  [HttpGet("calendar/workphotos/GetSASForUploadFile")]
  public async Task<ActionResult<string>> HandleAsync(string blobName, CancellationToken cancellationToken = default)
  {
    Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder();
   
    blobSasBuilder.SetPermissions("wactfx");
    blobSasBuilder.StartsOn = DateTime.UtcNow;
    blobSasBuilder.Snapshot = "bf";
    blobSasBuilder.Protocol = SasProtocol.Https;
    blobSasBuilder.BlobVersionId = "2022-11-02";
    blobSasBuilder.BlobContainerName = blobContainerName;
    blobSasBuilder.BlobName = blobName;
    blobSasBuilder.ExpiresOn = DateTime.UtcNow.AddMinutes(15);
    blobSasBuilder.Resource = "b";
    
    var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureStorageAccount,
        azureStorageAccessKey)).ToString();

    BlobServiceClient blobServiceClient = new BlobServiceClient(blobConnectionString);
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
    string blobSasUri = containerClient.Uri.ToString() + "?" + sasToken;
    return Ok(blobSasUri);
  }
}
