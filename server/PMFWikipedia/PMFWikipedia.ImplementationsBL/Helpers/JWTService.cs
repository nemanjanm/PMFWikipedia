using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PMFWikipedia.ImplementationsBL.Helpers
{
    public class JWTService : IJWTService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JWTService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
        }
        public string GetUserProgram()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst("Program")?.Value;
        }
    }
}