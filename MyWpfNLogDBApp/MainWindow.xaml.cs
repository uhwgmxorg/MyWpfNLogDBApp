using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MyWpfNLogDBApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Install-NuGet-Package in Visual Studio Package Manager Console:
    /// We need:
    /// Install-Package NLog -Version 5.1.0
    /// Install-Package System.Data.SqlClient -Version 4.8.5
    /// Install-Package NLog.Database -Version 5.1.0    
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly NLog.Logger _logger;

        #region INotify Changed Properties  
        private string? message;
        public string? Message
        {
            get { return message; }
            set { SetField(ref message, value, nameof(Message)); }
        }

        // Template for a new INotify Changed Property
        // for using CTRL-R-R
        private string? xxx;
        public string? Xxx
        {
            get { return xxx; }
            set { SetField(ref xxx, value, nameof(Xxx)); }
        }
        #endregion

        /// <summary>
        /// Construtor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _logger = NLog.LogManager.GetCurrentClassLogger();
            NLog.GlobalDiagnosticsContext.Set("Application", "This is an example to store some text or other data in NLog");

#if DEBUG
            Title += "    Debug Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
#else
            Title += "    Release Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
#endif
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_6_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_6_Click(object sender, RoutedEventArgs e)
        {
            Message = "Create a user handled Stack Overflow";

            CatchStackOverflow1();
        }
        public void CatchStackOverflow1()
        {
            try
            {
                throw new StackOverflowException();
            }
            catch (StackOverflowException ex)
            {
                // Executes and handles the exception.
                // User code continues
                _logger.Error(ex.ToString());
                System.Console.Beep();
            }
        }

        /// <summary>
        /// Button_5_Click
        /// This doesn't really work in C# and .Net see:
        /// https://learn.microsoft.com/en-us/archive/blogs/jaredpar/when-can-you-catch-a-stackoverflowexception
        /// and Button_6_Click()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_5_Click(object sender, RoutedEventArgs e)
        {
            Message = "Create a Stackoverflow";

            try
            {
                Foo();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                System.Console.Beep();
            }
        }

        /// <summary>
        /// Button_4_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_4_Click(object sender, RoutedEventArgs e)
        {
            Message = "Divide by 0";
            int b = 1, c = 0;

            try
            {
                int a = b / c;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                System.Console.Beep();
            }
        }

        /// <summary>
        /// Button_3_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            Message = "Create an Exception";

            Exception myExcetion = new("MyException");

            try
            {
                throw myExcetion;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                System.Console.Beep();
            }
        }

        /// <summary>
        /// Button_2_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_2_Click(object sender, RoutedEventArgs e)
        {
            Message = "You pressed Button #2 and create an Error Message Beep()";
            _logger.Error(Message);
            System.Console.Beep();
        }

        /// <summary>
        /// Button_1_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            Message = "You pressed Button #1 Beep()";
            _logger.Info(Message);
            System.Console.Beep();
        }

        /// <summary>
        /// Button_Close_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// Window_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _logger.Info(String.Format("{0} started", Application.Current.MainWindow.GetType().Assembly));
        }

        /// <summary>
        /// Lable_Message_MouseDown
        /// Clear Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lable_Message_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info(String.Format("Delete Message Output"));
            Message = "";
        }

        /// <summary>
        /// Window_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            string x = NLog.GlobalDiagnosticsContext.Get("Application");  // necessary for the next line
            _logger.Info($"(From GlobalDiagnosticsContext --> {x}");      // $"()" replacement for String.Format

            _logger.Info(String.Format("{0} terminated", Application.Current.MainWindow.GetType().Assembly));
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// foo
        /// Just create a SackOverflow
        /// </summary>
        private void Foo()
        {
            Foo();
        }

        /// <summary>
        /// SetField
        /// for INotify Changed Properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
