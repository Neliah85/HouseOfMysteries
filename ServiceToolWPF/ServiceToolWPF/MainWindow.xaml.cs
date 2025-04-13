using ServiceToolWPF.Classes;
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
        string userSalt = "";
        string userHash = "";
        string lastAction = "";
        public static bool loggedIn = false;
        public static LoggedInUserDTO loggedInUser;
        public static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("http://localhost:5131/"),
        };

        //Booking
        public static string[] bookingTime = new string[7] { "09:00:00", "10:30:00", "12:00:00", "13:30:00", "15:00:00", "16:30:00", "18:00:00" };
        public static string[] rooms = new string[9] { "Menekülés az iskolából", "A pedellus bosszúja", "A tanári titkai", "A takarítónő visszanéz", "Szabadulás Kódja", "Időcsapda", "KódX Szoba", "Kalandok Kamrája", "Titkok Labirintusa" };
        //Challenge
        public static string[] ranking = new string[11] { "All", "Top 1", "Top 2", "Top 3", "Top 4", "Top 5", "Top 6", "Top 7", "Top 8", "Top 9", "Top 10" };
        public static bool bookingDataIsUpdated = false;
        public static bool userDataIsUpdated = false;
        public static bool teamsDataIsUpdated = false;
        public static bool rolesDataIsUpdated = false;
        public static bool roomsDataIsUpdated = false;
        public bool IsNumber(string number)
        {
            string mask = "0123456789";
            if (number.Length == 1 && mask.Contains(char.Parse(number)) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region Constuctor
        public MainWindow()
        {
            InitializeComponent();
            //Events
            LoginService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            LogoutService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            BookingService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            BookingService.refreshEvent.Refreshed += RefreshEvent_Refreshed;
            UserService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            UserService.refreshEvent.Refreshed += RefreshEvent_Refreshed;
            TeamService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            TeamService.refreshEvent.Refreshed += RefreshEvent_Refreshed;
            RoleService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            RoleService.refreshEvent.Refreshed += RefreshEvent_Refreshed;
            RoomService.sendLogEvent.LogSent += SendLogEvent_LogSent;
            RoomService.refreshEvent.Refreshed += RefreshEvent_Refreshed;

            ResetLoggedInUser();
            unTextCheck();
            passTextCheck();
            RegNameTextCheck();
            RegUsernameTextCheck();
            RegPhoneTextCheck();
            RegEmailTextCheck();
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
            //Users

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
        #region Login input test
        private bool LoginInputTest()
        {
            bool result = true;
            if (txbUserName.Text == "")
            {
                txbUserName.Background = Brushes.LightPink;
                result = false;
            }
            if (txbPassword.Password == "")
            {
                txbPassword.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }
        #endregion
        #region Login
        private void UserLogin()
        {
            if (LoginInputTest())
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
                                //WriteLog(l);
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
            var result = MessageBox.Show("Are you sure you want to delete the log?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) { lbxLog.Items.Clear(); }
        }
        private void btnSaveLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "txt | *.txt";
                saveFileDialog1.Title = "Save log";
                string dateTime = DateTime.Now.ToString();
                dateTime = dateTime.Trim();
                dateTime = dateTime.Replace('.', '_');
                saveFileDialog1.FileName = "log_" + dateTime;//DateTime.Now.ToString();
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
        #region Refresh 
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source == tabControl)
            {
                lastAction = "";
            }
        }
        private void RefreshEvent_Refreshed(object sender, string e)
        {
            if (lastAction == "" && e != "") { lastAction = e; }

            if (lastAction == "CheckBooking") { CheckBooking(); }
            if (lastAction == "GetAllBooking") { GetAllBooking(); }
            if (lastAction == "GetAllUsers") { GetAllUsers(); }
            if (lastAction == "GetUserByUserName") { GetUserByUserName(); }
            if (lastAction == "GetAllTeams") { GetAllTeams(); }
            if (lastAction == "GetAllRoles") { GetAllRoles(); }
            if (lastAction == "GetAllRooms") { GetAllRooms(); }
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
        #region Registration input test
        private bool RegistrationInputTest()
        {
            bool result = true;
            if (txbRegUserName.Text == "")
            {
                txbRegUserName.Background = Brushes.LightPink;
                result = false;
            }
            if (txbRegPassword1.Password == "")
            {
                txbRegPassword1.Background = Brushes.LightPink;
                result = false;
            }
            if (txbRegPassword2.Password == "")
            {
                txbRegPassword2.Background = Brushes.LightPink;
                result = false;
            }
            if (txbRegName.Text == "")
            {
                txbRegName.Background = Brushes.LightPink;
                result = false;
            }
            if (txbRegEmail.Text == "")
            {
                txbRegEmail.Background = Brushes.LightPink;
                result = false;
            }
            if (txbRegPhone.Text == "")
            {
                txbRegPhone.Background = Brushes.LightPink;
                result = false;
            }

            return result;
        }
        #endregion
        #region Registration
        private void Registration()
        {
            if (RegistrationInputTest())
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

        private void txbBookingId_GotFocus(object sender, RoutedEventArgs e)
        {
            BookingIdTextCheck();
        }
        private void txbBookingId_TextChanged(object sender, TextChangedEventArgs e)
        {
            BookingIdTextCheck();
        }
        private void BookingIdTextCheck()
        {
            if (txbBookingId.Text == "")
            {
                txbBookingId.Background = null;
            }
            else
            {
                txbBookingId.Background = Brushes.White;
            }
        }
        //ResultTime input 
        private void txbResultTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumber(e.Text))
            {
                switch (txbResultTime.Text.Length)
                {
                    case 0: if (int.Parse(e.Text) > 0) e.Handled = true; break;
                    case 1: if (int.Parse(e.Text) > 1) e.Handled = true; break;
                    case 3: if (int.Parse(e.Text) > 5) e.Handled = true; break;
                    case 6: if (int.Parse(e.Text) > 5) e.Handled = true; break;
                }
            }
            else
            {
                e.Handled = true;
            }
            InsertSeparator();
        }

        private void txbResultTime_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            InsertSeparator();
        }
        private void InsertSeparator()
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
            txbResultTime.CaretIndex = txbResultTime.Text.Length + 1;
            if (e.Key == Key.Left) { e.Handled = true; }
            if (e.Key == Key.Tab) { e.Handled = true; }
            if (e.Key == Key.Space) { e.Handled = true; }
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
        private void BookingClearInputs()
        {
            txbComment.Text = "";
            txbBookingId.Text = "";
            txbResultTime.Text = "";
        }
        #endregion
        #region Booking input test
        private bool BookingIdTest()
        {
            bool result = true;
            if (txbBookingId.Text == "")
            {
                txbBookingId.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }

        private bool BookingResultTimeTest()
        {
            bool result = true;
            if (txbResultTime.Text.Length < 8)
            {
                txbResultTime.Background = Brushes.LightPink;
                result = false;
            }
            return result;
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
            lastAction = "CheckBooking";
            WriteLog($"[Check booking >> Date: {dtpBookingDate.DisplayDate.ToShortDateString()}]");
            dgrBookingData.ItemsSource = await BookingService.CheckBooking(sharedClient, loggedInUser.Token, DateTime.Parse(dtpBookingDate.SelectedDate.Value.ToShortDateString()), cmbRooms.SelectedIndex + 1);
            BookingClearInputs();
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
            newBooking.BookingDate = GetBookingDateTime();
            newBooking.TeamId = loggedInUser.TeamId;
            newBooking.RoomId = cmbRooms.SelectedIndex + 1;
            newBooking.Comment = txbComment.Text;
            await BookingService.NewBooking(sharedClient, loggedInUser.Token, newBooking);
            BookingClearInputs();
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
            BookingClearInputs();
        }
        //Get all booking
        private void btnGetAllBooking_Click(object sender, RoutedEventArgs e)
        {
            GetAllBooking();
        }
        private async void GetAllBooking()
        {
            lastAction = "GetAllBooking";
            WriteLog("[Get all booking]");
            dgrBookingData.ItemsSource = await BookingService.GetAllBooking(sharedClient, loggedInUser.Token);
            BookingClearInputs();
            bookingDataIsUpdated = true;
        }
        //Delete booking
        private async void btnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (BookingIdTest())
            {
                if (MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int id = int.Parse(txbBookingId.Text);
                    WriteLog($"[Delete booking >> Id={id}]");
                    await BookingService.DeleteBooking(sharedClient, loggedInUser.Token, id);
                    BookingClearInputs();
                }
            }
        }
        //Get challenge result
        private void btnTeamCompetition_Click(object sender, RoutedEventArgs e)
        {
            GetTeamCompetition();
        }
        public async void GetTeamCompetition()
        {
            WriteLog($"Get challenge result. >> Room: {cmbRoomsChallenge.SelectedItem.ToString()}; Ranking: {cmbRanking.SelectedItem.ToString()};");
            dgrChallengeData.ItemsSource = await BookingService.GetChallengeResult(sharedClient, cmbRoomsChallenge.SelectedIndex + 1, cmbRanking.SelectedIndex);
        }
        //SaveResult
        private void btnSaveResult_Click(object sender, RoutedEventArgs e)
        {
            SaveResult();
        }
        public async void SaveResult()
        {
            if (BookingResultTimeTest())
            {
                SaveResultDTO saveResult = new SaveResultDTO();
                saveResult.BookingDate = GetBookingDateTime();
                saveResult.RoomId = cmbRooms.SelectedIndex + 1;
                saveResult.Result = TimeSpan.Parse(txbResultTime.Text);
                WriteLog($"[Save result >> {txbResultTime.Text}]");
                await BookingService.SaveResult(sharedClient, loggedInUser.Token, saveResult);
            }
        }
        public DateTime GetBookingDateTime()
        {
            TimeSpan time = TimeSpan.Zero;
            if (cmbBookingTime.SelectedIndex > -1)
            {
                time = TimeSpan.Parse(bookingTime[cmbBookingTime.SelectedIndex]);
            }
            return DateTime.Parse($"{dtpBookingDate.SelectedDate.Value.ToShortDateString()} " + time);
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
                dtpBookingDate.SelectedDate = (DateTime)row.BookingDate;
                cmbRooms.SelectedIndex = row.RoomId.Value - 1;
                cmbBookingTime.SelectedIndex = GetIndexOfBookingTime(row.BookingDate.Value.TimeOfDay);
                txbBookingId.Text = row.BookingId.ToString();
                txbComment.Text = row.Comment;

            }
        }
        private void dgrBookingData_LayoutUpdated(object sender, EventArgs e)
        {
            if (dgrBookingData.Items.Count > 0 && bookingDataIsUpdated == true)
            {
                dgrBookingData.ScrollIntoView(dgrBookingData.Items[dgrBookingData.Items.Count - 1]);
                bookingDataIsUpdated = false;
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


        private void txbUsersTeamName_GotFocus(object sender, RoutedEventArgs e)
        {
            UsersTeamNameTextCheck();
        }

        private void txbUsersTeamName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersTeamNameTextCheck();
        }

        private void UsersTeamNameTextCheck()
        {
            if (txbUsersTeamName.Text == "")
            {
                txbUsersTeamName.Background = null;
            }
            else
            {
                txbUsersTeamName.Background = Brushes.White;
            }
        }
        private void txbUsersRoleId_GotFocus(object sender, RoutedEventArgs e)
        {
            UsersRoleIdTextCheck();
        }

        private void txbUsersRoleId_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersRoleIdTextCheck();
        }
        private void UsersRoleIdTextCheck()
        {
            if (txbUsersRoleId.Text == "")
            {
                txbUsersRoleId.Background = null;
            }
            else
            {
                txbUsersRoleId.Background = Brushes.White;
            }
        }

        private void txbUsersRoleName_GotFocus(object sender, RoutedEventArgs e)
        {
            UsersRoleNameTextCheck();
        }

        private void txbUsersRoleName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersRoleNameTextCheck();
        }
        private void UsersRoleNameTextCheck()
        {
            if (txbUsersRoleName.Text == "")
            {
                txbUsersRoleName.Background = null;
            }
            else
            {
                txbUsersRoleName.Background = Brushes.White;
            }
        }

        private void txbUserId_GotFocus(object sender, RoutedEventArgs e)
        {
            UserIdTextCheck();
        }
        private void txbUserId_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserIdTextCheck();
        }
        private void UserIdTextCheck()
        {
            if (txbUserId.Text == "")
            {
                txbUserId.Background = null;
            }
            else
            {
                txbUserId.Background = Brushes.White;
            }
        }

        private void txbUsersTeamId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumber(e.Text)) { e.Handled = true; }
        }

        private void txbUsersTeamId_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void txbUsersTeamId_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void txbUsersRoleId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumber(e.Text)) { e.Handled = true; }
        }

        private void txbUsersRoleId_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void txbUsersRoleId_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }
        private void UsersClearInputs()
        {
            txbUsersUserName.Text = "";
            txbUsersRealName.Text = "";
            txbUsersEmail.Text = "";
            txbUsersPhone.Text = "";
            txbUsersTeamId.Text = "";
            txbUsersRoleId.Text = "";
            txbUsersRoleName.Text = "";
            txbUsersTeamId.Text = "";
            txbUsersTeamName.Text = "";
            txbUserId.Text = "";
        }
        #endregion
        #region Users input test
        private bool UsersInputTest()
        {
            bool result = true;
            if (txbUsersRealName.Text == "")
            {
                txbUsersRealName.Background = Brushes.LightPink;
                result = false;
            }
            if (txbUsersUserName.Text == "")
            {
                txbUsersUserName.Background = Brushes.LightPink;
                result = false;
            }
            if (txbUsersEmail.Text == "")
            {
                txbUsersEmail.Background = Brushes.LightPink;
                result = false;
            }
            if (txbUsersPhone.Text == "")
            {
                txbUsersPhone.Background = Brushes.LightPink;
                result = false;
            }
            if (txbUsersRoleId.Text == "")
            {
                txbUsersRoleId.Background = Brushes.LightPink;
                result = false;
            }
            if (txbUserId.Text == "")
            {
                txbUserId.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }

        private bool UsersIdTest()
        {
            bool result = true;
            if (txbUserId.Text == "")
            {
                txbUserId.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }
        private bool UsersNameTest()
        {
            bool result = true;
            if (txbUsersUserName.Text == "")
            {
                txbUsersUserName.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }


        #endregion
        #region Users
        //Get all users
        private void btnUsersGetAll_Click(object sender, RoutedEventArgs e)
        {
            GetAllUsers();
        }
        public async void GetAllUsers()
        {
            WriteLog("[Get all users]");
            lastAction = "GetAllUsers";
            dgrUserData.ItemsSource = await UserService.GetAllUsers(sharedClient, loggedInUser.Token);
            userDataIsUpdated = true;
            UsersClearInputs();
        }

        //Get user by username
        private void btnUsersGetByUserName_Click(object sender, RoutedEventArgs e)
        {
            GetUserByUserName();
        }

        private async void GetUserByUserName()
        {
            if (UsersNameTest())
            {
                WriteLog($"[Get user by username >> UserName={txbUsersUserName.Text}]");
                lastAction = "GetUserByUserName";
                List<User?> l = new List<User?>();
                dgrUserData.ItemsSource = l;
                var response = await UserService.GetUserByUserName(sharedClient, loggedInUser.Token, txbUsersUserName.Text);
                if (response != null)
                {
                    l.Add(response);
                    dgrUserData.Items.Refresh();
                }
                UsersClearInputs();
            }
        }
        //Delete user
        private void btnUserDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersIdTest())
            {
                if (MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int id = int.Parse(txbUserId.Text);
                    WriteLog($"[Delete users >> Id={id}]");
                    _ = UserService.DeleteUser(sharedClient, loggedInUser.Token, id);
                    UsersClearInputs();
                }
            }
        }
        //Update user
        private void btnUsersUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersInputTest())
            {
                UserDTO user = new UserDTO();
                user.RealName = txbUsersRealName.Text;
                user.NickName = txbUsersUserName.Text;
                user.Email = txbUsersEmail.Text;
                user.Phone = txbUsersPhone.Text;
                user.RoleId = int.Parse(txbUsersRoleId.Text);
                user.UserId = int.Parse(txbUserId.Text);
                user.Hash = userHash;
                user.Salt = userSalt;
                if (txbUsersTeamId.Text == "")
                {
                    user.TeamId = null;
                }
                else
                {
                    user.TeamId = int.Parse(txbUsersTeamId.Text);
                }
                WriteLog($"[Update user >> Id={user.UserId}]");
                _ = UserService.UpdateUser(sharedClient, loggedInUser.Token, user);
                UsersClearInputs();
            }
        }
        private void dgrUserData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = dgrUserData.SelectedIndex;
            if (index > -1)
            {
                User row = (User)dgrUserData.Items.GetItemAt(index);
                txbUsersUserName.Text = row.NickName;
                txbUsersRealName.Text = row.RealName;
                txbUsersEmail.Text = row.Email;
                txbUsersPhone.Text = row.Phone;
                txbUsersTeamId.Text = row.TeamId.ToString();
                txbUsersTeamName.Text = row.TeamName;
                txbUsersRoleId.Text = row.RoleId.ToString();
                txbUsersRoleName.Text = row.RoleName;
                txbUserId.Text = row.UserId.ToString();
                userHash = row.Hash;
                userSalt = row.Salt;
            }
        }
        private void dgrUserData_LayoutUpdated(object sender, EventArgs e)
        {
            if (dgrUserData.Items.Count > 0 && userDataIsUpdated == true)
            {
                dgrUserData.ScrollIntoView(dgrUserData.Items[dgrUserData.Items.Count - 1]);
                userDataIsUpdated = false;
            }
        }
        #endregion
        #region Teams input mask settings
        private void txbTeamsTeamName_GotFocus(object sender, RoutedEventArgs e)
        {
            TeamsTeamNameTextCheck();
        }
        private void txbTeamsTeamName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TeamsTeamNameTextCheck();
        }
        private void TeamsTeamNameTextCheck()
        {
            if (txbTeamsTeamName.Text == "")
            {
                txbTeamsTeamName.Background = null;
            }
            else
            {
                txbTeamsTeamName.Background = Brushes.White;
            }
        }
        private void txbTeamId_TextChanged(object sender, TextChangedEventArgs e)
        {
            TeamIdTextCheck();
        }
        private void TeamIdTextCheck()
        {
            if (txbTeamId.Text == "")
            {
                txbTeamId.Background = null;
            }
            else
            {
                txbTeamId.Background = Brushes.White;
            }
        }

        private void txbTeamsUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            TeamsUserNameTextCheck();
        }
        private void txbTeamsUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TeamsUserNameTextCheck();
        }
        private void TeamsUserNameTextCheck()
        {
            if (txbTeamsUserName.Text == "")
            {
                txbTeamsUserName.Background = null;
            }
            else
            {
                txbTeamsUserName.Background = Brushes.White;
            }
        }
        private void TeamsClearInputs()
        {
            txbTeamsTeamName.Text = "";
            txbTeamId.Text = "";
            txbTeamsUserName.Text = "";
        }
        #endregion
        #region Teams input test
        private bool TeamsIdTest()
        {
            bool result = true;
            if (txbTeamId.Text == "")
            {
                txbTeamId.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }
        private bool TeamsTeamNameTest()
        {
            bool result = true;
            if (txbTeamsTeamName.Text == "")
            {
                txbTeamsTeamName.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }
        private bool TeamsUserNameTest()
        {
            bool result = true;
            if (txbTeamsUserName.Text == "")
            {
                txbTeamsUserName.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }


        #endregion
        #region Teams
        private void btnGetAllTeams_Click(object sender, RoutedEventArgs e)
        {
            GetAllTeams();
        }
        private async void GetAllTeams()
        {
            WriteLog("[Get all teams]");
            lastAction = "GetAllTeams";
            dgrTeamsData.ItemsSource = await TeamService.GetAllTeams(sharedClient, loggedInUser.Token);
            teamsDataIsUpdated = true;
            TeamsClearInputs();
        }
        private async void btnTeamsAddNewTeam_Click(object sender, RoutedEventArgs e)
        {
            if (TeamsTeamNameTest())
            {
                WriteLog($"[Add new team >> Team name: {txbTeamsTeamName.Text}]");
                await TeamService.AddNewTeam(sharedClient, loggedInUser.Token, txbTeamsTeamName.Text);
                TeamsClearInputs();
            }
        }

        private async void btnTeamsRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (TeamsTeamNameTest())
            {
                WriteLog($"[Team registration >> Team name: {txbTeamsTeamName.Text}]");
                await TeamService.TeamRegistration(sharedClient, loggedInUser.Token, txbTeamsTeamName.Text);
                TeamsClearInputs();
            }
        }

        private async void btnTeamsUpdateTeam_Click(object sender, RoutedEventArgs e)
        {
            if (TeamsTeamNameTest() && TeamsIdTest())
            {
                WriteLog($"[Update team >> Team name: {txbTeamsTeamName.Text}]");
                Team team = new Team() { TeamId = int.Parse(txbTeamId.Text), TeamName = txbTeamsTeamName.Text };
                await TeamService.UpdateTeam(sharedClient, loggedInUser.Token, team);
                TeamsClearInputs();
            }
        }
        private async void btnTeamsDeleteTeam_Click(object sender, RoutedEventArgs e)
        {
            if (TeamsIdTest())
            {
                if (MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    WriteLog($"[Delete team >> Team id: {txbTeamId.Text} Team name: {txbTeamsTeamName.Text}]");
                    await TeamService.DeleteTeam(sharedClient, loggedInUser.Token, int.Parse(txbTeamId.Text));
                    TeamsClearInputs();
                }
            }
        }
        private async void btnTeamsAddUserToTeam_Click(object sender, RoutedEventArgs e)
        {
            if (TeamsUserNameTest())
            {
                WriteLog($"[Add user to team >> User name: {txbTeamsUserName.Text} Team name: {txbTeamsTeamName.Text}]");
                await TeamService.AddUserToTeam(sharedClient, loggedInUser.Token, txbTeamsUserName.Text, txbTeamsTeamName.Text);
                TeamsClearInputs();
            }
        }
        private void dgrTeamsData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = dgrTeamsData.SelectedIndex;
            if (index > -1)
            {
                Team team = new Team();
                team = (Team)dgrTeamsData.Items.GetItemAt(index);
                txbTeamsTeamName.Text = team.TeamName;
                txbTeamId.Text = team.TeamId.ToString();
            }
        }
        private void dgrTeamsData_LayoutUpdated(object sender, EventArgs e)
        {
            if (dgrTeamsData.Items.Count > 0 && teamsDataIsUpdated == true)
            {
                dgrTeamsData.ScrollIntoView(dgrTeamsData.Items[dgrTeamsData.Items.Count - 1]);
                teamsDataIsUpdated = false;
            }
        }
        #endregion
        #region Roles input mask settings
        private void txbRolesRoleName_GotFocus(object sender, RoutedEventArgs e)
        {
            RolesRoleNameTextCheck();
        }
        private void txbRolesRoleName_TextChanged(object sender, TextChangedEventArgs e)
        {
            RolesRoleNameTextCheck();
        }
        private void RolesRoleNameTextCheck()
        {
            if (txbRolesRoleName.Text == "")
            {
                txbRolesRoleName.Background = null;
            }
            else
            {
                txbRolesRoleName.Background = Brushes.White;
            }
        }
        private void txbRoleId_TextChanged(object sender, TextChangedEventArgs e)
        {
            RoleIdTextCheck();
        }
        private void RoleIdTextCheck()
        {
            if (txbRoleId.Text == "")
            {
                txbRoleId.Background = null;
            }
            else
            {
                txbRoleId.Background = Brushes.White;
            }
        }
        private void RolesClearInputs()
        {
            txbRolesRoleName.Text = "";
            txbRoleId.Text = "";
        }
        #endregion
        #region Roles input test
        private bool RolesRoleNameTest()
        {
            bool result = true;
            if (txbRolesRoleName.Text == "")
            {
                txbRolesRoleName.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }

        private bool RolesRoleIdTest()
        {
            bool result = true;
            if (txbRoleId.Text == "")
            {
                txbRoleId.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }


        #endregion
        #region Roles
        private void btnGetAllRoles_Click(object sender, RoutedEventArgs e)
        {
            GetAllRoles();
        }
        private async void GetAllRoles()
        {
            WriteLog("[Get all roles]");
            lastAction = "GetAllRoles";
            dgrRolesData.ItemsSource = await RoleService.GetAllRoles(sharedClient, loggedInUser.Token);
            rolesDataIsUpdated = true;
            RolesClearInputs();
        }
        private async void btnRolesAddNewRole_Click(object sender, RoutedEventArgs e)
        {
            if (RolesRoleNameTest())
            {
                WriteLog($"[Add new role >> Role name: {txbRolesRoleName.Text}]");
                await RoleService.AddNewRole(sharedClient, loggedInUser.Token, txbRolesRoleName.Text);
                RolesClearInputs();
            }
        }
        private async void btnRolesUpdateRole_Click(object sender, RoutedEventArgs e)
        {
            if (RolesRoleNameTest() && RolesRoleIdTest())
            {
                WriteLog($"[Update role >> Role name: {txbRolesRoleName.Text}]");
                Role role = new Role() { RoleId = int.Parse(txbRoleId.Text), RoleName = txbRolesRoleName.Text };
                await RoleService.UpdateRole(sharedClient, loggedInUser.Token, role);
                RolesClearInputs();
            }
        }
        private async void btnRolesDeleteRole_Click(object sender, RoutedEventArgs e)
        {
            if (RolesRoleIdTest())
            {
                if (MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    WriteLog($"[Delete role >> Role id: {txbRoleId.Text} Role name: {txbRolesRoleName.Text}]");
                    await RoleService.DeleteRole(sharedClient, loggedInUser.Token, int.Parse(txbRoleId.Text));
                    RolesClearInputs();
                }
            }
        }
        private void dgrRolesData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = dgrRolesData.SelectedIndex;
            if (index > -1)
            {
                Role role = new Role();
                role = (Role)dgrRolesData.Items.GetItemAt(index);
                txbRolesRoleName.Text = role.RoleName;
                txbRoleId.Text = role.RoleId.ToString();
            }
        }
        private void dgrRolesData_LayoutUpdated(object sender, EventArgs e)
        {
            if (dgrRolesData.Items.Count > 0 && rolesDataIsUpdated == true)
            {
                dgrRolesData.ScrollIntoView(dgrRolesData.Items[dgrRolesData.Items.Count - 1]);
                rolesDataIsUpdated = false;
            }
        }
        #endregion
        #region Rooms input mask settings
        private void txbRoomsRoomName_GotFocus(object sender, RoutedEventArgs e)
        {
            RoomsRoomNameTextCheck();
        }

        private void txbRoomsRoomName_TextChanged(object sender, TextChangedEventArgs e)
        {
            RoomsRoomNameTextCheck();
        }
        private void RoomsRoomNameTextCheck()
        {
            if (txbRoomsRoomName.Text == "")
            {
                txbRoomsRoomName.Background = null;
            }
            else
            {
                txbRoomsRoomName.Background = Brushes.White;
            }
        }
        private void txbRoomId_TextChanged(object sender, TextChangedEventArgs e)
        {
            RoomsRoomIdTextCheck();
        }
        private void RoomsRoomIdTextCheck()
        {
            if (txbRoomId.Text == "")
            {
                txbRoomId.Background = null;
            }
            else
            {
                txbRoomId.Background = Brushes.White;
            }
        }
        private void RoomsClearInputs()
        {
            txbRoomsRoomName.Text = "";
            txbRoomId.Text = "";
        }
        #endregion
        #region Rooms input test
        private bool RoomsRoomNameTest()
        {
            bool result = true;
            if (txbRoomsRoomName.Text == "")
            {
                txbRoomsRoomName.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }
        private bool RoomsRoomIdTest()
        {
            bool result = true;
            if (txbRoomId.Text == "")
            {
                txbRoomId.Background = Brushes.LightPink;
                result = false;
            }
            return result;
        }

        #endregion
        #region Rooms
        private void btnGetAllRooms_Click(object sender, RoutedEventArgs e)
        {
            GetAllRooms();
        }
        private async void GetAllRooms()
        {
            WriteLog("[Get all rooms]");
            lastAction = "GetAllRooms";
            dgrRoomsData.ItemsSource = await RoomService.GetAllRooms(sharedClient, loggedInUser.Token);
            roomsDataIsUpdated = true;
            RoomsClearInputs();
        }
        private async void btnRoomsAddNewRoom_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsRoomNameTest())
            {
                WriteLog($"[Add new room >> Room name: {txbRoomsRoomName.Text}]");
                await RoomService.AddNewRoom(sharedClient, loggedInUser.Token, txbRoomsRoomName.Text);
                RoomsClearInputs();
            }
        }
        private async void btnRoomsUpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsRoomNameTest() && RoomsRoomIdTest())
            {
                WriteLog($"[Update room >> Room name: {txbRoomsRoomName.Text}]");
                Room room = new Room() { RoomId = int.Parse(txbRoomId.Text), RoomName = txbRoomsRoomName.Text };
                await RoomService.UpdateRoom(sharedClient, loggedInUser.Token, room);
                RoomsClearInputs();
            }
        }
        private async void btnRoomsDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsRoomIdTest())
            {
                if (MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    WriteLog($"[Delete room >> Room id: {txbRoomId.Text} Room name: {txbRoomsRoomName.Text}]");
                    await RoomService.DeleteRoom(sharedClient, loggedInUser.Token, int.Parse(txbRoomId.Text));
                    RoomsClearInputs();
                }
            }
        }
        private void dgrRoomsData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = dgrRoomsData.SelectedIndex;
            if (index > -1)
            {
                Room room = new Room();
                room = (Room)dgrRoomsData.Items.GetItemAt(index);
                txbRoomsRoomName.Text = room.RoomName;
                txbRoomId.Text = room.RoomId.ToString();
            }
        }
        private void dgrRoomsData_LayoutUpdated(object sender, EventArgs e)
        {
            if (dgrRoomsData.Items.Count > 0 && roomsDataIsUpdated == true)
            {
                dgrRoomsData.ScrollIntoView(dgrRoomsData.Items[dgrRoomsData.Items.Count - 1]);
                roomsDataIsUpdated = false;
            }
        }
        #endregion
    }
}