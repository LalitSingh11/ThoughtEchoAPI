using ThoughtEcho.Entities;
using AutoMapper;
using ThoughtEcho.Models.ResponseModels;
using ThoughtEcho.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ThoughtEcho.Repository.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ThoughtEchoDbContext _db;
        private readonly IMapper _mapper;
        public AuthRepository(ThoughtEchoDbContext db, IMapper mapper) 
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
                    prevRefreshToken!.Token = refreshToken.Token;
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

        public async Task<bool> IsEmailVerified(string email)
        {
            return await _db.UserCreds.Where(x => x.Email == email).Select(x => x.IsEmailVerified).SingleOrDefaultAsync();
        }

        public async Task<bool> AddOtp(string email, int otp)
        {
            EmailVerificationOtp otpMOdel = new()
            {
                UserEmail = email,
                Otp = otp,
                Expires = DateTime.Now.AddMinutes(10)
            };
            await _db.EmailVerificationOtps.AddAsync(otpMOdel);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<EmailVerificationOtp> GetOtp(string email)
        {
            return await _db.EmailVerificationOtps.FirstOrDefaultAsync(x => x.UserEmail == email);
        }

        public async Task<bool> UpdateOtp(string email, int otp)
        {
            var previousOtp = await _db.EmailVerificationOtps.FindAsync(email);
            previousOtp.Otp = otp;
            previousOtp.Expires= DateTime.Now.AddMinutes(10);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> updateEmailVerifcation(int id)
        {
            UserCred user = await _db.UserCreds.FindAsync(id);
            user.IsEmailVerified = true;
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
