using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceToolWPF.Classes;

namespace ServiceToolWPF.Services
{
    internal class LoginService
    {

        public static string GetSalt(HttpClient httpClient, string loginName)
        {
            try
            {
                string uri = $"{httpClient.BaseAddress}Login/GetSalt/{loginName}";
                var response = httpClient.PostAsync(uri, null).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        
        public static string Login(HttpClient httpClient, string loginName, string tmpHash)
        {
            string url = $"{httpClient.BaseAddress}Login";
            LoginDTO loginUser = new LoginDTO { LoginName = loginName, TmpHash = tmpHash };
            string json = JsonSerializer.Serialize(loginUser);

            var request = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage? response = httpClient.PostAsync(url, request).Result;
            if (response.IsSuccessStatusCode)
            {
                MainWindow.message ="";
            }
            else
            {                    
                MainWindow.message = $"Login failed: {response.StatusCode}";
                MainWindow.loggedInUser = null;
                MainWindow.loggedIn = false;
            }
            return response.Content.ReadAsStringAsync().Result;
        }       


    }
}
