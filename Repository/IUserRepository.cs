using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internet_Bank.Model.User;
using Microsoft.AspNetCore.Identity;

namespace Internet_Bank.Repository
{
    public interface IUserRepository
    {
        Task<IdentityResult> SignUp(SignUpDto signUpDTO);
        Task<string> Login(LoginDto loginDto);
    }
}