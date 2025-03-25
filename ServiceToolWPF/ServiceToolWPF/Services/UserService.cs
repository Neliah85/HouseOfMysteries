using System.Net.Http;
using System.Text;
using System.Text.Json;
using ServiceToolWPF.Classes;

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
                string json = JsonSerializer.Serialize(confirmReg,JsonSerializerOptions.Default);
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
    }
}
