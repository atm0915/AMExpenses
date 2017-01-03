using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AMExpenses
{
    class Payment
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal moneyAmount { get; set; }

        public static Money newPayment(Money money, string description, decimal moneyAmountInput, CheckBox ignoreCreditOnAccountCheckBox)
        {
            Payment payment = new Payment();
            payment.Date = DateTime.Now;
            payment.Description = description;
            payment.moneyAmount = moneyAmountInput;
            if (ignoreCreditOnAccountCheckBox.IsChecked == true)
            {
                money.CurrentMoney -= payment.moneyAmount;
            }
            else
            {
                if (money.CurrentCredit > 0 && money.CurrentCredit > payment.moneyAmount)
                {
                    money.CurrentCredit -= payment.moneyAmount;
                }
                else if (money.CurrentCredit > 0 && money.CurrentCredit <= payment.moneyAmount)
                {
                    money.CurrentCredit -= payment.moneyAmount;
                    decimal newAmount = money.CurrentCredit * -1;
                    money.CurrentMoney -= newAmount;
                    money.CurrentCredit = 0;
                }
                else if (money.CurrentCredit == 0)
                {
                    money.CurrentMoney -= payment.moneyAmount;
                }
            }

            




            using (StreamWriter writetext = new StreamWriter("debits.txt", true))
            {
                writetext.WriteLine(String.Format("{0:MM/dd/yyyy} | {1} | {2:C}", 
                    payment.Date, 
                    payment.Description, 
                    payment.moneyAmount));

            }




            return money;

        }


    }
}
