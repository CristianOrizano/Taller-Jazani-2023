using AutoFixture;
using AutoMapper;
using Jazani.Taller.Aplication.Soc.Dtos.Holders;
using Jazani.Taller.Aplication.Soc.Dtos.Holders.Profiles;
using Jazani.Taller.Aplication.Soc.Services;
using Jazani.Taller.Aplication.Soc.Services.Implementations;
using Jazani.Taller.Domain.Soc.Models;
using Jazani.Taller.Domain.Soc.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.UnitTest.Application.Soc.Services
{
    public class HolderServiceTest
    {
        Mock<IHolderRepository> _holderRepository;
        Mock<ILogger<HolderService>> _mockILogger;

        IMapper _mapper;
        Fixture _fixture;

        public HolderServiceTest()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(c =>
            {
                c.AddProfile<HolderProfile>();
            });

            _mapper = mapperConfiguration.CreateMapper();

            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _holderRepository = new Mock<IHolderRepository>();

            _mockILogger = new Mock<ILogger<HolderService>>();
        }


        [Fact]
        public async void returnHolderDtoWhenFindAllAsync()
        {
            // Arrage
            IReadOnlyList<Holder> holders = _fixture.CreateMany<Holder>(5).ToList();

            _holderRepository
                .Setup(r => r.FindAllAsync())
                .ReturnsAsync(holders);

            // Act
            IHolderService holderService = new HolderService(_holderRepository.Object, _mapper);

            IReadOnlyList<HolderDto> holderDtos = await holderService.FindAllAsync();

            // Assert
            Assert.Equal(holders.Count, holderDtos.Count);
        }



    }
}
