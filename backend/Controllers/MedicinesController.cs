using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly OnlinemedicineorderingdbContext _context;

        public MedicinesController(OnlinemedicineorderingdbContext context)
        {
            _context = context;
        }

        // GET: api/medicines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetMedicines([FromQuery] int? categoryId, [FromQuery] string? search)
        {
            var query = _context.Medicines.Include(m => m.Category).AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(m => m.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m => m.Name.Contains(search));
            }

            var medicines = await query.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                CategoryId = m.CategoryId,
                CategoryName = m.Category.Name,
                Price = m.Price,
                Description = m.Description,
                Stock = m.Stock,
                IsActive = m.IsActive,
                // Simple placeholder image mapping based on Category Name or generic medical image
                ImageUrl = m.Category.Name == "Antibiotics" ? "https://images.unsplash.com/photo-1576086213369-97a306d36557?auto=format&fit=crop&q=80&w=500" :
                           m.Category.Name == "Painkillers" ? "https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?auto=format&fit=crop&q=80&w=500" :
                           m.Category.Name == "Vitamins" ? "https://images.unsplash.com/photo-1550572017-edb79a0b12ad?auto=format&fit=crop&q=80&w=500" :
                           m.Category.Name == "Supplements" ? "https://images.unsplash.com/photo-1628770737152-4464c2d3ca61?auto=format&fit=crop&q=80&w=500" :
                           m.Category.Name == "Antacids" ? "https://images.unsplash.com/photo-1563223771-6fb769a6dd1c?auto=format&fit=crop&q=80&w=500" :
                           "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?auto=format&fit=crop&q=80&w=500"
            }).ToListAsync();

            return Ok(medicines);
        }

        // GET: api/medicines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineDto>> GetMedicine(int id)
        {
            var m = await _context.Medicines.Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (m == null)
            {
                return NotFound();
            }

            return Ok(new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                CategoryId = m.CategoryId,
                CategoryName = m.Category.Name,
                Price = m.Price,
                Description = m.Description,
                Stock = m.Stock,
                IsActive = m.IsActive,
                ImageUrl = "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?auto=format&fit=crop&q=80&w=500"
            });
        }

        // POST: api/medicines
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MedicineDto>> PostMedicine([FromBody] CreateMedicineDto dto)
        {
            var medicine = new Medicine
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Description = dto.Description,
                Stock = dto.Stock,
                IsActive = dto.IsActive ?? true
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, new MedicineDto
            {
                Id = medicine.Id,
                Name = medicine.Name,
                CategoryId = medicine.CategoryId,
                Price = medicine.Price,
                Description = medicine.Description,
                Stock = medicine.Stock,
                IsActive = medicine.IsActive,
                ImageUrl = "https://images.unsplash.com/photo-1587854692152-cbe660dbde88?auto=format&fit=crop&q=80&w=500"
            });
        }

        // PUT: api/medicines/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicine(int id, [FromBody] CreateMedicineDto dto)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            medicine.Name = dto.Name;
            medicine.CategoryId = dto.CategoryId;
            medicine.Price = dto.Price;
            medicine.Description = dto.Description;
            medicine.Stock = dto.Stock;
            medicine.IsActive = dto.IsActive ?? true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/medicines/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // GET: api/medicines/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.Select(c => new 
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToListAsync();

            return Ok(categories);
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}
