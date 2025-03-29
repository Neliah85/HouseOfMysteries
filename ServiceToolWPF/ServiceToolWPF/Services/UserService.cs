using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HouseOfMysteries.Models;
using ServiceToolWPF.Classes;
using ServiceToolWPF.Models;

namespace ServiceToolWPF.Services
{
    public class UserService
    {
        public static SendLogEvent sendLogEvent = new SendLogEvent();
        public static async Task<string> Post(HttpClient httpClient, UserDTO user)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                string json = JsonSerializer.Serialize(user, options);
                string url = $"{httpClient.BaseAddress}Registry";
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, request);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    sendLogEvent.SendLog(content);
                    return content;
                }
                else
                {
                    sendLogEvent.SendLog($"Error: {response.StatusCode} {response.Content.Headers} {content}");
                    return $"Error: {response.StatusCode} {response.Content.Headers} {content}";
                }
            }
            catch (Exception ex)
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }
        }
        public static async Task<string> Post(HttpClient httpClient, ConfirmRegDTO confirmReg)
        {
            try
            {
                string json = JsonSerializer.Serialize(confirmReg, JsonSerializerOptions.Default);
                string url = $"{httpClient.BaseAddress}Registry/Confirm";
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, request);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    sendLogEvent.SendLog(content);
                    return content;
                }
                else
                {
                    sendLogEvent.SendLog(response.Content.ReadAsStringAsync().Result);
                    return $"Error: {response.StatusCode} {response.Content.Headers} {content}";
                }
            }
            catch (Exception ex)
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }
        }




        public static async Task<List<User>?> GetAllUsers(HttpClient httpClient, string token)
        {
            try
            {
                List<User>? users = new List<User>();
                string url = $"{httpClient.BaseAddress}Users/{token}";
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    users = await response.Content.ReadFromJsonAsync<List<User>>();
                    sendLogEvent.SendLog($"Successful query! {users.Count} user(s) found.");
                    return users;
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

        public static async Task<User?> GetUserByUserName(HttpClient httpClient, string token, string userName)
        {
            try
            {
                User? user = new User();
                string url = $"{httpClient.BaseAddress}Users/GetByUserName/{token},{userName}";
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    user = await response.Content.ReadFromJsonAsync<User>();
                    sendLogEvent.SendLog($"Successful query!");
                    return user;
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
        public static async Task<string> DeleteUser(HttpClient httpClient, string token, int userId)
        {
            try
            {
                User user = new User();
                string url = $"{httpClient.BaseAddress}Users/";
                string p = $"{token},{userId}";
                var response = await httpClient.DeleteAsync(url + p);
                string r = response.Content.ReadAsStringAsync().Result;
                sendLogEvent.SendLog(r);
                return r;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task<string> UpdateUser(HttpClient httpClient, string token, UserDTO user)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Users/UpdateUser/{token}";
                string json = JsonSerializer.Serialize(user);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, request);
                string r = response.Content.ReadAsStringAsync().Result;
                sendLogEvent.SendLog(r);
                return r;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
