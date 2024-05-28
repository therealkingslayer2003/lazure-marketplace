using AccountsAPI.DTOs;
using AccountsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using AccountsAPI.Models;

namespace AccountsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService userService;
        private readonly JwtTokenService jwtTokenService;
        private readonly string password;

        public UsersController(UserService userService, JwtTokenService jwtTokenService,
          IConfiguration configuration)
        {
            this.userService = userService;
            this.jwtTokenService = jwtTokenService;
            this.password = configuration["PASSWORD"];
        }

        [HttpPost("login")]         //Identification by login details(a crypto wallet unique id)
        public IActionResult Login([FromBody] UserLoginDto userLoginDto)
        {
            if(!IsRequestAuthorized()) {
                var errorResponse = new Dictionary<string, string>
        {
            { "message", "The password is missing or invalid" }
        };
                return Unauthorized(errorResponse); 
            }


            var user = userService.GetUserByWalletId(userLoginDto.WalletId);

            if(user == null)    // If first login ever
            {
                user = userService.AddNewUser(userLoginDto.WalletId);
            }

            var jwtToken = jwtTokenService.GenerateToken(user.UserId.ToString());
            
            Response.Headers.Add("Authorization", $"Bearer {jwtToken}");
            return Ok("Successful");
        }


        [HttpGet("/wallet/{walletId}")]                 //Getting a user by its wallet
        public ActionResult<User> GetUserByWalletId(string walletId)
        {
            if (walletId == null) return NotFound();

            User user = userService.GetUserByWalletId(walletId);

            if(user == null) {
                var errorResponse = new Dictionary<string, string>
        {
            { "message", "Wrong Wallet" }
        };
                return NotFound(errorResponse); 
            }

            return Ok(user);
        
        }

        [HttpGet("/user/{userId}")]                 //Getting a user by its wallet
        public ActionResult<User> GetUserByUserId(int userId)
        {
            User user = userService.GetUserByUserId(userId);

            if (user == null) {
                var errorResponse = new Dictionary<string, string>
        {
            { "message", "Wrong userId" }
        };
                return NotFound(errorResponse); 
            }

            return Ok(user);

        }
        private bool IsRequestAuthorized() //Checking decrypted password if attached
                                          //if the request is authorized
                                          //in order to ensure secured connection
        {
            string header = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer "))
            {
                return false;
            }

            var recievedPassword = header.Substring("Bearer ".Length).Trim();
            if (string.IsNullOrWhiteSpace(recievedPassword))
            {
                return false;
            }

            return recievedPassword.Equals(password);
        }

        [HttpGet("get-product-owner-wallet-by-product-id/{productId}")]
        public ActionResult<ProductOwnerWalletResponseDto> GetProductOwnerWalletByProductId(int productId)
        {
            try
            {
                string productOwnerWalletId = userService.GetProductOwnerWalletByProductId(productId);
                var response = new ProductOwnerWalletResponseDto()
                { WalletId = productOwnerWalletId, Message = "Successful"};


                return Ok(response);
            }
            catch(ArgumentException ex)
            {
                var errorResponse = new Dictionary<string, string>
        {
            { "message", ex.Message }
        };
                return NotFound(errorResponse);
            }

            
        }
        [HttpGet("get-product-owners")]
        public ActionResult<List<ProductOwnerResponseDto>> GetProductOwners([FromQuery] int[] productId)
        {
            if (productId == null || productId.Length == 0)
            {
                var errorResponse = new Dictionary<string, string> {
                    { "message","No product IDs provided"} };

                return BadRequest(errorResponse);
            }
            return userService.GetProductOwners(productId);
        }
    }
}
        