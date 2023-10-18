using AssessmentAEM.Data;
using AssessmentAEM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssessmentAEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformWell : ControllerBase
    {
        private readonly PlatfromDbContext _context;

        public PlatformWell(PlatfromDbContext context) => _context = context;
        [HttpGet("GetPlatformWellActual")]
        public async Task<IEnumerable<Platform>> GetPlatformWellActual()
        {
            var platforms = _context.Platform.Include(p => p.Well).ToList();
            return platforms;
            //return await _context.Platform.ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> PostData(Platform data)
        {
            if (data == null)
            {
                return BadRequest("Invalid data.");
            }

            // Check if the Platform exists
            var existingPlatform = await _context.Platform.FirstOrDefaultAsync(p => p.id == data.id);

            if (existingPlatform == null)
            {
                // If the Platform doesn't exist, insert a new one
                _context.Platform.Add(data);
            }
            else
            {
                // If the Platform exists, update it
                existingPlatform.uniqueName = data.uniqueName;
                existingPlatform.latitude = data.latitude;
                existingPlatform.longitude = data.longitude;
                existingPlatform.createdAt = data.createdAt;
                existingPlatform.updatedAt = data.updatedAt;
            }

            foreach (var well in data.Well)
            {

                // Check if the Well exists
                var existingWell = await _context.Well.FirstOrDefaultAsync(w => w.id == well.id);

                if (well.id == 0)
                {
                    // If the Well doesn't exist, insert a new one
                    _context.Well.Add(well);
                }
                else
                {
                    // If the Well exists, update it
                    existingWell.uniqueName = well.uniqueName;
                    existingWell.latitude = well.latitude;
                    existingWell.longitude = well.longitude;
                    existingWell.createdAt = well.createdAt;
                    existingWell.updatedAt = well.updatedAt;
                }
            }

            //// Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(data.Well);
        }
    }
}
