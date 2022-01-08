using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();

        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(100);
                
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
            switch ((int)pbStatus.Value)
            {
                case 10:
                    pbStatus.Foreground = System.Windows.Media.Brushes.Red;
                    break;
                case 20:
                    pbStatus.Foreground = System.Windows.Media.Brushes.Green;
                    break;
                case 30:
                    pbStatus.Foreground = System.Windows.Media.Brushes.Blue;
                    break;
                case 40:
                    pbStatus.Foreground = System.Windows.Media.Brushes.Yellow;
                    break;
                case 50:
                    pbStatus.Foreground = System.Windows.Media.Brushes.Black;
                    break;
                case 60:
                    pbStatus.Foreground = System.Windows.Media.Brushes.White;
                    break;
                case 70:
                    pbStatus.Foreground = System.Windows.Media.Brushes.Red;
                    break;
                case 80:
                    pbStatus.Foreground = System.Windows.Media.Brushes.Green;
                    break;
                case 90:
                    pbStatus.Foreground = System.Windows.Media.Brushes.Orange;
                    break;
                case 100:
                    pbStatus.Foreground = System.Windows.Media.Brushes.Yellow;
                    break;
               

            }
        }
    }
}
