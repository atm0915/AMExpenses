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
                money.currentMoney -= payment.moneyAmount;
            }
            else
            {
                if (money.currentCredit > 0 && money.currentCredit > payment.moneyAmount)
                {
                    money.currentCredit -= payment.moneyAmount;
                }
                else if (money.currentCredit > 0 && money.currentCredit <= payment.moneyAmount)
                {
                    money.currentCredit -= payment.moneyAmount;
                    decimal newAmount = money.currentCredit * -1;
                    money.currentMoney -= newAmount;
                    money.currentCredit = 0;
                }
                else if (money.currentCredit == 0)
                {
                    money.currentMoney -= payment.moneyAmount;
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
