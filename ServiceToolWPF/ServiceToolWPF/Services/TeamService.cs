using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


        public static async Task<List<Team>> GetAllTeams(HttpClient httpClient, string token) 
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


        public static async Task<string> DeleteTeam(HttpClient httpClient, string token, int id)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Teams/{token}?teamId={id}";               
                var response = await httpClient.DeleteAsync(url);
                string r = await response.Content.ReadAsStringAsync();
                sendLogEvent.SendLog(r);
                refreshEvent.Refresh("GetAllTeams");
                return r;
            }
            catch (Exception ex) 
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }
        }

        public static async Task<string> UpdateTeam(HttpClient httpClient, string token, Team team)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Teams/{token}/";
                string json = JsonSerializer.Serialize(team, JsonSerializerOptions.Default);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url,request);

                string r = response.Content.ReadAsStringAsync().Result.ToString();
                sendLogEvent.SendLog(r);
                refreshEvent.Refresh("GetAllTeams");
                return r;
            }
            catch (Exception ex) 
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }               
        }

        public static async Task<string> AddUserToTeam(HttpClient httpClient, string token, string userName, string teamName)
        {
            try
            {
                string url = httpClient.BaseAddress + $"Teams/AddUserToTeam/{token}?userName={userName}&teamName={teamName}";
                var response = await httpClient.PutAsync(url,null);
                sendLogEvent.SendLog(url);
                string r = response.Content.ReadAsStringAsync().Result.ToString();
                sendLogEvent.SendLog(r);
                return r;
            }
            catch (Exception ex) 
            { 
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;  
            }      
        }

        public static async Task<string> TeamRegistration(HttpClient httpClient, string token, string teamName)
        {
            try
            {            
                string url= httpClient.BaseAddress + $"Teams/TeamRegistration/{token},{WebUtility.UrlEncode(teamName)}";
                var response = await httpClient.PostAsync(url,null);
                sendLogEvent.SendLog(url);
                string r = response.Content.ReadAsStringAsync().Result.ToString();
                sendLogEvent.SendLog(r);
                refreshEvent.Refresh("GetAllTeams");
                return r;
            }
            catch (Exception ex) 
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }
        }





    }
}
