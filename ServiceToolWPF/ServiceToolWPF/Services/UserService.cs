﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ServiceToolWPF.Classes;

namespace ServiceToolWPF.Services
{
    class UserService
    {
        public static async Task<string> Post(HttpClient httpClient, UserDTO user)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                string uj = JsonSerializer.Serialize(user, options);
                string url = $"{httpClient.BaseAddress}Registry";
                var request = new StringContent(uj, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, request);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    MainWindow.message = content;   
                    return content;
                }
                else
                {
                    MainWindow.message = $"Error: {response.StatusCode} {response.Content.Headers} {content}";
                    return $"Error: {response.StatusCode} {response.Content.Headers} {content}";
                }
            }
            catch (Exception ex)
            {
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
                    MainWindow.message = content;
                    return content;
                }
                else
                {
                    MainWindow.message = $"Error: {response.StatusCode} {response.Content.Headers} {content}";
                    return $"Error: {response.StatusCode} {response.Content.Headers} {content}";
                }
            }
            catch (Exception ex)
            {
                MainWindow.message = ex.Message;
                return ex.Message;
            }
        }





    }
}
