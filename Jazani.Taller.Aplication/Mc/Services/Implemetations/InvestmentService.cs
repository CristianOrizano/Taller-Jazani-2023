using AutoMapper;
using Jazani.Core.Pagination;
using Jazani.Taller.Aplication.Cores.Exceptions;
using Jazani.Taller.Aplication.Mc.Dtos.Investmentconcepts;
using Jazani.Taller.Aplication.Mc.Dtos.Investments;
using Jazani.Taller.Aplication.Mc.Dtos.Investmenttypes;
using Jazani.Taller.Domain.Mc.Models;
using Jazani.Taller.Domain.Mc.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Jazani.Taller.Aplication.Mc.Services.Implemetations
{
    public class InvestmentService: IInvestmentService
    {
        private readonly IInvestmentRepository _invesmepeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestmentService> _logger;

        public InvestmentService(IInvestmentRepository invespeRepository, IMapper mapper, ILogger<InvestmentService> logger)
        {
            _invesmepeRepository = invespeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<InvestmentDto> CreateAsync(InvestmentSaveDto saveDto)
        {
            Investment invesme = _mapper.Map<Investment>(saveDto);
            invesme.RegistrationDate = DateTime.Now;
            invesme.State = true;

            Investment invesmeSaved = await _invesmepeRepository.SaveAsync(invesme);
            return _mapper.Map<InvestmentDto>(invesmeSaved);
        }

        public async Task<InvestmentDto> DisabledAsync(int id)
        {
            Investment? invesme = await _invesmepeRepository.FindByIdAsync(id);

            if (invesme is null) throw InvesmentNotFound(id);
            invesme.State = false;

            Investment auctionSaved = await _invesmepeRepository.SaveAsync(invesme);

            return _mapper.Map<InvestmentDto>(auctionSaved);
        }

        public async Task<InvestmentDto> EditAsync(int id, InvestmentSaveDto saveDto)
        {
            Investment? invesme = await _invesmepeRepository.FindByIdAsync(id);

            if (invesme is null) throw InvesmentNotFound(id);

            _mapper.Map<InvestmentSaveDto, Investment>(saveDto, invesme);

            Investment invesmSaved = await _invesmepeRepository.SaveAsync(invesme);

            return _mapper.Map<InvestmentDto>(invesmSaved);
        }

        public async Task<IReadOnlyList<InvestmentDto>> FindAllAsync()
        {
            IReadOnlyList<Investment> invesme = await _invesmepeRepository.FindAllAsync();

            return _mapper.Map<IReadOnlyList<InvestmentDto>>(invesme);
        }

        public async Task<InvestmentDto?> FindByIdAsync(int id)
        {
            Investment? invesm = await _invesmepeRepository.FindByIdAsync(id);
            if (invesm is null)
            {
                _logger.LogWarning("Tipo de Invesment no encontrado para el id: " + id);
                throw InvesmentNotFound(id);
            }
            _logger.LogInformation("Tipo de Invesment {name}", invesm.Description);

            return _mapper.Map<InvestmentDto>(invesm);
        }

        public async Task<ResponsePagination<InvestmentDto>> PaginatedSearch(RequestPagination<InvesmentFilterDto> request)
        {
            var entity = _mapper.Map<RequestPagination<Investment>>(request);
            var response = await _invesmepeRepository.PaginatedSearch(entity);

            return _mapper.Map<ResponsePagination<InvestmentDto>>(response);
        }

        private NotFoundCoreException InvesmentNotFound(int id)
        {
            return new NotFoundCoreException("Tipo de Invesment no encontrado para el id: " + id);
        }
    }
}
