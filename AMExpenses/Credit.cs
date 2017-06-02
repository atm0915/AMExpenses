using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMExpenses
{
    class Credit
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal moneyAmount { get; set; }

        public static Money newCredit(Money money, string description, decimal moneyAmountInput)
        {
            Credit credit = new Credit();
            credit.Date = DateTime.Now;
            credit.Description = description;
            credit.moneyAmount = moneyAmountInput;
            money.CurrentCredit += credit.moneyAmount;

            using (StreamWriter writetext = new StreamWriter("credits.txt", true))
            {
                writetext.WriteLine(String.Format("{0:MM/dd/yyyy} | {1} | {2:C}",
                    credit.Date,
                    credit.Description,
                    credit.moneyAmount));

            }
            SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=AMExpenses;User ID=sa;Password=***REMOVED***");
            conn.Open();
            string command = String.Format("INSERT INTO atm0915Log (Type, Date, Description, Amount) VALUES ('Credit', '{0:MM/dd/yyyy}', '{1}', '{2:C}')", credit.Date, credit.Description, credit.moneyAmount);
            SqlCommand log = new SqlCommand(command, conn);


            log.ExecuteNonQuery();
            conn.Close();
            return money;

        }

    }
}
