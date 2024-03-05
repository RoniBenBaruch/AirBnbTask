namespace airbnbServer.BL
{
    public class Flat
    {
        int id;
        string city;
        string address;
        double price;//In dollars
        int numberOfRooms;

        public Flat(string city, string address, int price, int numberOfRooms)
        {
            City = city;
            Address = address;
            Price = price;
            NumberOfRooms = numberOfRooms;
        }

        public Flat() { }

        public int Id { get => id; set => id = value; }
        public string City { get => city; set => city = value; }
        public string Address { get => address; set => address = value; }
        public int NumberOfRooms { get => numberOfRooms; set => numberOfRooms = value; }
        public double Price
        {
            get { return price; }
            set { price = Discount(value); }
        }

        public int Insert()
        {
                DBservices dbs = new DBservices();
                return dbs.InsertFlat(this);
          
        }
        public List<Flat> Read()
        {
            DBservices dbs=new DBservices();
            return dbs.ReadFlats(); 
        }
        public double Discount(double value)
        {
            double realPrice;
            if (NumberOfRooms > 1 && value > 100)
            {
                realPrice = value * 0.9;
            }
            else
            {
                realPrice = value;
            }
            return realPrice;
        }


        //להעביר לSP
        public List<Flat> FlatsByCityPrice(string city, double price)
        {
            DBservices dbs = new DBservices();
            List <Flat> flats = dbs.ReadFlats();  
            List<Flat> sameCityAndPriceL = new List<Flat>();
            foreach (Flat flat in flats)
            {
                if (flat.Price < price && flat.City == city.ToLower())
                {
                    sameCityAndPriceL.Add(flat);
                }
            }
            return sameCityAndPriceL;
        }
    }
}
