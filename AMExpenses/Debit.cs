using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AMExpenses
{
    class Debit
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal moneyAmount { get; set; }

        public static Money newDebit(Money money, string description, decimal moneyAmountInput, CheckBox ignoreCreditOnAccountCheckBox)
        {
            Debit debit = new Debit();
            debit.Date = DateTime.Now;
            debit.Description = description;
            debit.moneyAmount = moneyAmountInput;
            if (ignoreCreditOnAccountCheckBox.IsChecked == true)
            {
                money.CurrentCredit -= debit.moneyAmount;
            }
            else
            {
                if (money.CurrentCreditOnAccount > 0 && money.CurrentCreditOnAccount > debit.moneyAmount)
                {
                    money.CurrentCreditOnAccount -= debit.moneyAmount;
                }
                else if (money.CurrentCreditOnAccount > 0 && money.CurrentCreditOnAccount <= debit.moneyAmount)
                {
                    money.CurrentCreditOnAccount -= debit.moneyAmount;
                    decimal newAmount = money.CurrentCreditOnAccount * -1;
                    money.CurrentCredit -= newAmount;
                    money.CurrentCreditOnAccount = 0;
                }
                else if (money.CurrentCreditOnAccount == 0)
                {
                    money.CurrentCredit -= debit.moneyAmount;
                }
            }

            




            using (StreamWriter writetext = new StreamWriter("debits.txt", true))
            {
                writetext.WriteLine(String.Format("{0:MM/dd/yyyy} | {1} | {2:C}", 
                    debit.Date, 
                    debit.Description, 
                    debit.moneyAmount));

            }
            SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=AMExpenses;User ID=sa;Password=***REMOVED***");
            conn.Open();
            string command = String.Format("INSERT INTO atm0915Log (Type, Date, Description, Amount) VALUES ('Debit', '{0:MM/dd/yyyy}', '{1}', '{2:C}')", debit.Date, debit.Description, debit.moneyAmount);
            SqlCommand log = new SqlCommand(command, conn);


            log.ExecuteNonQuery();
            conn.Close();



            return money;

        }


    }
}
