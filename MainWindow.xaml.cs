using System;
using System.IO;
using System.Timers;
using System.Windows;
using Microsoft.Win32;
using MahApps.Metro.Controls;


namespace SpeechToText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTranscode_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "" || txtAudioPath.Text == "" || txtOutputPath.Text == "")
            {
                MessageBox.Show("Form incomplete", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                btnTranscode.IsEnabled = false;
                STT_Service s2t = new STT_Service(txtUsername.Text, txtPassword.Text, txtAudioPath.Text, txtOutputPath.Text);
                MessageBox.Show("Transcode complete", "Transcode Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                btnTranscode.IsEnabled = true;
            }
        }

        private void btnBrowseAudio_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 Files (*.mp3)|*.mp3";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                txtAudioPath.Text = ofd.FileName;
            }
        }

        private void btnBrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                txtOutputPath.Text = ofd.FileName;
            }
        }
    }
}
