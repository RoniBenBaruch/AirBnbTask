namespace airbnbServer.BL
{
    public class User
    {
        int id;
        bool isAdmin;
        bool isActive;
        string firstName;
        string familyName;
        string email;
        string password;

        public User(string firstName, string familyName, string email, string password)
        {
            FirstName = firstName;
            FamilyName = familyName;
            Email = email;
            Password = password;
        }

        public User() { }
        public string FirstName { get => firstName; set => firstName = value; }
        public string FamilyName { get => familyName; set => familyName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public int Id { get => id; set => id = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public bool IsActive { get => isActive; set => isActive = value; }

        public int Insert()
        {
            DBservices dbs = new DBservices();
            return dbs.InsertUser(this);

        }
        public int Update()
        {
            DBservices dbs = new DBservices();
            return dbs.Update(this);

        }
        public List<User> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.Readusers();
        }

        public User CheckLogIn(string email,string password)
        {
            DBservices dbs = new DBservices();
            User u= dbs.LogIn(email,password);
            return u;
        }

    }
}
