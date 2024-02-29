namespace ConeConnect.Calendar.API.Models;

public class ResponseView<T>
{
  public List<T> ResponseList { get; set; }
  public T ResponseObject { get; set; }
  public string Message { get; set; }

  public bool IsSuccess
  {
    get
    {
      return true;
    }
  }
}
