using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celesticalObject = _context.CelestialObjects.Find(id);

            if (celesticalObject == null)
                return NotFound();
            celesticalObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
            return Ok(celesticalObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celesticalObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (!celesticalObjects.Any())
                return NotFound();

            foreach (var celesticalObject in celesticalObjects)
            {
                celesticalObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celesticalObject.Id).ToList();
            }
            return Ok(celesticalObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celesticalObjects = _context.CelestialObjects.ToList();
            foreach (var celesticalObject in celesticalObjects)
            {
                celesticalObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celesticalObject.Id).ToList();
            }
            return Ok(celesticalObjects);
        }

    }
}
