using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.WorkSitePhotosAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkSitePhotosEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AddWorkSitePhotosWithFileEndPoint : EndpointBaseAsync.WithRequest<WorkSitePhotosRequestWithFile>.WithActionResult<WorkSitePhotosResponse>
{
  private readonly IWorkSitePhotosService _workSitePhotosService;

  public AddWorkSitePhotosWithFileEndPoint(IWorkSitePhotosService workSitePhotosService)
  {
    _workSitePhotosService = workSitePhotosService;
  }

  [HttpPost("calendar/addworkphotoswithfile")]
  public override async Task<ActionResult<WorkSitePhotosResponse>> HandleAsync([FromForm] WorkSitePhotosRequestWithFile photosRequest, CancellationToken cancellationToken = default)
  {

    var errors = new List<ValidationError>();

    if (photosRequest == null)
    {
      errors.Add(new() { Identifier = nameof(photosRequest), ErrorMessage = $"Invalid Request Data" });

      return BadRequest(Result<WorkSitePhotosResponse>.Invalid(errors));
    }

    if (photosRequest.Location == null
      || photosRequest.Location.GeoLat == null
      || photosRequest.Location.GeoLon == null
      || photosRequest.Location.GeoLat == 0
      || photosRequest.Location.GeoLon == 0)
    {
      errors.Add(new() { Identifier = nameof(photosRequest.Location), ErrorMessage = $"Location coordinates are required." });

      return BadRequest(Result<WorkSitePhotosResponse>.Invalid(errors));
    }

    WorkSitePhotosRequest workSitePhotos = new WorkSitePhotosRequest();

    workSitePhotos.Location = photosRequest.Location;
    workSitePhotos.SitePhotos = new List<WorkSitePhotosListRequest>();

    if (photosRequest.SitePhotos != null)
    {
      foreach (var photoFile in photosRequest.SitePhotos)
      {
        if (photoFile.IsRemoved == false)
        {
          if (photoFile.File != null)
          {
            var content = await _workSitePhotosService.UploadPhotosOnStorage(photoFile.File);

            if (content != null)
            {
              WorkSitePhotosListRequest workSitePhoto = new WorkSitePhotosListRequest
              {
                Type = content.Extension,
                URL = content.Location,
                FileName = content.Name,
                Category = photoFile.Category,
                IsRemoved = photoFile.IsRemoved,
              };

              workSitePhotos.SitePhotos.Add(workSitePhoto);
            }
          }
          else
          {
            WorkSitePhotosListRequest workSitePhoto = new WorkSitePhotosListRequest
            {
              Type = photoFile.Extension,
              URL = photoFile.Location,
              FileName = photoFile.Name,
              Category = photoFile.Category,
              IsRemoved = photoFile.IsRemoved,
            };

            workSitePhotos.SitePhotos.Add(workSitePhoto);
          }
        }
        else
        {
          WorkSitePhotosListRequest workSitePhoto = new WorkSitePhotosListRequest
          {
            URL = photoFile.URL,
            Category = photoFile.Category,
            IsRemoved = photoFile.IsRemoved,
          };

          workSitePhotos.SitePhotos.Add(workSitePhoto);
        }
      }
    }

    var result = await _workSitePhotosService.AddWorkSitePhotos(workSitePhotos);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
