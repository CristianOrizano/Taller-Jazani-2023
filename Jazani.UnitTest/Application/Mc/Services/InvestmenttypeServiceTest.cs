using AutoFixture;
using AutoMapper;
using Jazani.Taller.Aplication.Mc.Dtos.Investments.Profiles;
using Jazani.Taller.Aplication.Mc.Dtos.Investmenttypes;
using Jazani.Taller.Aplication.Mc.Dtos.Investmenttypes.Profiles;
using Jazani.Taller.Aplication.Mc.Services;
using Jazani.Taller.Aplication.Mc.Services.Implemetations;
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
    public class InvestmenttypeServiceTest
    {
        Mock<IInvestmenttypeRepository> _investmenttypeRepository;
        Mock<ILogger<InvestmenttypeService>> _mockILogger;

        IMapper _mapper;
        Fixture _fixture;

        public InvestmenttypeServiceTest()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(c =>
            {
                c.AddProfile<InvestmenttypeProfile>();
            });

            _mapper = mapperConfiguration.CreateMapper();

            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _investmenttypeRepository = new Mock<IInvestmenttypeRepository>();

            _mockILogger = new Mock<ILogger<InvestmenttypeService>>();
        }


        [Fact]
        public async void returnInvestmenttypeDtoWhenFindByIdAsync()
        {
            // Arrange
            Investmenttype invesmenttype = _fixture.Create<Investmenttype>();
            _investmenttypeRepository
                .Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(invesmenttype);
            // Act
            IInvestmenttypeService invesmentypeService = new InvestmenttypeService(_investmenttypeRepository.Object, _mapper);

            InvestmenttypeDto invesmentypeDto = await invesmentypeService.FindByIdAsync(invesmenttype.Id);
            // Assert
            Assert.Equal(invesmenttype.Id, invesmentypeDto.Id);
        }


        [Fact]
        public async void returnInvestmenttypeDtoWhenFindAllAsync()
        {
            // Arrage
            IReadOnlyList<Investmenttype> investmenttypes = _fixture.CreateMany<Investmenttype>(5).ToList();

            _investmenttypeRepository
                .Setup(r => r.FindAllAsync())
                .ReturnsAsync(investmenttypes);

            // Act
            IInvestmenttypeService invesmenttypeService = new InvestmenttypeService(_investmenttypeRepository.Object, _mapper);

            IReadOnlyList<InvestmenttypeDto> invesmenttypeDtos = await invesmenttypeService.FindAllAsync();

            // Assert
            Assert.Equal(investmenttypes.Count, invesmenttypeDtos.Count);
        }

        [Fact]
        public async void returnInvestmenttypeDtoWhenCreateAsync()
        {
            // Arrage
            Investmenttype investmenttype = new()
            {
                Id = 1,
                Name = "invesm",
                Description = "tipos invesme",
                State = true,
                RegistrationDate = DateTime.Now
            };

            _investmenttypeRepository
                .Setup(r => r.SaveAsync(It.IsAny<Investmenttype>()))
                .ReturnsAsync(investmenttype);
            // Act
            InvestmenttypeSaveDto investmenttypeSaveDto = new()
            {
                Name = investmenttype.Name,
                Description = investmenttype.Description,         
            };

            IInvestmenttypeService investmenttypeService = new InvestmenttypeService(_investmenttypeRepository.Object, _mapper);

            InvestmenttypeDto investmenttypeDto = await investmenttypeService.CreateAsync(investmenttypeSaveDto);
            // Assert
            Assert.Equal(investmenttype.Name, investmenttypeDto.Name);
        }

        [Fact]
        public async void returnInvestmenttypeDtoWhenEditAsync()
        {

            // Arrage
            int id = 1;

            Investmenttype investmenttype = new()
            {
                Id = id,
                Name = "invesm",
                Description = "tipos invesme",
                State = true,
                RegistrationDate = DateTime.Now
            };

            _investmenttypeRepository
                .Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(investmenttype);

            _investmenttypeRepository
                .Setup(r => r.SaveAsync(It.IsAny<Investmenttype>()))
                .ReturnsAsync(investmenttype);

            // Act
            InvestmenttypeSaveDto investmenttypeSaveDto = new()
            {
                Name = investmenttype.Name,
                Description = investmenttype.Description,
            };

            IInvestmenttypeService investmenttypeService = new InvestmenttypeService(_investmenttypeRepository.Object, _mapper);

            InvestmenttypeDto mineralTypeDto = await investmenttypeService.CreateAsync(investmenttypeSaveDto);

            // Assert
            Assert.Equal(investmenttype.Id, mineralTypeDto.Id);
        }

        [Fact]
        public async void returnInvestmenttypeDtoWhenDisabledAsync()
        {
            // Arrage
            int id = 1;
            Investmenttype investmenttype = new()
            {
                Id = id,
                Name = "invesm",
                Description = "tipos invesme",
                State = false,
                RegistrationDate = DateTime.Now
            };

            _investmenttypeRepository
                .Setup(r => r.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(investmenttype);

            _investmenttypeRepository
                .Setup(r => r.SaveAsync(It.IsAny<Investmenttype>()))
                .ReturnsAsync(investmenttype);
            // Act

            IInvestmenttypeService investmenttypeService = new InvestmenttypeService(_investmenttypeRepository.Object, _mapper);

            InvestmenttypeDto investmenttypeDto = await investmenttypeService.DisabledAsync(id);


            // Assert
            Assert.Equal(investmenttype.State, investmenttypeDto.State);
        }


    }
}
