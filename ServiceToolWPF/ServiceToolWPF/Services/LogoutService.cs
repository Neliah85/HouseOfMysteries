using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceToolWPF.Services
{
    internal class LogoutService
    {
        public static string Logout(HttpClient httpClient, string userName)
        {
            string url = $"{httpClient.BaseAddress}Logout/{userName}";
            string json = JsonSerializer.Serialize(userName);
            try
            {
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(url, request).Result;
                if (response.IsSuccessStatusCode)
                {
                    MainWindow.message = "";
                    MainWindow.loggedInUser = null;
                    MainWindow.loggedIn = false;
                }
                else
                {
                    MainWindow.message = $"{response.StatusCode} Logout failed!";
                }
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex) 
            {                 
                MainWindow.message = $"{ex.Message} Logout failed!";
                return ex.Message;
            }
        }
    }
}
