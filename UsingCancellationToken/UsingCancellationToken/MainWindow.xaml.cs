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

namespace UsingCancellationToken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource tokenSource;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //  Create a cancellationTokenSource which will be passed to the task
            tokenSource = new CancellationTokenSource();

            ThreadPool.QueueUserWorkItem(SomeWork_Method, tokenSource.Token);
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
        }

        private void SomeWork_Method(object state)
        {
            // The token is passed in as state.
            //  Cast it as the CancellationToken
            var token = (CancellationToken)state;
            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(100);
                Dispatcher.Invoke(new Action(() => progressBar1.Value = i));
                Dispatcher.Invoke(new Action(() => txtProgressMessage.Text = i + "%"));

                //  YOU must have code to look at token otherwise having it
                //  accomplishes nothing.
                if (token.IsCancellationRequested)
                {
                    Dispatcher.Invoke(new Action(() => progressBar1.Value = 0));
                    Dispatcher.Invoke(new Action(() => txtProgressMessage.Text = "Operation Cancelled"));

                }
            }
        }
    }
}
