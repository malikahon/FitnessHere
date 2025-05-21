using AutoMapper;
using FitnessHere.DAL.DTOs;
using FitnessHere.Models;

namespace FitnessHere.Infastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile() { 
            CreateMap<MemberDTO, MemberViewModel>().ReverseMap();
        }
    }
}
