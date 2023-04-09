using Microsoft.AspNetCore.Mvc;

namespace asp_dotnet_core_webapi_template.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
  private readonly ILogger<UsersController> _logger;

  public UsersController(ILogger<UsersController> logger)
  {
    _logger = logger;
  }

  [HttpGet(Name = "GetUsers")]
  public IActionResult Get()
  {
    return Ok("Hello");
  }
}
