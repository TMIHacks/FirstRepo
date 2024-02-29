using AutoMapper;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.JobSchedulesAggregate;
using ConeConnect.Calendar.Core.JSAAggregate;
using ConeConnect.Calendar.Core.WorkPerformedAggregate;
using ConeConnect.Calendar.Core.WorkReceiptAggregate;
using ConeConnect.Calendar.Core.WorkSitePhotosAggregate;
using Microsoft.Azure.Cosmos;

namespace ConeConnect.Calendar.API.Helpers
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<WorkReceiptDTO, WorkReceiptResponse>();
      CreateMap<WorkReceiptLocationDTO, WorkReceiptLocationResponse>();
      CreateMap<ReceiptDispatchCrewDTO, ReceiptDispatchCrewResponse>();
      CreateMap<SignatureAttachmentDTO, SignatureAttachmentResponse>();

      CreateMap<JobSchedulesDTO, JobSchedulesResponse>();
      CreateMap<JobSchedulesCrewDTO, JobSchedulesResponse>();
      CreateMap<DispatchStatusDTO, DispatchStatusResponse>();

      CreateMap<JobSchedulesLocationsDTO, JobScheduleLocationResponse>();

      CreateMap<WorkSitePhotosDTO, WorkSitePhotosResponse>();
      CreateMap<WorkSitePhotosLocationDTO, WorkSitePhotosLocationResponse>();
      CreateMap<WorkSitePhotosListDTO, WorkSitePhotosListResponse>();

      CreateMap<WorkPerformedLocationDTO, WorkPerformedResponse>();
      CreateMap<WorkPerformedServiceDTO, WorkPerformedServiceResponse>();

      CreateMap<JSALocations, JSARQRS>();
      CreateMap<JSARQRS, JSALocations>();

      CreateMap<DispatchCrewDTO, JobSchedulesCrewDTO>();
      CreateMap<JobSchedulesCrewDTO, DispatchCrewDTO>();

      CreateMap<DispatchDetailsDTO, JobSchedulesDTO>();
      CreateMap<JobSchedulesDTO, DispatchDetailsDTO>();
    }
  }
}
