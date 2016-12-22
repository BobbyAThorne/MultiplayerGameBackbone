using GameDataObjects;
using GameLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MultiplayerGameBackBone
{
    /// <summary>
    /// Author Bobby Thorne
    /// Interaction logic for MainWindow.xaml
    /// 
    /// ADDITIONAL IDEAS: 
    /// Message box overlay that uses a thread to update messages for user
    /// would need a DB Table to hold those or a listener like in java. it could use a counter that 
    /// will count down to hide overlay text box that will go back to one when it receives a new 
    /// message.
    /// Have a Table for player creation that will output a number that you can reference digits
    /// for data similar to a RGB Color Numbers
    /// </summary>
    public partial class MainWindow : Window
    {
        //Var for the use throughout the code that is passed between dispatch timers
        bool running = false;
        Player _user = null;
        Server player = null;
       
        bool moveRight = false;
        bool moveLeft = false;
        bool inAirLeft = false;
        bool inAirRight = false;
        bool jump = false;
        int G = 30;
        int Force;
        bool maxjump = false;

        DispatcherTimer dispatcherTimerServerUpdate;
        DispatcherTimer dispatcherTimerMovement;
        DispatcherTimer dispatcherTimerPlayerImage;
        BitmapImage PlayerBitmap;

        List<BitmapImage> BitmapListRight = new List<BitmapImage>();
        List<BitmapImage> BitmapListLeft = new List<BitmapImage>();
        //List<Server> players = new List<Server>();

        //This is the values that will select what image taht is pulled from the array list
        //to make it appear as the player is walking. This must be changed in the same thread 
        //as the one that changes the image or it will be un sync and cause an issue with unordered
        //image changes making the character spastic 
        int left = 0;
        int right = 0;

        //This is the value that the character advances acrossed the screen
        int moveSize = 5;

        int keyUp = 1; //this indicates which was the last key pressed if it was left = 0 or right = 1
        Image PlayerImage = new Image();


        public MainWindow()
        {
            InitializeComponent();
            var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../../Images/gamingIcon.png",
                 UriKind.RelativeOrAbsolute);

            //Sets the Icon at the top left on bar
            this.Icon = BitmapFrame.Create(uri);

            //This fills the right and left bitmap array list used to walk through while the 
            //character is walking starts with the standing left and right then adds the moving
            //images in the for loop
            BitmapListLeft.Add(new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../../Images/Finn/FinnStandingLift.png", UriKind.RelativeOrAbsolute)));
            BitmapListRight.Add(new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../../Images/Finn/FinnStandingRight.png", UriKind.RelativeOrAbsolute)));

            for (int i = 1; i <= 5; i++)
            {
                BitmapListLeft.Add(new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../../Images/Finn/FinnWalkingLeft" + i + ".png", UriKind.RelativeOrAbsolute)));
                BitmapListRight.Add(new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../../Images/Finn/FinnWalkingRight" + i + ".png", UriKind.RelativeOrAbsolute)));
            }
        }

        //This timer manages the movement of the player by updating the database
        //and the updating the direction that the player is moving in the local var
        //but NOT what is shown on screen. This is pulled in another thread.
        private void dispatcherTimerMovement_Tick(object sender, EventArgs e)
        {
            var srvmng = new ServerManager();
            //if (player != null) {
            player = srvmng.GetPlayer(_user.UserName);

            if (moveRight && !jump && !maxjump)
            {
                inAirRight = false;
                inAirLeft = false;

                srvmng.PlayerMoved(player.PlayerID, player.LocationX + moveSize, player.LocationY, player.LocationZ, player.Image, player.LocationX, player.LocationY, player.LocationZ);
            }
            else if (moveLeft && !jump && !maxjump)
            {
                inAirRight = false;
                inAirLeft = false;

                srvmng.PlayerMoved(player.PlayerID, player.LocationX - moveSize, player.LocationY, player.LocationZ, player.Image, player.LocationX, player.LocationY, player.LocationZ);
            }

            if (jump && !maxjump && !moveRight && !moveLeft)
            {
                srvmng.PlayerMoved(player.PlayerID, player.LocationX, player.LocationY + moveSize, player.LocationZ, player.Image, player.LocationX, player.LocationY, player.LocationZ);
                if (player.LocationY >= G)
                {

                    maxjump = true;
                }
            }
            if (maxjump && !moveRight & !moveLeft)
            {
                srvmng.PlayerMoved(player.PlayerID, player.LocationX, player.LocationY - moveSize, player.LocationZ, player.Image, player.LocationX, player.LocationY, player.LocationZ);

                if (player.LocationY <= 5)
                {
                    //srvmng.PlayerMoved(player.PlayerID, player.LocationX + moveSize, player.LocationY - moveSize, player.LocationZ, player.Image, player.LocationX, player.LocationY, player.LocationZ);

                    jump = false;
                    maxjump = false;
                }
            }
            if (jump && !maxjump && moveRight)
            {
                inAirRight = true;
                srvmng.PlayerMoved(player.PlayerID, player.LocationX + moveSize, player.LocationY + moveSize, player.LocationZ, player.Image, player.LocationX, player.LocationY, player.LocationZ);
                if (player.LocationY >= G)
                {

                    maxjump = true;
                }
            }
            if (maxjump && moveRight)
            {
                srvmng.PlayerMoved(player.PlayerID, player.LocationX + moveSize, player.LocationY - moveSize, player.LocationZ, player.Image, player.LocationX, player.LocationY, player.LocationZ);
                if (player.LocationY <= 5)
                {
                    inAirLeft = false;
                    inAirRight = false;
                    maxjump = false;
                    jump = false;
                }
            }
            if (jump && !maxjump && moveLeft)
            {
                inAirLeft = true;
                srvmng.PlayerMoved(player.PlayerID, player.LocationX - moveSize, player.LocationY + moveSize, player.LocationZ, player.Image, player.LocationX, player.LocationY, player.LocationZ);
                if (player.LocationY >= G)
                {

                    maxjump = true;
                }
            }
            if (maxjump && moveLeft)
            {
                srvmng.PlayerMoved(player.PlayerID, player.LocationX - moveSize, player.LocationY - moveSize, player.LocationZ, player.Image, player.LocationX, player.LocationY, player.LocationZ);
                if (player.LocationY <= 5)
                {
                    inAirRight = false;
                    inAirLeft = false;
                    maxjump = false;
                    jump = false;
                }
            }

            StatusNotification.Content = "key was pressed: x = " + player.LocationX + " y = " + player.LocationY;
        }

        //This timer is what updates the image and pulls from the local variables that are
        //updated else where to decide what image to draw after it removes it from the 
        //canvas once it has updated it will add one to the direction or return it to 1
        //if it is starting over in the loop or 0 if the character is standing still.
        private void dispatcherTimerPlayerImage_Tick(object sender, EventArgs e)
        {
            if (inAirRight)
            {
                GameView.Children.Remove(PlayerImage);

                try
                {
                    PlayerBitmap = BitmapListRight[2];

                }
                catch
                {
                    left = 1;
                    PlayerBitmap = BitmapListRight[2];
                }
                keyUp = 0;
                PlayerImage.Source = PlayerBitmap;
                PlayerImage.Width = PlayerBitmap.Width;
                PlayerImage.Height = PlayerBitmap.Height;

                GameView.Children.Add(PlayerImage);

                Canvas.SetBottom(PlayerImage, player.LocationY);
                Canvas.SetLeft(PlayerImage, player.LocationX);
            }
            else if (inAirLeft)
            {
                GameView.Children.Remove(PlayerImage);

                try
                {
                    PlayerBitmap = BitmapListLeft[2];

                }
                catch
                {
                    left = 1;
                    PlayerBitmap = BitmapListLeft[2];
                }
                keyUp = 0;
                PlayerImage.Source = PlayerBitmap;

                //should add a way to change player size in settings or in character creation
                //will probably need a table that will save player sizes and ratios
                //could create a multi-digit number to rep size ratios and and it parts it up 
                //when it is pulled similar to a rgb color number
                PlayerImage.Width = PlayerBitmap.Width;
                PlayerImage.Height = PlayerBitmap.Height;

                GameView.Children.Add(PlayerImage);

                Canvas.SetBottom(PlayerImage, player.LocationY);
                Canvas.SetLeft(PlayerImage, player.LocationX);
            }
            else if (moveLeft && !jump)
            {
                GameView.Children.Remove(PlayerImage);
                left += 1;
                try
                {
                    PlayerBitmap = BitmapListLeft[left];
                }
                catch
                {
                    left = 1;
                    PlayerBitmap = BitmapListLeft[left];
                }
                keyUp = 0;
                PlayerImage.Source = PlayerBitmap;

                //should add a way to change player size in settings or in character creation
                //will probably need a table that will save player sizes and ratios
                //could create a multi-digit number to rep size ratios and and it parts it up 
                //when it is pulled similar to a rgb color number
                PlayerImage.Width = PlayerBitmap.Width;
                PlayerImage.Height = PlayerBitmap.Height;

                GameView.Children.Add(PlayerImage);

                Canvas.SetBottom(PlayerImage, player.LocationY);
                Canvas.SetLeft(PlayerImage, player.LocationX);
            }
            else if (moveRight && !jump)
            {
                GameView.Children.Remove(PlayerImage);
                right += 1;
                try
                {
                    PlayerBitmap = BitmapListRight[right];
                }
                catch
                {
                    right = 1;
                    PlayerBitmap = BitmapListRight[right];
                }
                keyUp = 1;
                PlayerImage.Source = PlayerBitmap;
                PlayerImage.Width = PlayerBitmap.Width;
                PlayerImage.Height = PlayerBitmap.Height;

                GameView.Children.Add(PlayerImage);

                Canvas.SetBottom(PlayerImage, player.LocationY);
                Canvas.SetLeft(PlayerImage, player.LocationX);
            } else if (moveRight != true && moveLeft != true)
            {
                if (keyUp == 0)
                {
                    GameView.Children.Remove(PlayerImage);
                    PlayerBitmap = BitmapListLeft[0];
                }
                else
                {
                    GameView.Children.Remove(PlayerImage);
                    PlayerBitmap = BitmapListRight[0];
                }
                PlayerImage.Source = PlayerBitmap;
                PlayerImage.Width = PlayerBitmap.Width;
                PlayerImage.Height = PlayerBitmap.Height;

                GameView.Children.Add(PlayerImage);

                Canvas.SetBottom(PlayerImage, player.LocationY);
                Canvas.SetLeft(PlayerImage, player.LocationX);
            }
        }

        
        private void frmMain_Loader(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
            //List<String> play = new List<String>();
        }

        //This is logic where it will make the item list visible or collapse
        //this still needs to access the DB to pull items owned by player
        private void btnItemList_Click(object sender, RoutedEventArgs e)
        {
            if (ServerListGrid.Visibility == Visibility.Visible)
            {
                ServerListGrid.Visibility = Visibility.Collapsed;
            }
            if (ItemListGrid.Visibility == Visibility.Collapsed || ItemListGrid.Visibility == Visibility.Hidden)
            {
                ItemListGrid.Visibility = Visibility.Visible;
                PlayerImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                PlayerImage.Visibility = Visibility.Visible;
                ItemListGrid.Visibility = Visibility.Collapsed;
            }
        }

        //This is logic where it will make the item list visible or collapse
        //this still needs top labels to identify what the number means for the 
        //admin an add a username label for the other players 
        //Other additions this can use is a teleport to player btn for admin
        //and a messager/friend request for users 
        private void btnServerList_Click(object sender, RoutedEventArgs e)
        {
            ServerManager srvmng = new ServerManager();
            List<Server> players = srvmng.GetPlayersOnServer();
            List<String> playersOnServer = new List<string>();
            if (ItemListGrid.Visibility == Visibility.Visible)
            {
                ItemListGrid.Visibility = Visibility.Collapsed;
            }
            if (ServerListGrid.Visibility == Visibility.Collapsed || ServerListGrid.Visibility == Visibility.Hidden)
            {
                PlayerImage.Visibility = Visibility.Collapsed;
                if (_user.RoleID == 2)
                {
                    
                    foreach (Server i in players)
                    {
                        ServerListGrid.Visibility = Visibility.Visible;
                        ItemList.ItemsSource = playersOnServer;
                        playersOnServer.Add(i.PlayerID + " " + i.UserName + " " + i.LocationX + " " + i.Health);
                    }
                }
                if (_user.RoleID == 0)
                {
                    foreach (Server i in players)
                    {
                        ServerListGrid.Visibility = Visibility.Visible;
                        ItemList.ItemsSource = playersOnServer;
                        playersOnServer.Add(i.UserName);
                    }
                }

            }
            else
            {
                players.Clear();
                playersOnServer.Clear();
                ServerListGrid.Visibility = Visibility.Collapsed;
                PlayerImage.Visibility = Visibility.Visible;
            }

        }

        //Check for the key up events from moving left and right
        //where they are used else where in the program
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (running == true)
            {
                if (!Keyboard.IsKeyDown(Key.Left))
                {
                    moveLeft = false;
                }
                if (!Keyboard.IsKeyDown(Key.Right))
                {
                    moveRight = false;
                }
            }
        }

        //Key down events for the main window will check for down keys for the 
        //hotkeys escape, I, P or pause, Inventory, and player list on server and then
        //also for movement and update local variables that are used else where in the program
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (PauseGrid.Visibility == Visibility.Visible)
                {
                    if (_user == null) { LoginGrid.Visibility = Visibility.Visible; }
                    unpause();
                }
                else
                {
                    pause();
                }

            }

            //Certain buttons will not be recognized if the user is not logged in
            //such as movement and server list and item list
            else if (running == true)
            {
                if(_user.RoleID == 2 && e.Key == Key.Insert)
                {
                    if(ConsoleCommand.Visibility==Visibility.Collapsed || ConsoleCommand.Visibility == Visibility.Hidden)
                    {
                        ConsoleCommand.Visibility = Visibility.Visible;
                    }
                    else { ConsoleCommand.Visibility = Visibility.Collapsed; }
                }
                if (e.Key == Key.I)
                {
                    btnItemList_Click(sender, e);
                }
                if (e.Key == Key.P)
                {
                    btnServerList_Click(sender, e);
                }
                if (Keyboard.IsKeyDown(Key.Left) && Keyboard.IsKeyDown(Key.Up))
                {
                    moveLeft = true;
                    jump = true;
                    keyUp = 0;
                }

                else if (Keyboard.IsKeyDown(Key.Right) && Keyboard.IsKeyDown(Key.Up))
                {
                    moveRight = true;
                    jump = true;
                    keyUp = 1;
                }

                else if (Keyboard.IsKeyDown(Key.Right))
                {
                    moveRight = true;
                    jump = false;
                    keyUp = 1;
                }

                else if (Keyboard.IsKeyDown(Key.Left))
                {
                    moveLeft = true;
                    jump = false;
                    keyUp = 0;
                }
                else if (jump != true)
                {
                    if (Keyboard.IsKeyDown(Key.Up))
                    {

                        jump = true;
                        Force = G;
                    }
                }
            }
        }

        //This is the resume button for the pause menu
        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            unpause();
            if(_user == null)
            {
                ImageCanvas.Visibility = Visibility.Visible;
                LoginGrid.Visibility = Visibility.Visible;
            }
            SignUpGrid.Visibility = Visibility.Collapsed;
           
        }

        //This is the Setting button for the pause menu
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (_user != null)
            {
                GameView.Visibility = Visibility.Visible;
              
            }
            SettingGrid.Visibility = Visibility.Visible;
            ImageCanvas.Visibility = Visibility.Collapsed;
            ChangePasswordGrid.Visibility = Visibility.Hidden;
            ItemListGrid.Visibility = Visibility.Collapsed;
            SignUpGrid.Visibility = Visibility.Collapsed;

        }

        //This is the Logout button for the pause menu
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoggedOut();
            GameView.Children.Remove(PlayerImage);
            //dispatcherTimerRightLeft.Stop();
            try
            {
                dispatcherTimerMovement.Stop();
                dispatcherTimerPlayerImage.Stop();
                dispatcherTimerServerUpdate.Stop();
           
                ServerManager srvmng = new ServerManager();
                srvmng.UpdatePlayerToInactive(player.PlayerID);
            } catch { }
            StatusMessage.Content = "Must log in to continue.";
            StatusNotification.Content = "Logged out.";
            if (_user != null)
            {
                var serMgr = new ServerManager();
                serMgr.UpdatePlayerToInactive(_user.PlayerID);
                _user = null;
            }
        }

        //This is the Exit button for the pause menu
        //Pressing this will check if the player is currently logged in 
        //if so it will go into a logout state so that when it is closed it
        //cleans up before closing.
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            btnLogout_Click(sender, e);
            ServerManager srvmng = new ServerManager();
            if(player!=null)
            {
                srvmng.UpdatePlayerToInactive(player.PlayerID);
            }
            this.Close();
        }

        //Logic for collapsing the grids for certain actions 
        //needs to be updated and checked for mistakes
        private void pause()
        {
            //MenuGrid.Visibility = Visibility.Visible;
            PlayerImage.Visibility = Visibility.Collapsed;
            
            PauseGrid.Visibility = Visibility.Visible;
            btnsGameLists.Visibility = Visibility.Collapsed;
            SettingGrid.Visibility = Visibility.Collapsed;
            ChangePasswordGrid.Visibility = Visibility.Hidden;
            ItemListGrid.Visibility = Visibility.Collapsed;
            ServerListGrid.Visibility = Visibility.Collapsed;
            SignUpGrid.Visibility = Visibility.Collapsed;
            running = false;

        }

        private void unpause()
        {
            //MenuGrid.Visibility = Visibility.Hidden;
            SettingGrid.Visibility = Visibility.Collapsed;
            PauseGrid.Visibility = Visibility.Hidden;
            ChangePasswordGrid.Visibility = Visibility.Hidden;
            PlayerImage.Visibility = Visibility.Visible;
            SignUpGrid.Visibility = Visibility.Collapsed;

            if (_user != null)
            {
                btnsGameLists.Visibility = Visibility.Visible;
                running = true;
            }

        }

        private void LoggedOut()
        {
            //MenuGrid.Visibility = Visibility.Visible;
            btnsGameLists.Visibility = Visibility.Collapsed;
            PauseGrid.Visibility = Visibility.Hidden;
            SettingGrid.Visibility = Visibility.Collapsed;
            LoginGrid.Visibility = Visibility.Visible;
            ChangePasswordGrid.Visibility = Visibility.Hidden;
            ItemListGrid.Visibility = Visibility.Collapsed;
            ImageCanvas.Visibility = Visibility.Visible;
            _user = null;
            running = false;
        }

        private void LoggedIn()
        {
            // MenuGrid.Visibility = Visibility.Hidden;
            btnsGameLists.Visibility = Visibility.Visible;
            SettingGrid.Visibility = Visibility.Collapsed;
            LoginGrid.Visibility = Visibility.Hidden;
            ChangePasswordGrid.Visibility = Visibility.Hidden;
            ImageCanvas.Visibility = Visibility.Collapsed;
            running = true;

        }

        //Login auth user and will start the main game when it is passed
        //It also starts the timers for the player
        private void btnlogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Password;
            var plyMgr = new PlayerManager();
            var serMgr = new ServerManager();
       
            if (_user == null)
            {
                try
                {
                    _user = plyMgr.AuthenticatePlayer(username, password);
                    //MessageBox.Show("Welcome back, " + _user.FirstName);
                    LoggedIn();
                    try
                    {
                        serMgr.AddPlayer(_user.PlayerID);

                        player = serMgr.GetPlayer(_user.UserName);
                        serMgr.UpdatePlayerToActive(_user.PlayerID);
                        btnItemList.Visibility = Visibility.Visible;
                        btnServerList.Visibility = Visibility.Visible;
                        //Menu.Visibility = Visibility.Hidden;
                        txtPassword.Password = "";
                        txtUsername.Text = "";
                        btnLogin.IsDefault = false;

                        GameView.Children.Remove(PlayerImage);
                        PlayerImage = new Image();

                        PlayerBitmap = BitmapListRight[0];

                        PlayerImage.Source = PlayerBitmap;
                        PlayerImage.Width = PlayerBitmap.Width;
                        PlayerImage.Height = PlayerBitmap.Height;

                        GameView.Children.Add(PlayerImage);

                        Canvas.SetBottom(PlayerImage, player.LocationY);
                        Canvas.SetLeft(PlayerImage, player.LocationX);

                        StatusMessage.Content = "Logged in as " + _user.UserName;
                        dispatcherTimerServerUpdate = new DispatcherTimer(); //does new here to reset it so it does not speed up
                        dispatcherTimerServerUpdate.Tick += new EventHandler(dispatcherTimerServerUpdate_Tick);
                        dispatcherTimerServerUpdate.Interval = TimeSpan.FromMilliseconds(1000);
                        dispatcherTimerMovement = new DispatcherTimer(); //does new here to reset it so it does not speed up
                        dispatcherTimerMovement.Tick += new EventHandler(dispatcherTimerMovement_Tick);
                        dispatcherTimerMovement.Interval = TimeSpan.FromMilliseconds(60);
                        dispatcherTimerPlayerImage = new DispatcherTimer(); //does new here to reset it so it does not speed up
                        dispatcherTimerPlayerImage.Tick += new EventHandler(dispatcherTimerPlayerImage_Tick);
                        dispatcherTimerPlayerImage.Interval = TimeSpan.FromMilliseconds(60);
                        txtPassword.Background = Brushes.White;
                        dispatcherTimerServerUpdate.Start();
                        dispatcherTimerMovement.Start();
                        dispatcherTimerPlayerImage.Start();
                        //FindOthers();
                    }
                    catch
                    {
                        throw new Exception("Was not able to add Player to server.");
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Intruder Alert");
                    txtPassword.Background = Brushes.Red;
                    StatusNotification.Content = ex.Message;
                }
            }
            else
            {
                _user = null;
                LoggedOut();
                btnItemList.Visibility = Visibility.Hidden;
                btnServerList.Visibility = Visibility.Hidden;
                //Menu.Visibility = System.Windows.Visibility.Visible;
                btnLogin.Content = "Login";
                btnLogin.IsDefault = true;
                txtUsername.Focus();
                StatusMessage.Content = "You are not logged in. Log in to Continue.";
            }
        }

        //Need to figure out a way to draw others on the screen
        //needs an multi dimentional array that creates a list of keys aka usernames that is associated 
        //for an image set for each player so it will update this when players change their image but
        //walking image changes should not be identical acrossed all users it should be 
        //processed by each user so there is not a issue with syncing

        //another way is to array list with keys associated with usernames and it has image set
        //and also boolean values associated with moveleft and right similar to how we do the local
        //player
        private void FindOthers()
        {
            throw new NotImplementedException();
        }

        //This thread updates the server list at a slower rate then the screen refresh
        //also decides what to display depending on type of user
        private void dispatcherTimerServerUpdate_Tick(object sender, EventArgs e)
        {
            ServerManager srvmng = new ServerManager();
            List<Server> players = srvmng.GetPlayersOnServer();
            List<String> playersOnServer = new List<string>();
            try
            {

                if (_user.RoleID == 2)
                {

                    foreach (Server i in players)
                    {
                        // ServerListGrid.Visibility = Visibility.Visible;
                        ItemList.ItemsSource = playersOnServer;
                        playersOnServer.Add(i.PlayerID + " " + i.UserName + " " + i.LocationX + " " + i.Health);
                    }
                }
                if (_user.RoleID == 0)
                {
                    foreach (Server i in players)
                    {
                        //ServerListGrid.Visibility = Visibility.Visible;
                        ItemList.ItemsSource = playersOnServer;
                        playersOnServer.Add(i.UserName);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        //update Password btn in the settings grid. Checks for whether or not the user
        //is signed in first
        private void btnChangPassword_Click(object sender, RoutedEventArgs e)
        {
            if (_user != null)
            {
                ChangePasswordGrid.Visibility = Visibility.Visible;
            }
            else
            {
                LoggedOut();
                StatusMessage.Content = "You Must be logged in to change your password";
            }
        }

        //update password which will call to the store procedure and will set a status message
        //depending if the db was updated resulting in a 1 returned or 0 with a fail
        private void btnSubmitPassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword change = new ChangePassword(_user);
            try
            {
                int result = change.PasswordChangeSubmit(txtOldPassword.Password, txtNewPassword.Password, txtConfirmPassword.Password);
                if (result == 1)
                {
                    StatusMessage.Content = "Password has been updated";
                    ImageCanvas.Visibility = Visibility.Visible;
                    txtOldPassword.Clear();
                    txtNewPassword.Clear();
                    txtConfirmPassword.Clear();
                    pause();

                }
            } catch (Exception ex)
            {
                StatusMessage.Content = ex.Message;
            }
        }

        private void btnResume_MouseEnter(object sender, MouseEventArgs e)
        {
            btnResume.Background = Brushes.DeepSkyBlue;
        }

        private void btnResume_MouseLeave(object sender, MouseEventArgs e)
        {
            btnResume.Background = Brushes.DarkBlue;
        }

        //need to update this when items are added and a way to rep the 
        //equiped item
        private void btnEquip_Click(object sender, RoutedEventArgs e)
        {

        }

        //Returns to normal state 
        private void btnItemCancel_Click(object sender, RoutedEventArgs e)
        {
            ItemListGrid.Visibility = Visibility.Collapsed;
            PlayerImage.Visibility = Visibility.Visible;
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            LoginGrid.Visibility = Visibility.Collapsed;
            SignUpGrid.Visibility = Visibility.Visible;
            ImageCanvas.Visibility = Visibility.Collapsed;
        }

        //Validates user inputs before updating db for new user
        private void btnSubmitSignUp_Click(object sender, RoutedEventArgs e)
        {
            var username = txtSignUpUsername.Text;
            var password = txtSignUpPassword.Password;
            var email = txtSignUpEmail.Text;
            var ConPassword = txtConfirmPassword.Password;
            int Confirm = 0;
            PlayerManager check = new PlayerManager();
           
            if (check.GetPlayer(username) != null)
            {
                txtSignUpUsername.Background = Brushes.Red;
                StatusMessage.Content = "Username is used by another Player";
            }
            else if (check.GetPlayerViaEmail(email) != null)
            {
                txtSignUpEmail.Background = Brushes.Red;
                txtSignUpUsername.Background = Brushes.White;
                StatusMessage.Content = "Email is used by another Player";
            }
            else if (txtSignUpPassword.Password != txtSingUpConfirmPassword.Password)
            {
                txtSingUpConfirmPassword.Background = Brushes.Red;
                txtSignUpEmail.Background = Brushes.White;
                StatusMessage.Content = "Passwords do not match";
            }
            else
            {
                try
                {
                    Confirm = check.AddPlayer(username, email, password);
                    StatusMessage.Content = "New Player Created!";
                    txtConfirmPassword.Clear();
                    txtNewPassword.Clear();
                    txtOldPassword.Clear();
                    txtSignUpEmail.Clear();
                    txtSignUpUsername.Clear();
                    txtConfirmPassword.Background = Brushes.White;
                    txtNewPassword.Background = Brushes.White;
                    txtOldPassword.Background = Brushes.White;
                    txtSignUpEmail.Background = Brushes.White;
                    txtSignUpUsername.Background = Brushes.White;

                    SignUpGrid.Visibility = Visibility.Collapsed;
                    LoginGrid.Visibility = Visibility.Visible;
                    ImageCanvas.Visibility = Visibility.Visible;
                }
                catch 
                {
                    //StatusMessage.Content = ex.Message;
                    txtSignUpEmail.Background = Brushes.Red;
                    txtSignUpUsername.Background = Brushes.White;
                    StatusMessage.Content = "Email is used by another Player";
                }
            }
        }

        private void btnCloseServerList_Click(object sender, RoutedEventArgs e)
        {
            PlayerImage.Visibility = Visibility.Visible;
            ServerListGrid.Visibility = Visibility.Collapsed;
        }

        //This will clean up if user exits with the X in the top right of window 
        //similar to exit button or if it was to logout
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StatusMessage.Content = "Closing Window.";
            StatusNotification.Content = "Logged out.";

            LoggedOut();
            GameView.Children.Remove(PlayerImage);
            try
            {
                dispatcherTimerMovement.Stop();
                dispatcherTimerPlayerImage.Stop();
                dispatcherTimerServerUpdate.Stop();

                ServerManager srvmng = new ServerManager();
                srvmng.UpdatePlayerToInactive(player.PlayerID);
            }
            catch { }


            if (_user != null)
            {
                var serMgr = new ServerManager();
                serMgr.UpdatePlayerToInactive(_user.PlayerID);
                _user = null;
            }
            
            Thread.Sleep(1000);
            //this.Close();
        }

        //This is for Admin use it still needs to take in the arguement so 
        //it can choose what to do with what is entered
        private void ConsoleCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter)) {
                //This is where I would add a method that checks the arguement
                //to decide what method to apply and the parameters that are passed
                ConsoleCommand.Clear();
                ConsoleCommand.Visibility = Visibility.Collapsed;
                
            }
        }
    }
}
