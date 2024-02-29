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
public class GetSASEndPoint : EndpointBase
{
  private readonly IWorkSitePhotosService _workSitePhotosService;
  private readonly IConfiguration _configuration;
  private string azureStorageAccount;
  private string azureStorageAccessKey;
  private string blobContainerName;

  public GetSASEndPoint(IWorkSitePhotosService workSitePhotosService, IConfiguration configuration)
  {
    _workSitePhotosService = workSitePhotosService;
    azureStorageAccount = configuration.GetSection("AzureStorage:AzureAccount").Value;
    azureStorageAccessKey = configuration.GetSection("AzureStorage:AccessKey").Value;
    blobContainerName = configuration.GetSection("AzureStorage:blobContainerName").Value;
  }

  [HttpGet("calendar/workphotos/GetSAS")]
  public async Task<ActionResult<string>> HandleAsync(string filename, CancellationToken cancellationToken = default)
  {
    Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
    {
      BlobContainerName = blobContainerName,
      BlobName = filename,
      ExpiresOn = DateTime.UtcNow.AddMinutes(2),//Let SAS token expire after 5 minutes.
    };
    blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);//User will only be able to read the blob and it's properties
    var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureStorageAccount,
        azureStorageAccessKey)).ToString();
    return Ok(sasToken);
  }
}
