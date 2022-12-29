using HRSquared.Entities;
using AutoMapper;
using HRSquared.Models.ResponseModels;
using HRSquared.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRSquared.Repository.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HrsquaredDbContext _db;
        private readonly IMapper _mapper;
        public AuthRepository(HrsquaredDbContext db, IMapper mapper) 
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<RefreshToken> GetRefreshToken(int id)
        {
            return await _db.RefreshTokens.FindAsync(id);
        }

        public async Task<bool> AddRefreshToken(int id, RefreshTokenModel refreshToken)
        {
            RefreshToken refresh = _mapper.Map<RefreshToken>(refreshToken);
            refresh.UserId = id;
                if (await DoesRefreshTokenExists(id))
                {
                    RefreshToken prevRefreshToken = await _db.RefreshTokens.FindAsync(id);
                    prevRefreshToken.Token = refreshToken.Token;
                    prevRefreshToken.Expires = refreshToken.Expires;
                    return await _db.SaveChangesAsync() > 0;
                }
            await _db.RefreshTokens.AddAsync(refresh);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DoesRefreshTokenExists(int id)
        {
            return await _db.RefreshTokens.AnyAsync(x => x.UserId == id);
        }
    }
}
