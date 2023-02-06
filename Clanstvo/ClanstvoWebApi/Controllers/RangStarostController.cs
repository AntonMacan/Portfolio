using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clanstvo.Repositories;
using ClanstvoWebApi.DTOs;
using DbModels = Clanstvo.DataAccess.SqlServer.Data.DbModels;
using Clanstvo.Commons;
using Clanstvo.Repositories.SqlServer;
using BaseLibrary;
using Microsoft.AspNetCore.Rewrite;

namespace ClanstvoWebApi.Contollers

    // made by Luka Slugečić, Anton Macan, Bruno Rački
{
    [Route("api/[controller]")]
    [ApiController]
    public class RangStarostController : ControllerBase
    {
        private readonly IRangStarostRepository _rangStarostRepository;

        public RangStarostController(IRangStarostRepository context)
        {
            _rangStarostRepository = context;
        }

        // GET: api/RangStarost
        [HttpGet]
        public ActionResult<IEnumerable<RangStarost>> GetAllRangStarost()
        {
            var rangStarostResults = _rangStarostRepository.GetAll()
                .Map(rangStarost => rangStarost.Select(DtoMapping.ToDto));

            return rangStarostResults
                ? Ok(rangStarostResults.Data)
                : Problem(rangStarostResults.Message, statusCode: 500);
        }

        // GET: api/RangStarost/5
        [HttpGet("{id}")]
        public ActionResult<RangStarost> GetRangStarost(int id)
        {
            var rangStarostResult = _rangStarostRepository.Get(id).Map(DtoMapping.ToDto);

            return rangStarostResult switch
            {
                { IsSuccess: true } => Ok(rangStarostResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(rangStarostResult.Message, statusCode: 500)
            };
        }

        // PUT: api/RangStarost/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditRangStarost(int id, RangStarost rangStarost)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rangStarost.Id)
            {
                return BadRequest();
            }

            if (!_rangStarostRepository.Exists(id))
            {
                return NotFound();
            }
            var updateResult = _rangStarostRepository.Update(rangStarost.ToDomain());

            return updateResult
                ? AcceptedAtAction("EditRangStarost", rangStarost)
                : Problem(updateResult.Message, statusCode: 500);
        }

        // POST: api/RangStarost
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<RangStarost> CreateRangStarost(RangStarost rangStarost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createResult = _rangStarostRepository.Insert(rangStarost.ToDomain());

            return createResult
                ? CreatedAtAction("GetRangStarost", new { id = rangStarost.Id }, rangStarost)
                : Problem(createResult.Message, statusCode: 500);
        }

        // DELETE: api/RangStarost/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRangStarost(int id)
        {
            if (!_rangStarostRepository.Exists(id))
                return NotFound();

            var deleteResult = _rangStarostRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}
