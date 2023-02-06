using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clanstvo.Repositories;
using Clanstvo.Repositories.SqlServer;
using ClanstvoWebApi.DTOs;
using DbModels = Clanstvo.DataAccess.SqlServer.Data.DbModels;
using Clanstvo.Commons;
using BaseLibrary;
using System;

namespace ClanstvoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClanarinaController : ControllerBase
    {
        private readonly IClanarinaRepository _clanarinaRepository;

        public ClanarinaController(IClanarinaRepository context)
        {
            _clanarinaRepository = context;
        }

        // GET: api/Clanarine
        [HttpGet]
        public ActionResult<IEnumerable<Clanarina>> GetAllClanarina()
        {
            var clanarinaResults = _clanarinaRepository.GetAll()
            .Map(clanarina => clanarina.Select(DtoMapping.ToDto));

            return clanarinaResults
                ? Ok(clanarinaResults.Data)
                : Problem(clanarinaResults.Message, statusCode: 500);
        }

        
        // GET: api/Clanarine
        [HttpGet("/api/[controller]/Neplacene")]
        public ActionResult<IEnumerable<Clanarina>> GetNeplaceneClanarina()
        {
            var clanarinaResults = _clanarinaRepository.GetAllNeplacene()
            .Map(clanarina => clanarina.Select(DtoMapping.ToDto));

            return clanarinaResults
                ? Ok(clanarinaResults.Data)
                : Problem(clanarinaResults.Message, statusCode: 500);
        }


        // GET: api/Clanarine/5
        [HttpGet("{id}")]
        public ActionResult<Clanarina> GetClanarina(int id)
        { 
            var clanarinaResult = _clanarinaRepository.Get(id).Map(DtoMapping.ToDto);

            return clanarinaResult switch
            {
                { IsSuccess: true } => Ok(clanarinaResult.Data),
                { IsFailure: true } => NotFound(),
                { IsException: true } or _ => Problem(clanarinaResult.Message, statusCode: 500)
            };
        }

        // PUT: api/Clanarine/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditClanarina(int id, Clanarina clanarina)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clanarina.Id)
            {
                return BadRequest();
            }

            if (!_clanarinaRepository.Exists(id))
            {
                return NotFound();
            }


            var updateResult = _clanarinaRepository.Update(clanarina.ToDomain());

            return updateResult
                ? AcceptedAtAction("EditClanarina", clanarina)
                : Problem(updateResult.Message, statusCode: 500);
        }

        // POST: api/Clanarina
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Clanarina> CreateClanarina(Clanarina clanarina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createResult = _clanarinaRepository.Insert(clanarina.ToDomain());

            return createResult
            ? CreatedAtAction("GetClanarina", new { id = clanarina.Id }, clanarina)
                : Problem(createResult.Message, statusCode: 500);
        }

        // DELETE: api/Clanarina/5
        [HttpDelete("{id}")]
        public IActionResult DeleteClanarina(int id)
        {
            if (!_clanarinaRepository.Exists(id))
                return NotFound();

            var deleteResult = _clanarinaRepository.Remove(id);
            return deleteResult
                ? NoContent()
                : Problem(deleteResult.Message, statusCode: 500);
        }
    }
}
