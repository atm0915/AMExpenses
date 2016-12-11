using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMExpenses
{
    class Income
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal moneyAmount { get; set; }

        public static Money newIncome(Money money, string description, decimal moneyAmountInput)
        {
            Income income = new Income();
            income.Date = DateTime.Now;
            income.Description = description;
            income.moneyAmount = moneyAmountInput;
            money.currentMoney += income.moneyAmount;

            using (StreamWriter writetext = new StreamWriter("incomes.txt", true))
            {
                writetext.WriteLine(String.Format("{0:MM/dd/yyyy} | {1} | {2:C}",
                    income.Date,
                    income.Description,
                    income.moneyAmount));

            }
            return money;

        }

    }
}
