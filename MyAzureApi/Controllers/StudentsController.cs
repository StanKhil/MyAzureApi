using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAzureApi.Data;
using MyAzureApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace MyAzureApi.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DBContext _context;
        public StudentsController(DBContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _context.Students.ToListAsync());
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student s)
        {
            _context.Students.Add(s);
            await _context.SaveChangesAsync();
            return Ok(s);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _context.Students.FindAsync(id);
            if (s == null) return NotFound();
            _context.Students.Remove(s);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
