using ServiceToolWPF.Classes;
using ServiceToolWPF.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace ServiceToolWPF.Services
{
    public class RoleService
    {
        public static SendLogEvent sendLogEvent = new SendLogEvent();
        public static RefreshEvent refreshEvent = new RefreshEvent();
        public static async Task<List<Role>> GetAllRoles(HttpClient httpClient, string token)
        {
            try
            {
                List<Role> roles = new List<Role>();
                string url = $"{httpClient.BaseAddress}Roles/{token}";
                sendLogEvent.SendLog(url);
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    roles = await response.Content.ReadFromJsonAsync<List<Role>>();
                    sendLogEvent.SendLog($"Successful query! {roles.Count} role(s) found.");
                    return roles;
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
