using ServiceToolWPF.Classes;
using ServiceToolWPF.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace ServiceToolWPF.Services
{
    public class RoleService
    {
        #region Events
        public static SendLogEvent sendLogEvent = new SendLogEvent();
        public static RefreshEvent refreshEvent = new RefreshEvent();
        #endregion
        #region GetAllRoles
        public static async Task<List<Role>> GetAllRoles(HttpClient httpClient, string token)
        {
            try
            {
                List<Role>? roles = new List<Role>();
                string url = $"{httpClient.BaseAddress}Roles/{token}";
                //sendLogEvent.SendLog(url);
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
        #endregion
        #region AddNewRole
        public static async Task<string> AddNewRole(HttpClient httpClient, string token, string roleName)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Roles/{token},{roleName}";
                string json = JsonSerializer.Serialize(roleName);
                //sendLogEvent.SendLog(url);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    refreshEvent.Refresh("GetAllRoles");
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
        #endregion
        #region UpdateRole
        public static async Task<string> UpdateRole(HttpClient httpClient, string token, Role role)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Roles/{token}";
                string json = JsonSerializer.Serialize(role, JsonSerializerOptions.Default);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, request);
                string r = response.Content.ReadAsStringAsync().Result.ToString();
                sendLogEvent.SendLog(r);
                refreshEvent.Refresh("GetAllRoles");
                return r;
            }
            catch (Exception ex)
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }
        }
        #endregion
        #region DeleteRole
        public static async Task<string> DeleteRole(HttpClient httpClient, string token, int id)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Roles/{token}?id={id}";
                var response = await httpClient.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    refreshEvent.Refresh("GetAllRoles");
                }
                string r = await response.Content.ReadAsStringAsync();
                sendLogEvent.SendLog(r);                
                return r;
            }
            catch (Exception ex)
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }
        }
        #endregion
    }
}
