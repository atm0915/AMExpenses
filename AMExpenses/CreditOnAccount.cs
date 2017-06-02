using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMExpenses
{
    class CreditOnAccount
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal moneyAmount { get; set; }

        public static Money newCreditOnAccount(Money money, string description, decimal moneyAmountInput)
        {
            CreditOnAccount creditOnAccount = new CreditOnAccount();
            creditOnAccount.Date = DateTime.Now;
            creditOnAccount.Description = description;
            creditOnAccount.moneyAmount = moneyAmountInput;
            money.CurrentCreditOnAccount += creditOnAccount.moneyAmount;
            using (StreamWriter writetext = new StreamWriter("creditonaccount.txt", true))
            {
                writetext.WriteLine(String.Format("{0:MM/dd/yyyy} | {1} | {2:C}",
                    creditOnAccount.Date,
                    creditOnAccount.Description,
                    creditOnAccount.moneyAmount));

            }
            SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=AMExpenses;User ID=sa;Password=***REMOVED***");
            conn.Open();
            string command = String.Format("INSERT INTO atm0915Log (Type, Date, Description, Amount) VALUES ('Credit On Account', '{0:MM/dd/yyyy}', '{1}', '{2:C}')", creditOnAccount.Date, creditOnAccount.Description, creditOnAccount.moneyAmount);
            SqlCommand log = new SqlCommand(command, conn);


            log.ExecuteNonQuery();
            conn.Close();
            return money;
        }

    }
}
