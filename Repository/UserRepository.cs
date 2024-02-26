using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Internet_Bank.Data;
using Internet_Bank.Model.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Internet_Bank.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public UserRepository(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            var existingUser = await _userManager.FindByNameAsync(signUpDto.UserName);
            if (existingUser != null)
            {
                // Username is not unique, return an error
                return IdentityResult.Failed(new IdentityError { Code = "DuplicateUserName", Description = "این نام کاربری تکراری است. لطفا نام کاربری دیگری انتخاب کنید" });
            }

            var user = new ApplicationUser()
            {
                FirstName = signUpDto.FirstName,
                LastName = signUpDto.LastName,
                NationalCode = signUpDto.NationalCode,
                Birthdate = signUpDto.Birthdate.Date,
                PhoneNumber = signUpDto.PhoneNumber,
                Email = signUpDto.Email,
                UserName = signUpDto.UserName
            };
            return await _userManager.CreateAsync(user);
        }

        public async Task<string> Login([FromBody] LoginDto loginDto)
        {

            var user = await _userManager.Users.Where(x => x.UserName.ToLower() == loginDto.UserName.ToLower()
                                                        && x.Email.ToLower() == loginDto.Email.ToLower())
                                                 .FirstOrDefaultAsync();
            // var id=user.Id;
            if (user == null)
            {
                return null;
            }
            // claims-metadata(signature)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,loginDto.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
              issuer: _configuration["JWT:ValidIssuer"],
              audience: _configuration["JWT:ValidAudiance"],
              expires: DateTime.Now.AddDays(1),
              claims: authClaims,
              signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}