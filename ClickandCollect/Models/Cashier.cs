namespace ClickandCollect.Models
{
    public class Cashier : Person
    {
        public Cashier() {}
        public Cashier(int id,string ln, string fn, string un, string pw,Market m) : base(id,ln, fn, un, pw) {
            this.Market = m;
        }

        private Market market;

        public Market Market
        {
            get { return market; }
            set { market = value; }
        }
    }
}
