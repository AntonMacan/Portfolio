using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clanstvo.Repositories;
using ClanstvoWebApi.DTOs;
using DbModels = Clanstvo.DataAccess.SqlServer.Data.DbModels;
using Clanstvo.Commons;
using BaseLibrary;
using Microsoft.AspNetCore.Rewrite;
using System.Data;

namespace ClanstvoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RangZaslugaController : ControllerBase
    {
        private readonly IRangZaslugaRepository _rangZaslugaRepository;

        public RangZaslugaController(IRangZaslugaRepository context)
        {
            _rangZaslugaRepository = context;
        }

        // GET: api/RangZasluga
        [HttpGet]
        public ActionResult<IEnumerable<RangZasluga>> GetAllRangZasluga()
        {
            var rangZaslugaResults = _rangZaslugaRepository.GetAll()
                .Map(rangZasluga => rangZasluga.Select(DtoMapping.ToDto));

            return rangZaslugaResults
                ? Ok(rangZaslugaResults.Data)
                : Problem(rangZaslugaResults.Message, statusCode: 500);
        }

        // GET: api/RangZasluga/5
        [HttpGet("{id}")]
        public ActionResult<RangZasluga> GetRangZasluga(int id)
        {
            var rangZaslugaResult =  _rangZaslugaRepository.Get(id).Map(DtoMapping.ToDto);

            return rangZaslugaResult switch
            {
                { IsSuccess: true } => Ok(rangZaslugaResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(rangZaslugaResult.Message, statusCode: 500)
            };
        }

        // PUT: api/RangZasluga/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditRangZasluga(int id, RangZasluga rangZasluga)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rangZasluga.Id)
            {
                return BadRequest();
            }

            if (!_rangZaslugaRepository.Exists(id))
            {
                return NotFound();
            }

            var updateResult = _rangZaslugaRepository.Update(rangZasluga.ToDomain());

            return updateResult
                ? AcceptedAtAction("EditRangZasluga", rangZasluga)
                : Problem(updateResult.Message, statusCode: 500);
        }
        
        // POST: api/RangZasluga
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<RangZasluga> CreateRangZasluga(RangZasluga rangZasluga)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createResult = _rangZaslugaRepository.Insert(rangZasluga.ToDomain());

            return createResult
                ? CreatedAtAction("GetRangZasluga", new { id = rangZasluga.Id }, rangZasluga)
                : Problem(createResult.Message, statusCode: 500);
        }

        // DELETE: api/RangZasluga/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            if (!_rangZaslugaRepository.Exists(id))
                return NotFound();

            var deleteResult = _rangZaslugaRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}
