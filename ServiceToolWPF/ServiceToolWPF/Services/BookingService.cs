using ServiceToolWPF.Models;
using ServiceToolWPF.Classes;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using System.Globalization;
using ServiceToolWPF.DTOs;

namespace ServiceToolWPF.Services
{
    public class BookingService
    {
        #region Events
        public static SendLogEvent sendLogEvent = new SendLogEvent();
        public static RefreshEvent refreshEvent = new RefreshEvent();
        #endregion
        #region CheckBooking
        public static async Task<List<Booking>?> CheckBooking(HttpClient httpClient, string token, DateTime day, int roomId)
        {
            try
            {
                List<Booking>? bookings = new List<Booking>();
                string date = DateOnly.FromDateTime(day).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string parameters = $"{token}?day={date}&roomId={roomId}";
                string url = $"{httpClient.BaseAddress}Booking/CheckBooking";
                var response = await httpClient.GetAsync(url + parameters); 
                if (response.IsSuccessStatusCode)
                {
                    bookings = await response.Content.ReadFromJsonAsync<List<Booking>>();
                    sendLogEvent.SendLog($"Successful query! {bookings.Count} reservation(s) found on the requested date.");
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
        #endregion
        #region GetAllBooking
        public static async Task<List<Booking>?> GetAllBooking(HttpClient httpClient, string token)
        {
            try
            {   
                List<Booking>?bookings = new List<Booking>();
                string url = $"{httpClient.BaseAddress}Booking/GetAllBooking{token}";
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    bookings = await response.Content.ReadFromJsonAsync<List<Booking>>();
                    sendLogEvent.SendLog($"Successful query! {bookings.Count} reservation(s) found.");
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
        #endregion
        #region NewBooking
        public static async Task<string> NewBooking(HttpClient httpClient, string token, BookingDTO newBooking)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Booking/NewBooking/{token}?";
                string json = JsonSerializer.Serialize(newBooking);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                //sendLogEvent.SendLog(url);
                var response = await httpClient.PostAsync(url, request);
                sendLogEvent.SendLog(response.Content.ReadAsStringAsync().Result);
                refreshEvent.Refresh("CheckBooking");
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                sendLogEvent.SendLog(ex.Message);
                return ex.Message;
            }
        }
        #endregion
        #region DeleteBooking
        public static async Task<string> DeleteBooking(HttpClient httpClient,string token, int bookingId)
        {
            try
            {
                Booking booking = new Booking();
                booking.BookingId = bookingId;  
                string url = $"{httpClient.BaseAddress}Booking/";
                string p = $"{token}?bookingId={bookingId}";
                var response = await httpClient.DeleteAsync(url + p);
                string r = response.Content.ReadAsStringAsync().Result;
                sendLogEvent.SendLog(r);
                refreshEvent.Refresh("CheckBooking");
                return r; 
            }
            catch (Exception ex) 
            { 
                sendLogEvent.SendLog($"{ex.Message}");
                return ex.Message;
            }
        }
        #endregion
        #region ClearBooking
        public static async Task<string> ClearBooking(HttpClient httpClient, string token, ClearBookingDTO clearBooking)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Booking/ClearBooking/{token}";
                string json = JsonSerializer.Serialize(clearBooking);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                //sendLogEvent.SendLog(url);
                var response = await httpClient.PutAsync(url, request);
                string r = response.Content.ReadAsStringAsync().Result;
                sendLogEvent.SendLog(r);
                refreshEvent.Refresh("CheckBooking");
                return r;
            }
            catch(Exception ex) 
            {
                sendLogEvent.SendLog($"{ex.Message}");
                return ex.Message;
            }
        }
        #endregion
        #region GetChalengeResult
        public static async Task<List<Booking>?> GetChallengeResult(HttpClient httpClient, int roomId, int limit)
        {
            try
            {
                List<Booking>? bookings = new List<Booking>(); 
                string url = $"{httpClient.BaseAddress}Booking/TeamCompetition/";
                string p = $"{roomId}?limit={limit}";
                //sendLogEvent.SendLog(url + p);
                var response = await httpClient.GetAsync(url+p);
                if (response.IsSuccessStatusCode)
                {
                    bookings = await response.Content.ReadFromJsonAsync<List<Booking>>();
                    sendLogEvent.SendLog($"Successful query! {bookings.Count} result(s) found.");
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
        #endregion
        #region SaveResult
        public static async Task<string> SaveResult(HttpClient httpClient,string token, SaveResultDTO saveResult)
        {
            try
            {
                string url = $"{httpClient.BaseAddress}Booking/SaveResult/{token}";
                string json = JsonSerializer.Serialize(saveResult);
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                //sendLogEvent.SendLog(url);
                var response = await httpClient.PutAsync(url, request);
                string r = response.Content.ReadAsStringAsync().Result;
                sendLogEvent.SendLog(r);
                refreshEvent.Refresh("CheckBooking");
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
