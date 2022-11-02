namespace ClickandCollect.Models
{
    public class OrderPicker : Person
    {
        public OrderPicker(int id,string ln, string fn, string un, string pw,Market m) : base(id,ln, fn, un, pw) {
            this.market = m;
        }

        private Market market;

        public Market Market
        {
            get { return market; }
            set { market = value; }
        }

    }
}
