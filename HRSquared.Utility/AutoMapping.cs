using AutoMapper;
using HRSquared.Entities;
using HRSquared.Models.ResponseModels;

namespace HRSquared.Utility
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            CreateMap<RefreshTokenModel, RefreshToken>();
        }
    }
}
