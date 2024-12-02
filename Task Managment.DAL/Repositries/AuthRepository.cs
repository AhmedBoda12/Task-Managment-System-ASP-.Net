using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Task_Managment.DAL.Models;
using Task_Managment.DAL.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Task_Managment.DAL.Data;
using System.Text.RegularExpressions;
using Task_Managment.DAL.Enums;
using System.Security.Principal;

using Task_Managment.DAL.Helpers;

namespace Task_Managment.DAL.Repositries
{
    public class AuthRepository : BaseRepository<ApplicationUser>, IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
            : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
           
        }

        public async Task<AuthModel> RegisterAsync(ApplicationUser model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new AuthModel { Massage = "Email already registered!" };

            var result = await _userManager.CreateAsync(model, model.PasswordHash);
            if (!result.Succeeded)
                return new AuthModel { Massage = string.Join(", ", result.Errors.Select(e => e.Description)) };

            await _userManager.AddToRoleAsync(model, "User");

            var token = await CreateJwtTokenAsync(model);
            return new AuthModel
            {
                Massage = "Register Done Successfully!",
                Id = model.Id,
                Email = model.Email,
                ExpiresOn = token.ValidTo,
                Username = model.UserName,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };
        }
        public async Task<AuthModel> GetJwtTokenAsync(string identifier, string password)
        {
            var authModel = new AuthModel();
            ApplicationUser user;

            if (IsValidEmail(identifier))
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }
            else
            {
                user = await _userManager.FindByNameAsync(identifier);
            }

            var pass = await _userManager.CheckPasswordAsync(user, password);
            if (user is null || !pass)
            {
                authModel.Massage = "Invalid CREDENTIALS!";
                return authModel;
            }
            var jwtSecurityToken = await CreateJwtTokenAsync(user);
            var rolesList = await _userManager.GetRolesAsync(user);
            authModel.Id = user.Id;
            authModel.Username = user.UserName;
            authModel.Roles = rolesList.ToList();
            authModel.IsAuthenticated = true;
            authModel.Email = user.Email;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            return authModel;
        }
        public async Task<string> AddToRoleAsync(AddToRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var sanitizedRole = model.Role.Trim().ToLower();

            if (user == null || !await _roleManager.RoleExistsAsync(sanitizedRole))
            {
                return "UserId or role is not valid!";
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove all current roles
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                return "Failed to remove existing roles!";
            }

            var addResult = await _userManager.AddToRoleAsync(user, sanitizedRole);

            if (!addResult.Succeeded)
            {
                return "Failed to add new role!";
            }

            // Update UserType based on the new role
            switch (sanitizedRole)
            {
                case "user":
                    user.UserType = UserTypes.User;
                    break;
                case "admin":
                    user.UserType = UserTypes.Admin;
                    break;
                default:
                    return "Invalid role!";
            }


            // Save changes to the user
            var updateResult = await _userManager.UpdateAsync(user);

            return addResult.Succeeded && updateResult.Succeeded ? string.Empty : "Something went wrong, please try again!";
        }
        public async Task<AuthModel> UpdateUserAsync(string id, ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new AuthModel { Massage = "User not found!" };
            }
            if (!string.IsNullOrEmpty(model.UserName) && model.UserName != user.UserName)
            {
                var existingUserName = await _userManager.FindByNameAsync(model.UserName);
                if (existingUserName != null)
                {
                    return new AuthModel { Massage = "Username is already taken!" };
                }
                user.UserName = model.UserName;
            }

            if (!string.IsNullOrEmpty(model.Email) && model.Email != model.Email)
            {
                var existingEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingEmail != null)
                {
                    return new AuthModel { Massage = "Email is already registered!" };
                }
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                var emailChangeResult = await _userManager.ChangeEmailAsync(user, model.Email, token);
                if (!emailChangeResult.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in emailChangeResult.Errors)
                    {
                        errors += $"{error.Description}, ";
                    }
                    return new AuthModel { Massage = errors };
                }
            }



            // Update user properties
            user.Name = model.Name ?? user.Name;
            user.Country = model.Country ?? user.Country;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

       

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthModel { Massage = errors };
            }

            return new AuthModel
            {
                Massage = "User Updated Successfully!",
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                IsAuthenticated = true,
                Roles = new List<string> { user.UserType.ToString() },

            };

        }
        public async Task<string> ChangePasswordAsync(string id, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return "User not found";

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded ? "Password Changed" : "Failed to change password";
        }
        public async Task<string> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? string.Empty : "Failed to delete user";
        }
        private async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", "User"));

            var claims = new[]
            {

                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        private bool IsValidEmail(string email)
        {
            // Simple regex for email validation
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

       
    }
}
