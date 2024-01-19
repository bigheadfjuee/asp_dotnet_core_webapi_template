using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Novell.Directory.Ldap;
using webapi.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly YourProjectContext _context;

    public UserController(YourProjectContext context)
    {
        _context = context;
    }

   #region  REST API 典型的 CRUD 操作
    [HttpGet()]
    public IActionResult List()
    {
        return new JsonResult(_context.Users.ToList<User>());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return new JsonResult(_context.Users.SingleOrDefault(p => p.Id == id));
    }

    [HttpPost]
    public IActionResult Post([FromBody] User entity)
    {
        _context.Users.Add(entity);
        _context.SaveChanges();
        return Get(entity.Id);
    }

    [HttpPut]
    public IActionResult Put([FromBody] User entity)
    {
        var oriEmployee = _context.Users.SingleOrDefault(c => c.Id == entity.Id);
        if (oriEmployee != null)
        {
            _context.Entry(entity).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            return Ok();
        }
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var user = _context.Users.SingleOrDefault(c => c.Id == id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok();
        }
        return BadRequest();
    }
    #endregion
}
