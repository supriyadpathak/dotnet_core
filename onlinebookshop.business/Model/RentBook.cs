namespace onlinebookshop.business
{
    public class RentBook
    {
        public int BookID { get; set; }        

        public string CustomerName { get; set; }

        public string CustomerContactNumber { get; set; }

        public string CustomerAdress { get; set; }

        public int RentPeriodInweeks { get; set; }
    }
}