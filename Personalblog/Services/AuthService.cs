using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Auth;
using Personalblog.Models.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Personalblog.Services
{
    public class AuthService
    {
        private readonly MyDbContext _myDbContext;
        private readonly SecuritySetting _securitySetting;
        public AuthService(IOptions<SecuritySetting> options,MyDbContext myDbContext)
        {
            _securitySetting = options.Value;
            _myDbContext = myDbContext;
        }
        public LoginToken GenerateLoginToken(User user)
        {
            var claims = new List<Claim>
            {
                new("username",user.Name),
                new(JwtRegisteredClaimNames.Name,user.Id),
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securitySetting.Token.Key));
            var signCredentital = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var jwtToken =  new JwtSecurityToken(
                issuer:_securitySetting.Token.Issuer,
                audience:_securitySetting.Token.Audience,
                claims:claims,
                expires:DateTime.Now.AddDays(7),
                signingCredentials:signCredentital);

            return new LoginToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
            };
        }

        public User? GetUserByName(string name)
        {
            return _myDbContext.users.FirstOrDefault(x => x.Name == name);
        }
        public User? GetUser(ClaimsPrincipal userClaim)
        {
            var userId = userClaim.Identity?.Name;
            var userName = userClaim.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            if (userId == null || userName == null) return null;
            return new User { Id = userId, Name = userName };
        }
    }
}
