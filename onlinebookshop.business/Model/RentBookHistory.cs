namespace onlinebookshop.business
{
    public class RentBookHistory
    {
        public int BookID { get; set; }

        public string BookName { get; set; }

        public string CustomerName { get; set; }

        public string CustomerContactNumber { get; set; }

        public string CustomerAdress { get; set; }

        public int RentPeriodInweeks { get; set; }

        public DateTime RentDate { get; set; }
    }
}