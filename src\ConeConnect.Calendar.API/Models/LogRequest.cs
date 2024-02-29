using Newtonsoft.Json;

namespace ConeConnect.Calendar.API.Models;

public class LogRequest
{
  [JsonProperty(PropertyName = "id")]
  public string? Id { get; set; }
  public string ControllerName { get; set; }
  public string Model { get; set; }
  public string Type { get; set; }
}
