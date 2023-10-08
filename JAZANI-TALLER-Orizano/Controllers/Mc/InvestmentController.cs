using Jazani.Taller.Aplication.Mc.Dtos.Investmentconcepts;
using Jazani.Taller.Aplication.Mc.Dtos.Investments;
using Jazani.Taller.Aplication.Mc.Services;
using JAZANI_TALLER_Orizano.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JAZANI_TALLER_Orizano.Controllers.Mc
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {
        //Inyeccion
        private readonly IInvestmentService _invesmenService;
        public InvestmentController(IInvestmentService invesmeService)
        {
            _invesmenService = invesmeService;
        }

        // GET: api/<InvestmentController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvestmentDto))]
        public async Task<IEnumerable<InvestmentDto>> Get()
        {
            return await _invesmenService.FindAllAsync();
        }

        // GET api/<InvestmentController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvestmentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorModel))]
        public async Task<Results<NotFound, Ok<InvestmentDto>>> Get(int id)
        {
            var response = await _invesmenService.FindByIdAsync(id);
            return TypedResults.Ok(response);
        }

        // POST api/<InvestmentController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(InvestmentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<Results<BadRequest, CreatedAtRoute<InvestmentDto>>> Post([FromBody] InvestmentSaveDto invesSaveDto)
        {
            var response = await _invesmenService.CreateAsync(invesSaveDto);
            return TypedResults.CreatedAtRoute(response);
        }

        // PUT api/<InvestmentController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvestmentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<Results<BadRequest, CreatedAtRoute<InvestmentDto>>> Put(int id, [FromBody] InvestmentSaveDto invesmeSaveDto)
        {
            var response = await _invesmenService.EditAsync(id, invesmeSaveDto);
            return TypedResults.CreatedAtRoute(response);
        }

        // DELETE api/<InvestmentController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvestmentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<InvestmentDto> Delete(int id)
        {
            return await _invesmenService.DisabledAsync(id);
        }
    }
}
