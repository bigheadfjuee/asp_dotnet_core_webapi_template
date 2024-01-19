namespace webapi.Models;
public class ApiResult<T>
{
  public string? ErrorCode { get; set; }
  public string? Message { get; set; }
  public DateTime? DataTime { get; set; }
  public T? Data { get; set; }

  public ApiResult() { }

  public ApiResult(T data)
  {
    ErrorCode = "0000";
    DataTime = DateTime.Now;
    Data = data;
  }
}

public class ApiError : ApiResult<object>
{
  public ApiError(string code, string message)
  {
    ErrorCode = code;
    this.DataTime = DateTime.Now;
    Message = message;
  }
}