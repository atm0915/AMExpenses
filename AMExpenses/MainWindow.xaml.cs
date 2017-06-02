using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            money.AmountChanged += new AmountChangedDelegate(setTotalText);
            startupSetter();
            if (firstTimeSetup == true)
            {
                mainStackPanel.Visibility = Visibility.Hidden;
                mainTextPanel.Visibility = Visibility.Hidden;
                toolbar.Visibility = Visibility.Hidden;
                firstTimeSetupStackPanel.Visibility = Visibility.Visible;

            }
            displayCredit.Text = String.Format("Current Credit: {0:C}", money.CurrentCredit);
            displayCreditOnAccount.Text = String.Format("Current Credit on Account: {0:C}", money.CurrentCreditOnAccount);
            setTotalText(money);



        }

        private void exitFirstTimeSetupButton_Click(object sender, RoutedEventArgs e)
        {
            money.CurrentCredit = decimal.Parse(firstTimeSetupMoneyInput.Text);
            money.CurrentCreditOnAccount = decimal.Parse(firstTimeSetupCreditInput.Text);
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
                writetext.WriteLine(money.CurrentCredit.ToString());
                writetext.WriteLine(money.CurrentCreditOnAccount.ToString());


            }
            SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=AMExpenses;User ID=sa;Password=***REMOVED***");
            conn.Open();
            SqlCommand updateCredit = new SqlCommand("UPDATE atm0915MainData SET Credit = " + money.CurrentCredit, conn);
            SqlCommand updateCreditOnAccount = new SqlCommand("UPDATE atm0915MainData SET CreditOnAccount = " + money.CurrentCreditOnAccount, conn);
            updateCredit.ExecuteNonQuery();
            updateCreditOnAccount.ExecuteNonQuery();
            conn.Close();
        }

        private void New_Debit_Click(object sender, RoutedEventArgs e)
        {
            mainStackPanel.Visibility = Visibility.Hidden;
            mainTextPanel.Visibility = Visibility.Hidden;
            debitButtonPanel.Visibility = Visibility.Visible;
            debitTextPanel.Visibility = Visibility.Visible;

        }

        private void Save_Debit_Info_Click(object sender, RoutedEventArgs e)
        {
            string debitDescription = debitDescriptionInput.Text;
            decimal debitMoneyAmount = decimal.Parse(debitAmountInput.Text);
            money = Debit.newDebit(money, debitDescription, debitMoneyAmount, ignoreCreditOnAccountCheckBox);
            MessageBox.Show("Debit Saved!");
            mainStackPanel.Visibility = Visibility.Visible;
            mainTextPanel.Visibility = Visibility.Visible;
            debitButtonPanel.Visibility = Visibility.Hidden;
            debitTextPanel.Visibility = Visibility.Hidden;
            displayCredit.Text = String.Format("Current Credit: {0:C}", money.CurrentCredit);
            displayCreditOnAccount.Text = String.Format("Current Credit on Account: {0:C}", money.CurrentCreditOnAccount);
        }

        private void New_Credit_On_Account_Click(object sender, RoutedEventArgs e)
        {
            mainStackPanel.Visibility = Visibility.Hidden;
            mainTextPanel.Visibility = Visibility.Hidden;
            creditOnAccountButtonPanel.Visibility = Visibility.Visible;
            creditOnAccountTextPanel.Visibility = Visibility.Visible;
        }

        private void Save_Credit_On_Account_Info_Click(object sender, RoutedEventArgs e)
        {
            string creditOnAccountDescription = creditOnAccountDescriptionInput.Text;
            decimal creditOnAccountMoneyAmount = decimal.Parse(creditOnAccountAmountInput.Text);
            money = CreditOnAccount.newCreditOnAccount(money, creditOnAccountDescription, creditOnAccountMoneyAmount);
            MessageBox.Show("Credit on Account Saved!");
            mainStackPanel.Visibility = Visibility.Visible;
            mainTextPanel.Visibility = Visibility.Visible;
            creditOnAccountButtonPanel.Visibility = Visibility.Hidden;
            creditOnAccountTextPanel.Visibility = Visibility.Hidden;
            displayCredit.Text = String.Format("Current Credit: {0:C}", money.CurrentCredit);
            displayCreditOnAccount.Text = String.Format("Current Credit on Account: {0:C}", money.CurrentCreditOnAccount);
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
            MessageBox.Show("Credit Saved!");
            mainStackPanel.Visibility = Visibility.Visible;
            mainTextPanel.Visibility = Visibility.Visible;
            creditButtonPanel.Visibility = Visibility.Hidden;
            creditTextPanel.Visibility = Visibility.Hidden;
            displayCredit.Text = String.Format("Current Credit: {0:C}", money.CurrentCredit);
            displayCreditOnAccount.Text = String.Format("Current Credit on Account: {0:C}", money.CurrentCreditOnAccount);

        }


        private void DebitsTxt_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("debits.txt");

        }
        private void CreditOnAccountsTxt_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("creditonaccount.txt");
        }
        private void CreditsTxt_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("credits.txt");
        }
        public void startupSetter()
        {
            if (!File.Exists("startup"))
            {
                StreamWriter sw = new StreamWriter("startup");
                sw.Flush();
                sw.Close();
            }
            using (StreamReader sr = new StreamReader("startup"))
            {
                firstTimeSetup = Convert.ToBoolean(sr.ReadLine());
            }
            SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=AMExpenses;User ID=sa;Password=***REMOVED***");
            conn.Open();
            SqlDataReader rdr = null;
            SqlCommand findCredit = new SqlCommand("SELECT Credit FROM atm0915MainData", conn);
            SqlCommand findCreditOnAccount = new SqlCommand("SELECT CreditOnAccount FROM atm0915MainData", conn);

            rdr = findCredit.ExecuteReader();
            while (rdr.Read())
            {
                money.CurrentCredit = Decimal.Parse(rdr[0].ToString());
            }
            rdr.Close();
            rdr = findCreditOnAccount.ExecuteReader();
            while (rdr.Read())
            {
                money.CurrentCreditOnAccount = Decimal.Parse(rdr[0].ToString());
            }
            rdr.Close();
            conn.Close();
        }
        public void setTotalText(Money sender)
        {
            decimal total = sender.CurrentCreditOnAccount + sender.CurrentCredit;
            displayTotal.Text = String.Format($"Total: {total:C}");

        }
    }
}
