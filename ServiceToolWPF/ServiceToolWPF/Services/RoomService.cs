using ServiceToolWPF.Classes;
using ServiceToolWPF.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ServiceToolWPF.Services
{
    public class RoomService
    {
        public static SendLogEvent sendLogEvent = new SendLogEvent();
        public static RefreshEvent refreshEvent = new RefreshEvent();
        public static async Task<List<Room>> GetAllRooms(HttpClient httpClient, string token)
        {
            try
            {
                List<Room> rooms = new List<Room>();
                string url = $"{httpClient.BaseAddress}Rooms/{token}";
                sendLogEvent.SendLog(url);
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    rooms = await response.Content.ReadFromJsonAsync<List<Room>>();
                    sendLogEvent.SendLog($"Successful query! {rooms.Count} room(s) found.");
                    return rooms;
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

        public static async Task<string> AddNewRoom(HttpClient httpClient, string token, string roomName)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Rooms/{token},{roomName}";
                string json = JsonSerializer.Serialize(roomName);
                sendLogEvent.SendLog(url);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    refreshEvent.Refresh("GetAllRooms");
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

        public static async Task<string> UpdateRoom(HttpClient httpClient, string token, Room room)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Rooms/{token}/";
                string json = JsonSerializer.Serialize(room, JsonSerializerOptions.Default);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, request);

                string r = response.Content.ReadAsStringAsync().Result.ToString();
                sendLogEvent.SendLog(r);
                refreshEvent.Refresh("GetAllRooms");
                return r;
            }
            catch (Exception ex)
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }
        }

        public static async Task<string> DeleteRoom(HttpClient httpClient, string token, int id)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Rooms/{token}?id={id}";
                var response = await httpClient.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    refreshEvent.Refresh("GetAllRooms");
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



    }
}
