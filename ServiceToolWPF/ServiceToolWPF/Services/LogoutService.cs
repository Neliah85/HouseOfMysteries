using ServiceToolWPF.Classes;
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
        public static SendLogEvent sendLogEvent = new SendLogEvent();
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
                    sendLogEvent.SendLog("Logout succesful!");
                    MainWindow.loggedInUser = null;
                    MainWindow.loggedIn = false;
                }
                else
                {
                    sendLogEvent.SendLog($"Logout failed! {response.StatusCode}");
                }
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex) 
            {
                sendLogEvent.SendLog($"Logout failed! {ex.Message}");
                return ex.Message;
            }
        }
    }
}
