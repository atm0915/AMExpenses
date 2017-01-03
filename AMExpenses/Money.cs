using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMExpenses
{
    public class Money
    {
        private decimal _currentmoney;
        private decimal _currentcredit;


        public decimal CurrentMoney
        {
            get
            {
                return _currentmoney;
            }
            set
            {
                _currentmoney = value;
                AmountChanged(this);
            }
        }
        public decimal CurrentCredit
        {
            get
            {
                return _currentcredit;
            }
            set
            {
                _currentcredit = value;
                AmountChanged(this);
            }
        }

        public event AmountChangedDelegate AmountChanged;



    }
}
