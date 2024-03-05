namespace airbnbServer.BL
{
    public class Vacation
    {
        int id;
        int userId;
        int flatId;
        string userEmail;
        DateTime startDate;
        DateTime endDate;
        static List<Vacation> vacationsList = new List<Vacation>();

        public Vacation() { }
        public Vacation(int id, int userId, int flatId, DateTime startDate, DateTime endDate)
        {
            Id = id;
            UserId = userId;
            FlatId = flatId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int Id { get => id; set => id = value; }
        public int UserId { get => userId; set => userId = value; }
        public int FlatId { get => flatId; set => flatId = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public string UserEmail { get => userEmail; set => userEmail = value; }

        public bool insert()
        {
            DBservices dbs = new DBservices();
            List<Vacation> vacations = dbs.ReadVacs();
            foreach (Vacation vication in vacations)
            {
                if (vication.Id == this.Id)
                {
                    return false;
                }
                if (this.FlatId == vication.FlatId)
                {
                    if (this.StartDate <= vication.EndDate && this.EndDate >= vication.StartDate)
                    {
                        return false;
                    }
                }
            }
            dbs.InsertVac(this);
            return true;
        }
        public List<Vacation> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadVacs();
        }
        public List<Vacation> getByDates(DateTime startDate, DateTime endDate)
        {
            List<Vacation> filterVicationL = new List<Vacation>();
            foreach (Vacation vication in vacationsList)
            {
                if (vication.StartDate >= startDate && vication.EndDate <= endDate)
                {
                    filterVicationL.Add(vication);
                }
            }
            return filterVicationL;
        }
    }
}
