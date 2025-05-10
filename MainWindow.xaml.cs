using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;
using Microsoft.UI.Windowing;
using Microsoft.UI; // Needed for AppWindow.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Shut_Down_App
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private DispatcherTimer countDownTimer;
        private int counter;
        private AppWindow m_AppWindow;


        public MainWindow()
        {
            this.InitializeComponent();


            //Title Bar Customization
            Title = "Shut Down Program";
            AppWindow m_AppWindow = this.AppWindow;
            m_AppWindow.SetIcon("C:\\Users\\Justin\\Shut_Down_App\\Shut_Down_App\\Shut_Down_App\\Assets\\powerIco.ico");
            AppWindowTitleBar m_TitleBar = AppWindow.TitleBar;
            // Set title bar text color
            m_TitleBar.ForegroundColor = Colors.White; // Color of the title text in the title bar
            // Set title bar background color
            m_TitleBar.BackgroundColor = Colors.Black; // Background color of the title bar
            // Set color of the title bar buttons (minimize, maximize, close)
            m_TitleBar.ButtonForegroundColor = Colors.Black; // Default color of the button icons (e.g., X, -, square)
            m_TitleBar.ButtonBackgroundColor = Colors.LightGray; // Default background color of the title bar buttons
            // Set color when hovering over title bar buttons
            m_TitleBar.ButtonHoverForegroundColor = Colors.White; // Color of button icons when hovered
            m_TitleBar.ButtonHoverBackgroundColor = Colors.DarkGray; // Background color of buttons when hovered
            // Set color when title bar buttons are pressed
            m_TitleBar.ButtonPressedForegroundColor = Colors.Gray; // Color of button icons when pressed
            m_TitleBar.ButtonPressedBackgroundColor = Colors.SlateGray; // Background color of buttons when pressed



            this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(100, 100, 1600, 900));

            countDownTimer = new DispatcherTimer();
            countDownTimer.Interval = TimeSpan.FromSeconds(1);
            countDownTimer.Tick += CountDownTimerTick;
        }

        private void submitButtonClick(object sender, RoutedEventArgs e)
        {
            int hours = int.Parse(hoursTextBox.Text);
            int minutes = int.Parse(minutesTextBox.Text);
            //get seconds
            int seconds = convertToSeconds(hours, minutes);

            accessOS(seconds);
            submitButton.Content = "Shut Down Confirmed";

            //initialize countDownTimer
            counter = seconds;
            countDownTimer.Start();

            lblCountDown.Text = FormatTime(counter);
        }

        //Button Supporting Functions
        public int convertToSeconds(int hours, int minutes)
        {
            return ((hours * 3600) + (minutes * 60));
        }
        public void accessOS(int seconds)
        {
            string command = $"/c shutdown /s /t {seconds}";
            //Opens command prompt and runs shutdown command
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = command;
            process.StartInfo.CreateNoWindow = false; // Show the command prompt window
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
        public void CountDownTimerTick(object sender, object e)
        {
            if (counter > 0 )
            {
                counter--;
                lblCountDown.Text = FormatTime(counter);

                if (counter == 0)
                {
                    countDownTimer.Stop();
                    lblCountDown.Text = "Shutting Down";
                }
            }
        }
        public string FormatTime(int totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            return time.ToString(@"hh\:mm\:ss");
        }
    }
}