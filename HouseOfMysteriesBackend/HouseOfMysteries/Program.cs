
using HouseOfMysteries.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using HouseOfMysteries.Classes;
using System.Security.Cryptography;
using System.Net.Mail;

namespace HouseOfMysteries
{
    public class Program
    {
        public static int SaltLength = 64;

        public static TokenHolder loggedInUsers = new TokenHolder(Guid.Parse("724bdf82-0e13-4dc4-a596-ee660d0a700a"),false, 1000);//Master token

        public static string GenerateSalt()
        {
            Random random = new Random();
            string karakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string salt = "";
            for (int i = 0; i < SaltLength; i++)
            {
                salt += karakterek[random.Next(karakterek.Length)];
            }
            return salt;
        }

        public static string CreateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public static async Task SendEmail(string mailAddressTo, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("noreply.rejtelyekhaza@gmail.com");
            mail.To.Add(mailAddressTo);
            mail.Subject = subject;
            mail.Body = body;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("noreply.rejtelyekhaza@gmail.com", "avskcvaempdkwsog");
            SmtpServer.EnableSsl = true;
            await SmtpServer.SendMailAsync(mail);
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddDbContext<HouseofmysteriesContext>(options =>
            {
                var ConnectionString = builder.Configuration.GetConnectionString("MySQL");
                options.UseMySQL(ConnectionString);
            }
            );

            builder.Services.AddCors(options => 
            {

                options.AddPolicy(MyAllowSpecificOrigins,
                                      policy =>
                                      {
                                          policy.WithOrigins("http://localhost:3000",
                                                             "http://localhost:3000")
                                                                .AllowAnyHeader()
                                                                .AllowAnyMethod();
                                      });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.MapControllers();

            app.Run();
        }
    }
}
//Scaffold-DbContext "server=localhost;database=houseofmysteries;user=root;password=;sslmode=none;" mysql.entityframeworkcore -outputdir Models –f