﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace Network_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string code = "";
        ButtonFunctions buttonFunctions = new ButtonFunctions();
        DataBinder data = new DataBinder();

        public MainWindow()
        {
            DataContext = data;
            InitializeComponent();
        }


        //Button click event, different methods triggered by selected item in the menu
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //IP Check function
            if (listBox1.SelectedIndex.Equals(0))
            {

                //ProgBar.IsIndeterminate = true;
                data.Textblock = "Checking IP addresses...\n";
                String[] ipInfo = new string[2];
                ipInfo = buttonFunctions.IpCheck();
                foreach (string s in ipInfo)
                {
                    data.Textblock = s+"\n";
                }
                //ProgBar.IsIndeterminate = false;

            }

            //Generate encrypted hash function
            if (listBox1.SelectedIndex.Equals(1))
            {
                if (!inputBox.Text.Equals(""))
                {

                    //ProgBar.IsIndeterminate = true;
                    data.Textblock = "Initializing Code, serializing input to 256bit encrypted hash.";
                    string pass = password.Password;
                    code = inputBox.Text;
                    data.Textblock = buttonFunctions.GenHash(code, pass);
                    //ProgBar.IsIndeterminate = false;

                }
                else
                    data.Textblock = "The text box is empty. Please enter the information you would like to be encrypted.\n";
            }

            //Server connection function (Will connect to current Spring webserver for pulling feedback data)
            if (listBox1.SelectedIndex.Equals(3))
            {

                //ProgBar.IsIndeterminate = true;
                List<int> openPorts = new List<int>();
                openPorts = buttonFunctions.ServerConnect();
                foreach (int port in openPorts)
                {
                    data.Textblock = "Connection has been made on port " + port;
                }
                //ProgBar.IsIndeterminate = false;
            }

            //Progress Bar button(Place holder for now, future features will go here)
            if (listBox1.SelectedIndex.Equals(4))
            {

                data.Textblock = "Executing Progress Bar Check.\n";
                data.Textblock = "Starting long Task...\n";

                Thread.Sleep(1000);

                data.Textblock = "In Progress...\n";

                ProgBar.Value = 0;

                Task.Run(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(50);
                        this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                        {
                            ProgBar.Value = i;
                        });
                    }
                });
            }
                      
        }
            //Button to clear out text box
            private void ClearButton_Click(object sender, RoutedEventArgs e)
            {
                data.reset();
                inputBox.Text = String.Empty;
                password.Clear();
                ProgBar.Value = 0;
                
            }

            //Methods for any event on click items in list.
            private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
            {

            }
        

        public class DataBinder : INotifyPropertyChanged
        {
            private string _textBlock = "";
            private string _inputBox = "";

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string property)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
            public string Textblock
            {
                get { return _textBlock; }
                set { _textBlock += value;
                    OnPropertyChanged("Textblock");
                }
            }
            public void reset()
            {
                _textBlock = "";
                OnPropertyChanged("Textblock");
            }
            public string Inputbox
            {
                get { return _inputBox; }
                set
                {
                    _inputBox = value;
                    OnPropertyChanged("Inputbox");
                }
            }
        }
    }
}
