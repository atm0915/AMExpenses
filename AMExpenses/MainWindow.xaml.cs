using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

namespace AMExpenses
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Money money = new Money();
        bool firstTimeSetup;

        public MainWindow()
        {
            InitializeComponent();
            startupSetter();
            if (firstTimeSetup == true)
            {
                mainStackPanel.Visibility = Visibility.Hidden;
                mainTextPanel.Visibility = Visibility.Hidden;
                toolbar.Visibility = Visibility.Hidden;
                firstTimeSetupStackPanel.Visibility = Visibility.Visible;

            }
            displayMoney.Text = String.Format("Current Credit: {0:C}", money.currentMoney.ToString());
            displayCredit.Text = String.Format("Current Credit on Account: {0:C}", money.currentCredit.ToString());

        }

        private void exitFirstTimeSetupButton_Click(object sender, RoutedEventArgs e)
        {
            money.currentMoney = decimal.Parse(firstTimeSetupMoneyInput.Text);
            money.currentCredit = decimal.Parse(firstTimeSetupCreditInput.Text);
            firstTimeSetup = false;
            MessageBox.Show("Please restart the application");
            Application.Current.Shutdown();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            shutdownSetter(money, firstTimeSetup);

        }

        private void shutdownSetter(Money money, bool firstTimeSetup)
        {
            using (StreamWriter writetext = new StreamWriter("startup"))
            {
                writetext.WriteLine(firstTimeSetup.ToString());
                writetext.WriteLine(money.currentMoney.ToString());
                writetext.WriteLine(money.currentCredit.ToString());


            }
        }

        private void New_Payment_Click(object sender, RoutedEventArgs e)
        {
            mainStackPanel.Visibility = Visibility.Hidden;
            mainTextPanel.Visibility = Visibility.Hidden;
            paymentButtonPanel.Visibility = Visibility.Visible;
            paymentTextPanel.Visibility = Visibility.Visible;

        }

        private void Save_Payment_Info_Click(object sender, RoutedEventArgs e)
        {
            string paymentDescription = paymentDescriptionInput.Text;
            decimal paymentMoneyAmount = decimal.Parse(paymentAmountInput.Text);
            money = Payment.newPayment(money, paymentDescription, paymentMoneyAmount);
            MessageBox.Show("Debit Saved!");
            mainStackPanel.Visibility = Visibility.Visible;
            mainTextPanel.Visibility = Visibility.Visible;
            paymentButtonPanel.Visibility = Visibility.Hidden;
            paymentTextPanel.Visibility = Visibility.Hidden;
            displayMoney.Text = String.Format("Current Credit: {0:C}", money.currentMoney.ToString());
            displayCredit.Text = String.Format("Current Credit on Account: {0:C}", money.currentCredit.ToString());
        }

        private void New_Credit_Click(object sender, RoutedEventArgs e)
        {
            mainStackPanel.Visibility = Visibility.Hidden;
            mainTextPanel.Visibility = Visibility.Hidden;
            creditButtonPanel.Visibility = Visibility.Visible;
            creditTextPanel.Visibility = Visibility.Visible;
        }

        private void Save_Credit_Info_Click(object sender, RoutedEventArgs e)
        {
            string creditDescription = creditDescriptionInput.Text;
            decimal creditMoneyAmount = decimal.Parse(creditAmountInput.Text);
            money = Credit.newCredit(money, creditDescription, creditMoneyAmount);
            MessageBox.Show("Credit on Account Saved!");
            mainStackPanel.Visibility = Visibility.Visible;
            mainTextPanel.Visibility = Visibility.Visible;
            creditButtonPanel.Visibility = Visibility.Hidden;
            creditTextPanel.Visibility = Visibility.Hidden;
            displayMoney.Text = String.Format("Current Credit: {0:C}", money.currentMoney.ToString());
            displayCredit.Text = String.Format("Current Credit on Account: {0:C}", money.currentCredit.ToString());
        }

        private void New_Income_Click(object sender, RoutedEventArgs e)
        {
            mainStackPanel.Visibility = Visibility.Hidden;
            mainTextPanel.Visibility = Visibility.Hidden;
            incomeButtonPanel.Visibility = Visibility.Visible;
            incomeTextPanel.Visibility = Visibility.Visible;
        }

        private void Save_Income_Info_Click(object sender, RoutedEventArgs e)
        {
            string incomeDescription = incomeDescriptionInput.Text;
            decimal incomeMoneyAmount = decimal.Parse(incomeAmountInput.Text);
            money = Income.newIncome(money, incomeDescription, incomeMoneyAmount);
            MessageBox.Show("Credit Saved!");
            mainStackPanel.Visibility = Visibility.Visible;
            mainTextPanel.Visibility = Visibility.Visible;
            incomeButtonPanel.Visibility = Visibility.Hidden;
            incomeTextPanel.Visibility = Visibility.Hidden;
            displayMoney.Text = String.Format("Current Credit: {0:C}", money.currentMoney.ToString());
            displayCredit.Text = String.Format("Current Credit on Account: {0:C}", money.currentCredit.ToString());

        }


        private void PaymentsTxt_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("debits.txt");
        }
        private void CreditsTxt_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("creditonaccount.txt");
        }
        private void IncomesTxt_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("credits.txt");
        }
        public void startupSetter()
        {
            using (StreamReader sr = new StreamReader("startup"))
            {
                firstTimeSetup = Convert.ToBoolean(sr.ReadLine());
                money.currentMoney = decimal.Parse(sr.ReadLine());
                money.currentCredit = decimal.Parse(sr.ReadLine());

            }
        }
    }
}
