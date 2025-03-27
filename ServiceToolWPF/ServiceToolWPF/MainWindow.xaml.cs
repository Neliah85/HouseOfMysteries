﻿using ServiceToolWPF.Classes;
using ServiceToolWPF.Services;
using ServiceToolWPF.Models;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Security.Cryptography;
using System.Net.Http;
using Microsoft.Win32;
using System.IO;
using System;
using ServiceToolWPF.DTOs;


namespace ServiceToolWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        #region Declarations
        string SALT;
        string HASH;
        public static bool loggedIn = false;
        public static LoggedInUserDTO? loggedInUser;
        public static HttpClient? sharedClient = new()
        {
            BaseAddress = new Uri("http://localhost:5131/"),
        };

        ////Booking
        //public static List<Booking> bookings = new List<Booking>();
        public static string[] bookingTime = new string[7] {"09:00:00","10:30:00","12:00:00","13:30:00","15:00:00","16:30:00","18:00:00" };
        //public static string[] bookingTime = new string[7] { "09:00", "10:30", "12:00", "13:30", "15:00", "16:30", "18:00" };
        public static string[] rooms = new string[9] { "Menekülés az iskolából", "A pedellus bosszúja", "A tanári titkai", "A takarítónő visszanéz", "Szabadulás Kódja", "Időcsapda", "KódX Szoba", "Kalandok Kamrája", "Titkok Labirintusa" };



        #endregion
        #region Constuctor
        public MainWindow()
        {
            InitializeComponent();
            UserService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            LoginService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            LogoutService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            BookingService.sendLogEvent.LogSent += SendLogEvent_LogSent;

            ResetLoggedInUser();

            unTextCheck();
            passTextCheck();
            RegNameTextCheck();
            RegUsernameTextCheck();
            RegPhoneTextCheck();
            RegEmailTextCheck();
            //RegPassword1TextCheck();
            //RegPassword2TextCheck();


            //Booking
            cmbBookingTime.ItemsSource = bookingTime;
            cmbBookingTime.SelectedIndex = 0;
            dtpBookingDate.SelectedDate = DateTime.Now;
            cmbRooms.ItemsSource = rooms;
            cmbRooms.SelectedIndex = 0;

        }
        #endregion
        #region Generate Salt/Hash
        static int SaltLength = 64;
        public static string GenerateSalt()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string salt = "";
            for (int i = 0; i < SaltLength; i++)
            {
                salt += chars[random.Next(chars.Length)];
            }
            return salt;
        }
        public static string CreateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        #endregion
        #region UserName/Password textbox mask settings
        private void txbUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            unTextCheck();
        }
        private void txbUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            unTextCheck();
        }
        private void unTextCheck()
        {
            if (txbUserName.Text == "")
            {
                txbUserName.Background = null;
            }
            else
            {
                txbUserName.Background = txbUserNameBackgrnd.Background;
            }
        }

        private void txbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            passTextCheck();
        }
        private void txbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passTextCheck();
        }
        private void passTextCheck()
        {
            if (txbPassword.Password == "")
            {
                txbPassword.Background = null;
            }
            else
            {
                txbPassword.Background = txbPasswordBackgrnd.Background;
            }
        }
        #endregion
        #region Login trigger events
        private void txbUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { UserLogin(); }
        }
        private void txbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { UserLogin(); }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (btnLogin.Content.ToString() == "Login")
            {
                UserLogin();
            }
            else
            {
                UserLogout();
            }
        }
        #endregion
        #region Login
        private void UserLogin()
        {
            if (txbUserName.Text != "" && txbPassword.Password != "")
            {
                WriteLog($"[Login: {txbUserName.Text}]");
                var salt = LoginService.GetSalt(ServiceToolWPF.MainWindow.sharedClient, txbUserName.Text);
                if (salt != "" && salt != "Error")
                {
                    string tmpHash = MainWindow.CreateSHA256(txbPassword.Password + salt);
                    try
                    {
                        string l = LoginService.Login(ServiceToolWPF.MainWindow.sharedClient, txbUserName.Text, tmpHash);
                        if (l != null && l != "Incorrect username or password!")
                        {
                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                            };

                            loggedInUser = JsonSerializer.Deserialize<LoggedInUserDTO>(l, options);
                            if (loggedInUser?.Token != "")
                            {
                                MainWindow.loggedIn = true;
                                txbUserName.Focusable = false;
                                txbUserName.Background = Brushes.LightGreen;
                                txbPassword.Focusable = false;
                                txbPassword.Background = Brushes.LightGreen;
                                btnLogin.Content = "Logout";
                                WriteLog("Login successful!");
                                WriteLog(l);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Login failed: " + ex.Message);
                    }
                }
                else
                {
                    WriteLog("Login failed: Incorrect username or password!");
                }
            }
        }
        #endregion
        #region Logout
        public static void ResetLoggedInUser()
        {
            LoggedInUserDTO user = new LoggedInUserDTO();
            user.Token = Guid.NewGuid().ToString();
            user.RoleId = -1;
            user.Phone = "";
            user.Email = "";
            user.UserId = 0;
            user.RealName = "";
            user.NickName = "";
            user.TeamId = 0;
            loggedInUser = user;
            loggedIn = false;
        }
        public void UserLogout()
        {
            WriteLog("[Logout]");
            string response = LogoutService.Logout(ServiceToolWPF.MainWindow.sharedClient, MainWindow.loggedInUser.NickName);
            txbUserName.Focusable = true;
            txbUserName.Background = Brushes.White;
            txbPassword.Focusable = true;
            txbPassword.Background = Brushes.White;
            btnLogin.Content = "Login";
        }
        #endregion
        #region LogWindow
        private void SendLogEvent_LogSent(object sender, string e)
        {
            WriteLog(e);
        }
        public void WriteLog(string line)
        {
            lbxLog.Items.Add($"{DateTime.Now.ToString()}> {line}");
            lbxLog.ScrollIntoView(lbxLog.Items[lbxLog.Items.Count - 1]);
        }
        private void btnClearLog_Click(object sender, RoutedEventArgs e)
        {
            lbxLog.Items.Clear();
        }
        private void btnSaveLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "txt | *.txt";
                saveFileDialog1.Title = "Save log";
                saveFileDialog1.FileName = "log " + DateTime.Now.ToString();
                if (saveFileDialog1.ShowDialog() == true)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                    foreach (string line in lbxLog.Items)
                    {
                        sw.WriteLine(line);
                    }
                    sw.Close();
                }
            }
            catch
            {
                WriteLog("Save log error!");
            }
        }
        #endregion
        #region Registration input mask settings
        private void txbRegUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            RegUsernameTextCheck();
        }
        private void txbRegUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegUsernameTextCheck();
        }
        private void RegUsernameTextCheck()
        {
            if (txbRegUserName.Text == "")
            {
                txbRegUserName.Background = null;
            }
            else
            {
                txbRegUserName.Background = Brushes.White;
            }
        }
        private void txbRegPassword1_GotFocus(object sender, RoutedEventArgs e)
        {
            RegPassword1TextCheck();
            RegPassword2TextCheck();
        }
        private void txbRegPassword1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            RegPassword1TextCheck();
            RegPassword2TextCheck();
        }
        private void RegPassword1TextCheck()
        {
            if (txbRegPassword1.Password == "")
            {
                txbRegPassword1.Background = null;
            }
            else
            {
                txbRegPassword1.Background = Brushes.White;
            }
        }
        private void txbRegPassword2_GotFocus(object sender, RoutedEventArgs e)
        {
            RegPassword2TextCheck();
        }
        private void txbRegPassword2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            RegPassword2TextCheck();
        }
        private void RegPassword2TextCheck()
        {
            if (txbRegPassword2.Password == "")
            {
                txbRegPassword2.Background = null;
            }
            else
            {
                if (txbRegPassword1.Password == txbRegPassword2.Password)
                {
                    txbRegPassword2.Background = Brushes.White;
                }
                else
                {
                    txbRegPassword2.Background = Brushes.LightPink;
                }
            }
        }

        private void txbRegName_GotFocus(object sender, RoutedEventArgs e)
        {
            RegNameTextCheck();
        }

        private void txbRegName_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegNameTextCheck();
        }
        private void RegNameTextCheck()
        {
            if (txbRegName.Text == "")
            {
                txbRegName.Background = null;
            }
            else
            {
                txbRegName.Background = Brushes.White;
            }
        }

        private void txbRegEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            RegEmailTextCheck();
        }

        private void txbRegEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegEmailTextCheck();
        }

        private void RegEmailTextCheck()
        {
            if (txbRegEmail.Text == "")
            {
                txbRegEmail.Background = null;
            }
            else
            {
                txbRegEmail.Background = Brushes.White;
            }
        }
        private void txbRegPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            RegPhoneTextCheck();
        }

        private void txbRegPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegPhoneTextCheck();
        }

        private void RegPhoneTextCheck()
        {
            if (txbRegPhone.Text == "")
            {
                txbRegPhone.Background = null;
            }
            else
            {
                txbRegPhone.Background = Brushes.White;
            }
        }

        #endregion
        #region Registration trigger events
        private void txbRegUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegPassword1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegPassword2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void txbRegPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { Registration(); }
        }
        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            Registration();
        }
        private void btnConfirmRegistration_Click(object sender, RoutedEventArgs e)
        {
            ConfirmRegistration();
        }
        #endregion
        #region Registration
        private void Registration()
        {
            if (txbRegUserName.Text != "" && txbRegPassword1.Password != "" && txbRegPassword1.Password == txbRegPassword2.Password && txbRegName.Text != "" && txbRegEmail.Text != "" && txbRegPhone.Text != "")
            {
                WriteLog("[Registration]");
                SALT = GenerateSalt();
                UserDTO user = new UserDTO();
                user.UserId = 0;
                user.NickName = txbRegUserName.Text;
                user.RealName = txbRegName.Text;
                user.Email = txbRegEmail.Text;
                user.Phone = txbRegPhone.Text;
                user.RoleId = null;
                user.TeamId = null;
                user.Salt = SALT;
                user.Hash = CreateSHA256(CreateSHA256(txbRegPassword1.Password + SALT));
                UserService.Post(sharedClient, user);
            }
        }
        private void ConfirmRegistration()
        {
            if (txbRegUserName.Text != "" && txbRegEmail.Text != "")
            {
                WriteLog("[Confirm registration]");
                ConfirmRegDTO confirmReg = new ConfirmRegDTO { LoginName = txbRegUserName.Text, Email = txbRegEmail.Text };
                UserService.Post(sharedClient, confirmReg);
            }
        }
        #endregion
        #region Booking
        //CheckBooking
        private async void btnCheckBooking_Click(object sender, RoutedEventArgs e)
        {
            CheckBooking();
        }
        private async void CheckBooking()
        {
            WriteLog($"[Check booking >> Date: {dtpBookingDate.DisplayDate.ToShortDateString()}]");
            dgrBookingData.ItemsSource = await BookingService.CheckBooking(sharedClient, loggedInUser.Token, DateTime.Parse(dtpBookingDate.SelectedDate.Value.ToShortDateString()), cmbRooms.SelectedIndex + 1);
        }
        //New booking
        private async void btnNewBooking_Click(object sender, RoutedEventArgs e)
        {
            NewBooking();
        }
        private async void NewBooking()
        {
            WriteLog($"[New booking >> Room: {cmbRooms.SelectedItem.ToString()} Date: {GetBookingDateTime()}]");
            TimeSpan time = TimeSpan.Parse(bookingTime[cmbBookingTime.SelectedIndex]);
            BookingDTO newBooking = new BookingDTO();
            //newBooking.BookingDate = DateTime.Parse(dtpBookingDate.DisplayDate.ToShortDateString() + " " + time.ToString());
            newBooking.BookingDate = DateTime.Parse(dtpBookingDate.DisplayDate.ToShortDateString() + " " + TimeSpan.Parse(bookingTime[cmbBookingTime.SelectedIndex]).ToString());
            newBooking.TeamId = null;
            newBooking.RoomId = cmbRooms.SelectedIndex + 1;
            newBooking.Comment = "Teszt";
            await BookingService.NewBooking(sharedClient, loggedInUser.Token, newBooking);

        }
        //Clear booking
        private void btnClearBooking_Click(object sender, RoutedEventArgs e)
        {
            ClearBooking();
        }
        private async void ClearBooking() 
        {
            WriteLog("Clear booking");
            ClearBookingDTO clearBooking = new ClearBookingDTO();
            clearBooking.BookingDate = GetBookingDateTime();
            clearBooking.RoomId = cmbRooms.SelectedIndex + 1;
            await BookingService.ClearBooking(sharedClient,loggedInUser.Token,clearBooking);
        }
        //Get all booking
        private void btnGetAllBooking_Click(object sender, RoutedEventArgs e)
        {
            GetAllBooking();
        }
        private async void GetAllBooking()
        {
            WriteLog("[Get all booking]");
            dgrBookingData.ItemsSource = await BookingService.GetAllBooking(sharedClient, loggedInUser.Token);
        }

        //Delete booking
        private async void btnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (txbBookingId.Text != "") 
            {
                int id = int.Parse(txbBookingId.Text);
                WriteLog("[Delete booking]");
                await BookingService.DeleteBooking(sharedClient, loggedInUser.Token, id);
            }   
        }


        public DateTime GetBookingDateTime() 
        { 
            return DateTime.Parse($"{dtpBookingDate.DisplayDate.ToShortDateString()} "+ bookingTime[cmbBookingTime.SelectedIndex]);
        }

        public int GetIndexOfBookingTime(TimeSpan bookingTime) 
        {
            //int index = 0;
            int index = cmbBookingTime.Items.IndexOf(bookingTime.ToString());
            return index;
        }






        private void SetBookingDate()
        {


        }

        private void cmbBookingTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetBookingDate();
        }


        private void dgrBookingData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = dgrBookingData.SelectedIndex;
            if (index > -1)
            {
                Booking row = (Booking)dgrBookingData.Items.GetItemAt(index);
                dtpBookingDate.DisplayDate = (DateTime)row.BookingDate;
                cmbRooms.SelectedIndex = row.RoomId.Value - 1;
                cmbBookingTime.SelectedIndex = GetIndexOfBookingTime(row.BookingDate.Value.TimeOfDay);
                //txbReasultTime.Text = row.BookingDate.ToString().Split(".")[3].Trim(' ').Substring(0, 5);
                txbBookingId.Text = row.BookingId.ToString();

            }
        }











        #endregion


    }
}