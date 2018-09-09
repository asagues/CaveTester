using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.Example.Aperture
{
    public class TurretService
    {
        private readonly ApertureContext _context;

        public TurretService(ApertureContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Remove all defective turrets from database
        /// </summary>
        public async Task RemoveAllDefectiveTurrets()
        {
            foreach (var turret in await _context.Turrets.ToListAsync())
            {
                if (turret.IsDefective)
                    _context.Remove(turret);
            }

            await _context.SaveChangesAsync();
        }
    }
}