using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TeaUnitWebAPI.Models;

namespace TeaUnitWebAPI.BussinessLogin
{
    public partial class ServiceAction
    {
        public IConfiguration Configuration { get; }
        private string ConnectionString =string.Empty;
        public ServiceAction(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            this.ConnectionString = Configuration["ConnectionStrings:ConnectionSQLServer"];
        }
        public string BuildToken(Users user)
        {
            // key is case-sensitive
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, Configuration["Jwt:Subject"]),
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("prename", user.PreName),
                new Claim("firstname", user.FirstName),
                new Claim("lastname", user.LastName),
                new Claim("position", user.Position),
            //ใช้ role เพื่อลดการโหลดดาต้าเบส
                new Claim(ClaimTypes.Role, user.Position)
            };
            var expires = DateTime.Now.AddDays(Convert.ToDouble(Configuration["Jwt:ExpireDay"]));
            //แก้วันที่ได้
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}