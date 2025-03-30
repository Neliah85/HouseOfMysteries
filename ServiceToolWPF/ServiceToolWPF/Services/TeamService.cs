using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ServiceToolWPF.Classes;
using ServiceToolWPF.Models;

namespace ServiceToolWPF.Services
{
    public class TeamService
    {
        public static SendLogEvent sendLogEvent = new SendLogEvent();
        public static async Task<List<Team>> GetAllTeam(HttpClient httpClient, string token) 
        {
            try
            {
                List<Team>teams = new List<Team>(); 
                string url = $"{httpClient.BaseAddress}Teams/{token}";
                sendLogEvent.SendLog(url);
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
    }
}
