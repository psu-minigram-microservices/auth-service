namespace Minigram.Auth.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Minigram.Auth.DTO;
    using Minigram.Auth.Models;
    using Minigram.Auth.Services;

    [ApiVersion("1.0")]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        private readonly UserService _userService;

        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(
            TokenService tokenService,
            UserService userService,
            IPasswordHasher<User> passwordHasher)
        {
            _tokenService = tokenService;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost(nameof(Login))]
        public async Task<JwtResponse> Login(LoginRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            User user = await _userService.GetByEmail(dto.Email);
            
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(null!, user.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Invalid email or password.");
            }

            string jwtToken = await _tokenService.Generate(user);

            return new JwtResponse
            {
                AccessToken = jwtToken,
                RefreshToken = Guid.NewGuid().ToString(),
            };
        }

        [HttpPost(nameof(Register))]
        public async Task<JwtResponse> Register(RegisterRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            dto.Password = _passwordHasher.HashPassword(null!, dto.Password);

            User user = await _userService.Create(dto);
            string jwtToken = await _tokenService.Generate(user);

            return new JwtResponse
            {
                AccessToken = jwtToken,
                RefreshToken = Guid.NewGuid().ToString(),
            };
        }

        [HttpPost(nameof(Logout))]
        public async Task<ActionResult> Logout()
        {
            return Ok();
        }

        [HttpPost(nameof(Refresh))]
        public async Task<JwtResponse> Refresh(RefreshRequest dto)
        {
            return new JwtResponse
            {
                AccessToken = string.Empty,
                RefreshToken = "12345",
            };
        }
    }
}
