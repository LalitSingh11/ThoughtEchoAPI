using AutoMapper;
using ThoughtEcho.Entities;
using ThoughtEcho.Models.RequestModels;
using ThoughtEcho.Models.ResponseModels;

namespace ThoughtEcho.Utilities
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            CreateMap<RefreshTokenModel, RefreshToken>();
            CreateMap<UserRegisterModel, UserCred>();
        }
    }
}
