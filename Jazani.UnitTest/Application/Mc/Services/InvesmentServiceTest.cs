using AutoFixture;
using AutoMapper;
using Jazani.Taller.Aplication.Ge.Dtos.MeasureUnits.Profiles;
using Jazani.Taller.Aplication.Ge.Dtos.PeriodTypes.Profiles;
using Jazani.Taller.Aplication.Mc.Dtos.Investmentconcepts.Profiles;
using Jazani.Taller.Aplication.Mc.Dtos.Investments;
using Jazani.Taller.Aplication.Mc.Dtos.Investments.Profiles;
using Jazani.Taller.Aplication.Mc.Dtos.Investmenttypes.Profiles;
using Jazani.Taller.Aplication.Mc.Dtos.MiningConcessions.Profiles;
using Jazani.Taller.Aplication.Mc.Services;
using Jazani.Taller.Aplication.Mc.Services.Implemetations;
using Jazani.Taller.Aplication.Soc.Dtos.Holders.Profiles;
using Jazani.Taller.Domain.Mc.Models;
using Jazani.Taller.Domain.Mc.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.UnitTest.Application.Mc.Services
{
    public class InvesmentServiceTest
    {
        Mock<IInvestmentRepository> _investmentRepository;
        Mock<ILogger<InvestmentService>> _mockILogger;

        IMapper _mapper;
        Fixture _fixture;

        public InvesmentServiceTest()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(c =>
            {
                c.AddProfile<InvestmentProfile>();
                c.AddProfile<MiningConcessionProfile>();
                c.AddProfile<InvestmentconceptProfile>();
                c.AddProfile<PeriodTypeProfile>();
                c.AddProfile<MeasureUnitProfile>();
                c.AddProfile<HolderProfile>();
                c.AddProfile<InvestmenttypeProfile>();
            });

            _mapper = mapperConfiguration.CreateMapper();

            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _investmentRepository = new Mock<IInvestmentRepository>();
            _mockILogger = new Mock<ILogger<InvestmentService>>();
        }


        [Fact]
        public async void returnInvestmentDtoWhenFindByIdAsync()
        {
            // Arrange
            Investment invesment = _fixture.Create<Investment>();
            _investmentRepository
                .Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(invesment);
            // Act
            IInvestmentService invesmentService = new InvestmentService(_investmentRepository.Object, _mapper, _mockILogger.Object);

            InvestmentDto ivesmentDto = await invesmentService.FindByIdAsync(invesment.Id);
            // Assert
            Assert.Equal(invesment.Description, ivesmentDto.Description);
        }


        [Fact]
        public async void returnInvestmentDtoWhenFindAllAsync()
        {
            // Arrage
            IReadOnlyList<Investment> investments = _fixture.CreateMany<Investment>(5).ToList();

            _investmentRepository
                .Setup(r => r.FindAllAsync())
                .ReturnsAsync(investments);

            // Act
            IInvestmentService invesmentService = new InvestmentService(_investmentRepository.Object, _mapper, _mockILogger.Object);

            IReadOnlyList<InvestmentDto> invesmentDtos = await invesmentService.FindAllAsync();

            // Assert
            Assert.Equal(investments.Count, invesmentDtos.Count);
        }

        [Fact]
        public async void returnInvestmentDtoWhenCreateAsync()
        {
            // Arrage
            Investment investment = new()
            {
                Id = 1,
                Description= "",
                AmountInvested = 1,
                MiningConcessionid=4,
                Investmentconceptid=2,
                Measureunitid=1,
                Periodtypeid=3,
                State = true,
                RegistrationDate = DateTime.Now
            };

            _investmentRepository
                .Setup(r => r.SaveAsync(It.IsAny<Investment>()))
                .ReturnsAsync(investment);
            // Act
            InvestmentSaveDto investmentSaveDto = new()
            {
                AmountInvested = investment.AmountInvested,
                Description = investment.Description,
                MiningConcessionid = investment.MiningConcessionid,
                Periodtypeid = investment.Periodtypeid,
                Measureunitid = investment.Measureunitid,
                Investmentconceptid = investment.Investmentconceptid
            };

            IInvestmentService invesmentService = new InvestmentService(_investmentRepository.Object, _mapper, _mockILogger.Object);

            InvestmentDto invesmentDto = await invesmentService.CreateAsync(investmentSaveDto);
            // Assert
            Assert.Equal(investment.Description, invesmentDto.Description);
        }

        [Fact]
        public async void returnInvestmentDtoWhenEditAsync()
        {

            // Arrage
            int id = 1;

            Investment investment = new()
            {
                Id = id,
                Description = "",
                AmountInvested = 1,
                MiningConcessionid = 4,
                Investmentconceptid = 2,
                Measureunitid = 1,
                Periodtypeid = 3,
                State = true,
                RegistrationDate = DateTime.Now
            };

            _investmentRepository
                .Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(investment);

            _investmentRepository
                .Setup(r => r.SaveAsync(It.IsAny<Investment>()))
                .ReturnsAsync(investment);

            // Act
            InvestmentSaveDto investmentSaveDto = new()
            {
                AmountInvested = investment.AmountInvested,
                Description = investment.Description,
                MiningConcessionid = investment.MiningConcessionid,
                Periodtypeid = investment.Periodtypeid,
                Measureunitid = investment.Measureunitid,
                Investmentconceptid = investment.Investmentconceptid
            };

            IInvestmentService invesmentService = new InvestmentService(_investmentRepository.Object, _mapper, _mockILogger.Object);

            InvestmentDto mineralTypeDto = await invesmentService.EditAsync(id, investmentSaveDto);

            // Assert
            Assert.Equal(investment.Id, mineralTypeDto.Id);
        }

        [Fact]
        public async void returnInvestmentDtoWhenDisabledAsync()
        {
            // Arrage
            int id = 1;
            Investment investment = new()
            {
                Id = id,
                Description = "",
                AmountInvested = 1,
                MiningConcessionid = 4,
                Investmentconceptid = 2,
                Measureunitid = 1,
                Periodtypeid = 3,
                State = false,
                RegistrationDate = DateTime.Now
            };

            _investmentRepository
                .Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(investment);

            _investmentRepository
                .Setup(r => r.SaveAsync(It.IsAny<Investment>()))
                .ReturnsAsync(investment);
            // Act

            IInvestmentService invesmentService = new InvestmentService(_investmentRepository.Object, _mapper, _mockILogger.Object);
            InvestmentDto invesmentDto = await invesmentService.DisabledAsync(id);
            // Assert
            Assert.Equal(investment.State, invesmentDto.State);
        }



    }
}
