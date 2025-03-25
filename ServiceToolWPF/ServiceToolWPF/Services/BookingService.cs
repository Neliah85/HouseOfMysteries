using ServiceToolWPF.Models;
using ServiceToolWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using System.Windows.Controls;
using System.Globalization;
using ServiceToolWPF.DTOs;
using System.DirectoryServices.ActiveDirectory;
using System.Text.Json.Nodes;


namespace ServiceToolWPF.Services
{
    public class BookingService
    {
        public static SendLogEvent sendLogEvent = new SendLogEvent();
        public static async Task<List<Booking>?> CheckBooking(HttpClient httpClient, string token, DateTime day, int roomId)
        {
            try
            {
                List<Booking> bookings = new List<Booking>();
                string date = DateOnly.FromDateTime(day).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string parameters = $"{token}?day={date}&roomId={roomId}";
                string url = $"{httpClient.BaseAddress}Booking/CheckBooking";
                var response = await httpClient.GetAsync(url + parameters); 
                if (response.IsSuccessStatusCode)
                {
                    bookings = await response.Content.ReadFromJsonAsync<List<Booking>>();
                    sendLogEvent.SendLog($"Successful query! {bookings.Count} reservations found on the requested date.");
                    return bookings;
                }
                else
                {
                    sendLogEvent.SendLog(response.Content.ReadAsStringAsync().Result);
                    return (null);
                }
            }
            catch (Exception ex)
            {
                sendLogEvent.SendLog(ex.Message);
                return null;
            }
        }

        public static async Task<List<Booking>?> GetAllBooking(HttpClient httpClient, string token)
        {
            try
            {   
                List<Booking>bookings = new List<Booking>();
                string url = $"{httpClient.BaseAddress}Booking/GetAllBooking{token}";
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    bookings = await response.Content.ReadFromJsonAsync<List<Booking>>();
                    sendLogEvent.SendLog($"Successful query! {bookings.Count} reservations found.");
                    return bookings;
                }
                else 
                {
                    sendLogEvent.SendLog(response.Content.ReadAsStringAsync().Result);
                    return (null);
                }                
            }
            catch (Exception ex)
            {              
                sendLogEvent.SendLog(ex.Message);
                return null;
            }
        }

        public static async Task<string> NewBooking(HttpClient httpClient, string token, BookingDTO newBooking)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Booking/NewBooking/{token}?";
                string json = JsonSerializer.Serialize(newBooking);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                sendLogEvent.SendLog(url);
                HttpResponseMessage? response = await httpClient.PostAsync(url, request);
                if (response.IsSuccessStatusCode)
                {
                    sendLogEvent.SendLog(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    sendLogEvent.SendLog($"New booking failed: {response.StatusCode}");
                }
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
