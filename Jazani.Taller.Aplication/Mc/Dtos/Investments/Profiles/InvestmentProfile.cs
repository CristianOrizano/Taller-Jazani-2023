using AutoMapper;
using Jazani.Core.Pagination;
using Jazani.Taller.Aplication.Mc.Dtos.Investmentconcepts;
using Jazani.Taller.Domain.Mc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.Taller.Aplication.Mc.Dtos.Investments.Profiles
{
    public class InvestmentProfile: Profile
    {
        public InvestmentProfile()
        {
            CreateMap<Investment, InvestmentDto>();


            CreateMap<Investment, InvestmentSaveDto>().ReverseMap();

            CreateMap<Investment, InvesmentFilterDto>().ReverseMap();

            CreateMap<ResponsePagination<Investment>, ResponsePagination<InvestmentDto>>();
            CreateMap<RequestPagination<Investment>, RequestPagination<InvesmentFilterDto>>()
                .ReverseMap();

        }
    }
}
