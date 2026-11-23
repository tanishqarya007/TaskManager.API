using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Dtos;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
	private readonly AppDbContext _db;
	public TasksController(AppDbContext db) => _db = db;

	private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

	[HttpGet]
	public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll([FromQuery] bool? completed)
	{
		var query = _db.Tasks.Where(t => t.UserId == UserId);
		if (completed.HasValue) query = query.Where(t => t.IsCompleted == completed.Value);
		var items = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
		return Ok(items);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<TaskItem>> GetById(int id)
	{
		var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);
		return item is null ? NotFound() : Ok(item);
	}

	[HttpPost]
	public async Task<ActionResult<TaskItem>> Create(TaskCreateDto dto)
	{
		var item = new TaskItem { Title = dto.Title, Description = dto.Description, UserId = UserId };
		_db.Tasks.Add(item);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
	{
		var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);
		if (item is null) return NotFound();
		item.Title = dto.Title;
		item.Description = dto.Description;
		item.IsCompleted = dto.IsCompleted;
		await _db.SaveChangesAsync();
		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var item = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);
		if (item is null) return NotFound();
		_db.Tasks.Remove(item);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}