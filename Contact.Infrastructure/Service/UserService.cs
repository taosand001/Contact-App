﻿using Contact.Domain.Interfaces;
using Contact.Infrastructure.Helpers;
using Contact.Infrastructure.Interfaces;
using Contact.Shared.Custom;
using Contact.Shared.Dto;
using Contact.Shared.Enum;
using Contact.Shared.Model;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Contact.Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IJwtService jwtService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<string> Login(LoginDto user)
        {
            var existingUser = await _userRepository.Get(user.Username);
            if (existingUser == null || !PasswordHashingService.VerifyPasswordHash(user.Password, existingUser.PasswordHash, existingUser.PasswordSalt))
            {
                _logger.LogError("Username or password is incorrect");
                throw new UnauthorizedErrorException("Username or password is incorrect");
            }
            var token = _jwtService.GenerateSecurityToken(existingUser);
            return token;

        }

        public async Task<string> Register(UserDto user)
        {
            var newUser = await CreateUser(user);
            await _userRepository.Add(newUser);
            _logger.LogInformation("User registered successfully");
            return _jwtService.GenerateSecurityToken(newUser);
        }

        public async Task UpdateUserRole(string userName, RoleType role)
        {
            var existingUser = await _userRepository.Get(userName);
            if (existingUser == null)
            {
                _logger.LogError("User not found");
                throw new NotFoundErrorException("User not found");
            }

            existingUser.Role = role;
            await _userRepository.Update(existingUser);
        }

        public async Task DeleteUserAdminRole(string userName)
        {
            var existingUser = await _userRepository.Get(userName);
            if (existingUser == null)
            {
                _logger.LogError("User not found");
                throw new NotFoundErrorException("User not found");
            }
            existingUser.Role = RoleType.User;
            await _userRepository.Update(existingUser);
        }

        public async Task DeleteUser(string userName)
        {
            var existingUser = await _userRepository.Get(userName);
            if (existingUser == null)
            {
                _logger.LogError("User not found");
                throw new NotFoundErrorException("User not found");
            }
            await _userRepository.Delete(existingUser);
        }

        public async Task ChangePassword(string userName, ChangePasswordDto changePassword)
        {
            var existingUser = await _userRepository.Get(userName);
            if (existingUser == null)
            {
                _logger.LogError("User not found");
                throw new NotFoundErrorException("User not found");
            }

            if (existingUser.Username != userName)
            {
                _logger.LogError("User not authorized to change password");
                throw new UnauthorizedErrorException("User not authorized to change password");
            }

            if (!PasswordHashingService.VerifyPasswordHash(changePassword.oldPassword, existingUser.PasswordHash, existingUser.PasswordSalt))
            {
                _logger.LogError("Old password is incorrect");
                throw new UnauthorizedErrorException("Old password is incorrect");
            }


            CreatePasswordHash(changePassword.newPassword, out var passwordHash, out var passwordSalt);
            existingUser.PasswordHash = passwordHash;
            existingUser.PasswordSalt = passwordSalt;
            existingUser.LastPasswordChange = DateTime.Now;
            await _userRepository.Update(existingUser);
        }

        public async Task SendPasswordToken(string userName)
        {
            try
            {
                var existingUser = await _userRepository.Get(userName);
                if (existingUser == null)
                {
                    _logger.LogError("User not found");
                    throw new NotFoundErrorException("User not found");
                }

                var token = GenerateRandomSixNumber();
                existingUser.Token = token;
                await _userRepository.Update(existingUser);
                EmailService.SendPasswordVerificationEmail(existingUser.Username, existingUser.Email, token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task VerifyPasswordToken(string userName, VerifyPasswordDto verifyPasswordDto)
        {
            var existingUser = await _userRepository.Get(userName);
            if (existingUser == null)
            {
                _logger.LogError("User not found");
                throw new NotFoundErrorException("User not found");
            }

            if (existingUser.Token != verifyPasswordDto.Token)
            {
                _logger.LogError("Invalid token");
                throw new BadRequestErrorException("Invalid token");
            }

            existingUser.Token = null;
            CreatePasswordHash(verifyPasswordDto.newPassword, out var passwordHash, out var passwordSalt);
            existingUser.PasswordHash = passwordHash;
            existingUser.PasswordSalt = passwordSalt;
            existingUser.LastPasswordChange = DateTime.Now;
            await _userRepository.Update(existingUser);
        }


        private string GenerateRandomSixNumber()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task<User> CreateUser(UserDto user)
        {
            var existingUser = await _userRepository.Get(user.Username);
            if (existingUser != null)
            {
                _logger.LogError("User already exists");
                throw new ConflictErrorException("User already exists");
            }

            CreatePasswordHash(user.Password, out var passwordHash, out var passwordSalt);
            return new User
            {
                Username = user.Username,
                Email = user.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = RoleType.User
            };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

    }
}
