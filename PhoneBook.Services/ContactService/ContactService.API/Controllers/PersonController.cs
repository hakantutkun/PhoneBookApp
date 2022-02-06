using ContactService.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonService.Core.Models;

namespace ContactService.API.Controllers
{
    /// <summary>
    /// Person Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PersonController : Controller
    {
        #region Members

        /// <summary>
        /// Context Object
        /// </summary>
        private readonly ContactServiceContext _context;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Received context with DI</param>
        public PersonController(ContactServiceContext context)
        {
            // Inject received context
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all from database
        /// </summary>
        [HttpGet]
        public async Task<List<Person>> GetAllAsync()
        {
            // Get all 
            return await _context.Persons.Include(p =>p.ContactInfo).ToListAsync();
        }

        /// <summary>
        /// Fetches only one person from database
        /// </summary>
        /// <param name="id">Requested person's id</param>
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> GetOneByIdAsync(string id)
        {
            // Check if received id is null
            if (id == null)
            {
                return NotFound();
            }

            // Get requested person from database
            var person = await _context.Persons.Include(p => p.ContactInfo).FirstOrDefaultAsync(m => m.Id == id);

            // Check if person was found
            if (person == null)
            {
                return NotFound();
            }

            // Return the fetched person
            return Ok(person);
        }

        /// <summary>
        /// Fetches only one person from database
        /// </summary>
        /// <param name="id">Requested person's id</param>
        [HttpGet]
        [Route("Info/{Id}")]
        public async Task<IActionResult> GetOneInfoByIdAsync(int id)
        {
            // Check if received id is null
            if (id == null)
            {
                return NotFound();
            }

            // Get requested person from database
            var info = await _context.ContactInfos.FirstOrDefaultAsync(m => m.Id == id);

            // Check if person was found
            if (info == null)
            {
                return NotFound();
            }

            // Return the fetched person
            return Ok(info);
        }

        /// <summary>
        /// Creates a new person with contact infos.
        /// </summary>
        /// <param name="person">Received person object</param>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Person person)
        {
            // Check if model state is valid
            if (ModelState.IsValid && person.ContactInfo != null)
            {
                // Create a new guid
                person.Id = Guid.NewGuid().ToString();

                // Add person to db
                await _context.AddAsync(person);

                foreach (var info in person.ContactInfo)
                {
                    // Set info id 
                    info.PersonId = person.Id;

                    // Add info to the table
                    await _context.ContactInfos.AddAsync(info);
                }

                // Save Changes
                await _context.SaveChangesAsync();

                // Return created person
                return Created("",person);
            }

            // If model state is not valid, return bad request.
            return BadRequest();
        }

        /// <summary>
        /// Updates requested person.
        /// </summary>
        /// <param name="id">Uid of person that will be updated</param>
        /// <param name="person">Changed person object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] Person person)
        {
            // Check if received id is the same with person's id
            if (id != person.Id)
            {
                return NotFound();
            }

            // Check if model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Update requested person
                    _context.Update(person);

                    // Save db changes
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if person exist in database
                    var p = await _context.Persons.FindAsync(person.Id);

                    if (p == null)
                    {
                        return NotFound();
                    }
                }

                // If updated
                return NoContent();
            }

            // If update process failed
            return BadRequest();
        }

        /// <summary>
        /// Deletes requested person.
        /// </summary>
        /// <param name="id">Requested person id.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            // Find person
            var person = await _context.Persons.FindAsync(id);

            // Check if person found.
            if(person == null)
                return NotFound();

            // Remove contact infos first
            var infos = await _context.ContactInfos.Where(i => i.PersonId == id).ToListAsync();

            foreach (var info in infos)
            {
                _context.ContactInfos.Remove(info);
            }

            // remove person
            _context.Persons.Remove(person);

            // Save db changes
            await _context.SaveChangesAsync();
            
            // Return
            return NoContent();
        }

        /// <summary>
        /// Creates requested contact info.
        /// </summary>
        /// <param name="infoId">Requested info id.</param>
        [HttpPost("Info/")]
        public async Task<IActionResult> CreateInfoAsync(ContactInfo contactInfo)
        {
            // add contact info
            await _context.ContactInfos.AddAsync(contactInfo);

            // Save Changes
            await _context.SaveChangesAsync();

            // Return created person
            return Created("", contactInfo);
        }

        /// <summary>
        /// Deletes requested contact info.
        /// </summary>
        /// <param name="infoId">Requested info id.</param>
        [HttpDelete("Info/{Id}")]
        public async Task<IActionResult> DeleteInfoAsync(int Id)
        {
            // Get contact info
            var info = await _context.ContactInfos.Where(i => i.Id == Id).FirstOrDefaultAsync();

            // Remove info
            _context.ContactInfos.Remove(info);

            // Save db changes
            await _context.SaveChangesAsync();

            // Return
            return NoContent();
        }

        #endregion
    }
}
