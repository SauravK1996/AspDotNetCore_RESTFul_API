﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly AppSettings _appSettings;
        public UserRepository(ApplicationDbContext applicationDb,IOptions<AppSettings> appSettings)
        {
            _applicationDb = applicationDb;
            _appSettings = appSettings.Value;
        }

        public IOptions<AppSettings> AppSettings { get; }

        public User Authenticate(string username, string password)
        {
            var user = _applicationDb.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
            //user not found
            if(user == null)
            {
                return null;
            }

            //if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                { 
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                                (new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";
            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _applicationDb.Users.SingleOrDefault(x => x.Username == username);

            //return null if user not found
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public User Register(string username, string password)
        {
            User userObj = new User()
            {
                Username = username,
                Password = password,
                Role = "Admin"
            };

            _applicationDb.Users.Add(userObj);
            _applicationDb.SaveChanges();
            userObj.Password = "";
            return userObj;
        }
    }
}
