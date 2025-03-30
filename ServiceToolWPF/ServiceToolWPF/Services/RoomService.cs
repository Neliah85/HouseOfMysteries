using ServiceToolWPF.Classes;
using ServiceToolWPF.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceToolWPF.Services
{
    public class RoomService
    {
        public static SendLogEvent sendLogEvent = new SendLogEvent();
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






    }
}
