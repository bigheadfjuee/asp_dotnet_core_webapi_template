using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Novell.Directory.Ldap;
using webapi.Models;

[Route("api/[controller]")]
[ApiController]
public class UserInfoController : ControllerBase
{
    private readonly ILogger<UserInfoController> _logger;

    public UserInfoController(ILogger<UserInfoController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("WindowsAuth")]
    public IActionResult WindowsAuth()
    {
        var userInfo = new UserInfo { UserAD = User?.Identity?.Name };

        return Ok(new ApiResult<UserInfo>(userInfo));
    }

    [HttpPost]
    [Route("CheckAD")]
    public IActionResult CheckAD([FromBody] UserInfo userInfo)
    {
        // 參考 https://rainmakerho.github.io/2019/09/17/2019025/
        // using Novell.Directory.Ldap;
        var ldapServer = @"khaddc01.kh.asegroup.com";
        // LDAP SSL (TCP 636) and LDAP GC SSL (TCP 3269)
        var ldapConn = new LdapConnection() { SecureSocketLayer = true };

        try
        {
            ldapConn.Connect(ldapServer, LdapConnection.DefaultSslPort);
            // var domainUser = "AD工號@kh.asegroup.com";
            // var domainUser = @"KH\AD工號";
            var domainUser = @$"KH\{userInfo.UserAD}";
            ldapConn.Bind(domainUser, userInfo.Password);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // 400: Invalid Credentials
        }

        var authDN = ldapConn.AuthenticationDn;
        string result = string.IsNullOrEmpty(authDN) ? "ng" : "ok";

        return Ok(new ApiResult<string>(result));
    }

    [HttpPost]
    [Route("Logout")]
    public IActionResult Logout()
    {
        var userInfo = new UserInfo { UserAD = User?.Identity?.Name };

        return Ok(new ApiResult<UserInfo>(userInfo));
    }
}
