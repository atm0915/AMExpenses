using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMExpenses
{
    public class Money
    {
        private decimal _currentcredit;
        private decimal _currentcreditonaccount;


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
        public decimal CurrentCreditOnAccount
        {
            get
            {
                return _currentcreditonaccount;
            }
            set
            {
                _currentcreditonaccount = value;
                AmountChanged(this);
            }
        }

        public event AmountChangedDelegate AmountChanged;



    }
}
