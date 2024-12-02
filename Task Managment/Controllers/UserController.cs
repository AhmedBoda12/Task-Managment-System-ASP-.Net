using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Managment.bll.DTOs;
using Task_Managment.bll.Interfaces;
using Task_Managment.DAL.Models;


namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _authService.GetUserByIdAsync(id);
                return Ok(user);

            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromForm] EditUserDTO dto,IFormFile? imageFile)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst("uid")?.Value;
                var result = await _authService.UpdateUserAsync(userId, dto);
                if (!result.IsAuthenticated)
                {
                    return BadRequest(result.Massage);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("add-to-role")]
        public async Task<IActionResult> AddToRole([FromBody] AddToRoleModel model)
        {
            try
            {
                var result = await _authService.AddToRoleAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("search-by-role/{role}")]
        public async Task<IActionResult> SearchByRole(string role)
        {
            try
            {
                var users = await _authService.SearchUsersByRoleAsync(role);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("search-by-name/{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            try
            {
                var users = await _authService.SearchUsersByNameAsync(name);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
