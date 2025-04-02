using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceToolWPF.Classes;
using ServiceToolWPF.Models;

namespace ServiceToolWPF.Services
{
    public class TeamService
    {
        public static SendLogEvent sendLogEvent = new SendLogEvent();
        public static RefreshEvent refreshEvent = new RefreshEvent();


        public static async Task<List<Team>> GetAllTeam(HttpClient httpClient, string token) 
        {
            try
            {
                List<Team>teams = new List<Team>(); 
                string url = $"{httpClient.BaseAddress}Teams/{token}";
                //sendLogEvent.SendLog(url);
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    teams = await response.Content.ReadFromJsonAsync<List<Team>>();
                    sendLogEvent.SendLog($"Successful query! {teams.Count} team(s) found.");
                    return teams;
                }
                else
                {
                    sendLogEvent.SendLog(response.Content.ReadAsStringAsync().Result);
                    return null;
                }      
            }
            catch (Exception ex)
            {
                sendLogEvent.SendLog(ex.Message);
                return null;
            }        
        }

        public static async Task<string> AddNewTeam(HttpClient httpClient, string token, string teamName)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Teams/{token},{teamName}";
                string json = JsonSerializer.Serialize(teamName);
                sendLogEvent.SendLog(url);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, null);
                if (response.IsSuccessStatusCode)
                {                  
                    refreshEvent.Refresh("GetAllTeams");
                }
                sendLogEvent.SendLog(response.Content.ReadAsStringAsync().Result);
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }
        }
    }
}
