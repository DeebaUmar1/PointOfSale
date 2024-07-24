using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace PointOfSaleWebAPIs
{
    public class TokenService
    {
        // Ensure the key is at least 32 bytes long (256 bits)
        private const string SecretKey = "YourSuperSecretKeyThatIsAtLeast32BytesLong!"; // Ensure this key is at least 32 bytes long
        private readonly SymmetricSecurityKey _signingKey;

        public TokenService()
        {
            //uses secret key to create signature on token
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        }

        public string GenerateToken(string username)
        {
            //to create and validate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
           //to describe to whom the token belongs to, the expiration, etc.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //adds a piece of info to the username
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature) 
                // this signs the token using
                // signingkey plus algorithm
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); //creates token
            return tokenHandler.WriteToken(token); // converts token to string and sends it to client side
        }
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
            return principal;
        }
    }
}