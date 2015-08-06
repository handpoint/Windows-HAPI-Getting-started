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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowsGettingStarted
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyClass my;

        public MainWindow()
        {
            InitializeComponent();
            my = new MyClass(this);
        }

        // Functions for buttons
        private void ConnectToSimulator_Click(object sender, RoutedEventArgs e)
        {
            my.Connect();
        }

        private void PayWithSignatureAuthorized_Click(object sender, RoutedEventArgs e)
        {
            my.PayWithSignatureAuthorized();
        }

        private void PayWithSignatureDeclined_Click(object sender, RoutedEventArgs e)
        {
            my.PayWithSignatureDeclined();
        }

        private void PayWithPinAuthorized_Click(object sender, RoutedEventArgs e)
        {
            my.PayWithPinAuthorized();
        }

        private void PayWithPinDeclined_Click(object sender, RoutedEventArgs e)
        {
            my.PayWithPinDeclined();
        }

        // Functions to display receipts
        public delegate void UpdateReceiptsCallback(string MerchantReceipt, string CustomerReceipt);
        public void DisplayReceipts(string MerchantReceipt, string CustomerReceipt)
        {
            //Only need to check for one of the webbrowsers
            if (!MerchantReceiptBrowser.CheckAccess())
            {
                UpdateReceiptsCallback d = new UpdateReceiptsCallback(DisplayReceipts);
                this.Dispatcher.Invoke(d, new object[] { MerchantReceipt, CustomerReceipt });
            }
            else
            {
                MerchantReceiptBrowser.NavigateToString(MerchantReceipt);
                CardholderReceiptBrowser.NavigateToString(CustomerReceipt);
            }
        }

        //function that updates status label
        public void DisplayStatus(string Status)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                CurrentStatusLabel.Text = Status;
            }));
            
        }
    }
}
