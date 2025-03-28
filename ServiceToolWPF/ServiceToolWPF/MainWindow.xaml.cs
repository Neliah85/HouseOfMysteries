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
using System.Linq;


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

        //Booking
        public static string[] bookingTime = new string[7] { "09:00:00", "10:30:00", "12:00:00", "13:30:00", "15:00:00", "16:30:00", "18:00:00" };
        //public static string[] bookingTime = new string[7] { "09:00", "10:30", "12:00", "13:30", "15:00", "16:30", "18:00" };
        public static string[] rooms = new string[9] { "Menekülés az iskolából", "A pedellus bosszúja", "A tanári titkai", "A takarítónő visszanéz", "Szabadulás Kódja", "Időcsapda", "KódX Szoba", "Kalandok Kamrája", "Titkok Labirintusa" };
        //Challenge
        public static string[] ranking = new string[11] { "All", "Top 1", "Top 2", "Top 3", "Top 4", "Top 5", "Top 6", "Top 7", "Top 8", "Top 9", "Top 10" };


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
            //Challenge
            cmbRoomsChallenge.ItemsSource = rooms;
            cmbRoomsChallenge.SelectedIndex = 0;
            cmbRanking.ItemsSource = ranking;
            cmbRanking.SelectedIndex = 0;

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
        #region Booking input mask settings
        private void txbComment_GotFocus(object sender, RoutedEventArgs e)
        {
            BookingCommentTextCheck();
        }

        private void txbComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            BookingCommentTextCheck();
        }

        private void BookingCommentTextCheck()
        {
            if (txbComment.Text == "")
            {
                txbComment.Background = null;
            }
            else
            {
                txbComment.Background = Brushes.White;
            }
        }
        private void txbResultTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            BookingResultTimeTextCheck();
        }
        private void txbResultTime_GotFocus(object sender, RoutedEventArgs e)
        {
            BookingResultTimeTextCheck();
        }

        private void BookingResultTimeTextCheck()
        {
            if (txbResultTime.Text == "")
            {
                txbResultTime.Background = null;
            }
            else
            {
                txbResultTime.Background = Brushes.White;
            }
        }
        private void txbResultTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string mask = "0123456789";
            if (mask.Contains(char.Parse(e.Text)) == false)
            {
                e.Handled = true;
            }
            else
            {
                switch (txbResultTime.Text.Length)
                {
                    case 0: if (int.Parse(e.Text) > 0) e.Handled = true; break;
                    case 1: if (int.Parse(e.Text) > 1) e.Handled = true; break;
                    case 3: if (int.Parse(e.Text) > 5) e.Handled = true; break;
                    case 6: if (int.Parse(e.Text) > 5) e.Handled = true; break;
                }
            }
        }

        private void txbResultTime_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (txbResultTime.Text.Length)
            {
                case 2:
                    {
                        txbResultTime.Text += ":";
                        txbResultTime.CaretIndex = 3;
                    }; break;
                case 5:
                    {
                        txbResultTime.Text += ":";
                        txbResultTime.CaretIndex = 6;
                    }; break;
            }
        }

        private void txbResultTime_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back) { txbResultTime.CaretIndex = txbResultTime.Text.Length+1; }
           
            if (txbResultTime.Text.Length == 3)
            {
                if (e.Key == Key.Back)
                {
                    e.Handled = true;
                    string s = txbResultTime.Text;
                    txbResultTime.Text = s.Remove(1, 2);
                    txbResultTime.CaretIndex = 1;
                }
            }
            if (txbResultTime.Text.Length == 6)
            {
                if (e.Key == Key.Back)
                {
                    e.Handled = true;
                    string s = txbResultTime.Text;
                    txbResultTime.Text = s.Remove(4, 2);
                    txbResultTime.CaretIndex = 4;
                }
            }
        }
        private void txbResultTime_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void txbResultTime_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
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
            newBooking.BookingDate = DateTime.Parse(dtpBookingDate.DisplayDate.ToShortDateString() + " " + TimeSpan.Parse(bookingTime[cmbBookingTime.SelectedIndex]).ToString());
            newBooking.TeamId = loggedInUser.TeamId;
            newBooking.RoomId = cmbRooms.SelectedIndex + 1;
            newBooking.Comment = txbComment.Text;
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
            await BookingService.ClearBooking(sharedClient, loggedInUser.Token, clearBooking);
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
        //Get challenge result
        private void btnTeamCompetition_Click(object sender, RoutedEventArgs e)
        {
            GetTeanCompetition();
        }

        public async void GetTeanCompetition()
        {
            WriteLog($"Get challenge result. >> Room: {cmbRoomsChallenge.SelectedItem.ToString()}; Ranking: {cmbRanking.SelectedItem.ToString()};");
            dgrChallengeData.ItemsSource = await BookingService.GetChallengeResult(sharedClient, cmbRoomsChallenge.SelectedIndex + 1, cmbRanking.SelectedIndex);
        }
        public DateTime GetBookingDateTime()
        {
            TimeSpan time = TimeSpan.Zero;
            if (cmbBookingTime.SelectedIndex > -1)
            {
                time = TimeSpan.Parse(bookingTime[cmbBookingTime.SelectedIndex]);
            }
            return DateTime.Parse($"{dtpBookingDate.DisplayDate.ToShortDateString()} " + time);
        }

        public int GetIndexOfBookingTime(TimeSpan bookingTime)
        {
            int index = cmbBookingTime.Items.IndexOf(bookingTime.ToString());
            return index;
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
                txbBookingId.Text = row.BookingId.ToString();
                txbComment.Text = row.Comment;

            }
        }














        #endregion
        #region Users input mask settings
        private void txbUsersUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            UsersUserNameTextCheck();
        }
        private void txbUsersUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersUserNameTextCheck();
        }
        private void UsersUserNameTextCheck()
        {
            if (txbUsersUserName.Text == "")
            {
                txbUsersUserName.Background = null;
            }
            else
            {
                txbUsersUserName.Background = Brushes.White;
            }
        }
        private void txbUsersRealName_GotFocus(object sender, RoutedEventArgs e)
        {
            UsersRealNameTextCheck();
        }
        private void txbUsersRealName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersRealNameTextCheck();
        }
        private void UsersRealNameTextCheck()
        {
            if (txbUsersRealName.Text == "")
            {
                txbUsersRealName.Background = null;
            }
            else
            {
                txbUsersRealName.Background = Brushes.White;
            }
        }
        private void txbUsersEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            UsersEmailTextCheck();
        }
        private void txbUsersEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersEmailTextCheck();
        }
        private void UsersEmailTextCheck()
        {
            if (txbUsersEmail.Text == "")
            {
                txbUsersEmail.Background = null;
            }
            else
            {
                txbUsersEmail.Background = Brushes.White;
            }
        }
        private void txbUsersPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            UsersPhoneTextCheck();
        }
        private void txbUsersPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersPhoneTextCheck();
        }
        private void UsersPhoneTextCheck()
        {
            if (txbUsersPhone.Text == "")
            {
                txbUsersPhone.Background = null;
            }
            else
            {
                txbUsersPhone.Background = Brushes.White;
            }
        }
        private void txbUsersTeamId_GotFocus(object sender, RoutedEventArgs e)
        {
            UsersTeamIdTextCheck();
        }
        private void txbUsersTeamId_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersTeamIdTextCheck();
        }
        private void UsersTeamIdTextCheck()
        {
            if (txbUsersTeamId.Text == "")
            {
                txbUsersTeamId.Background = null;
            }
            else
            {
                txbUsersTeamId.Background = Brushes.White;
            }
        }

        #endregion

    }
}