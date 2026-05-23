using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcApi.Data;
using PcApi.DTOs;
using PcApi.Models;

namespace PcApi.Controllers;

[ApiController]
[Route("api/pcs")]
public class PCsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PCsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PCResponseDto>>> GetAll()
    {
        var pcs = await _context.PCs
            .Select(pc => new PCResponseDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            })
            .ToListAsync();

        return Ok(pcs);
    }

    [HttpGet("{id}/components")]
    public async Task<IActionResult> GetComponents(int id)
    {
        var pc = await _context.PCs
            .Include(p => p.PCComponents)
                .ThenInclude(pc => pc.Component)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null)
            return NotFound();

        return Ok(pc);
    }

    [HttpPost]
    public async Task<ActionResult<PCResponseDto>> Create(CreatePCDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetComponents), new { id = pc.Id },
            new PCResponseDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePCDto dto)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc == null)
            return NotFound();

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc == null)
            return NotFound();

        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}