using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using airbnbServer.BL;
using System.Net;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {
        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //Insert Flat
    public int InsertFlat(Flat flat)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        cmd = CreateInsertFlatWithStoredProcedure("sp_InsertFlat", con, flat);     // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command- 0/1
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateInsertFlatWithStoredProcedure(String spName, SqlConnection con, Flat flat)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@City",flat.City);
        cmd.Parameters.AddWithValue("@Address", flat.Address);
        cmd.Parameters.AddWithValue("@Price", flat.Price);
        cmd.Parameters.AddWithValue("@NumberOfRooms", flat.NumberOfRooms);


        return cmd;
    }
    

    //readFlats

    public List<Flat> ReadFlats()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Flat> flatsList = new List<Flat>();

        cmd = buildReadStoredProcedureCommand(con, "spGetFlatsList");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Flat f = new Flat();

            f.Id= Convert.ToInt32(dataReader["Flat_id"]); 
            f.City = dataReader["City"].ToString();
            f.Address = dataReader["Address"].ToString();
            f.Price = Convert.ToDouble(dataReader["Price"]);
            f.NumberOfRooms = Convert.ToInt32(dataReader["NumberOfRooms"]);
            flatsList.Add(f);
        }

        if (con != null)
        {
            con.Close();
        }

        return flatsList;

    }


    public User LogIn(string email,string password)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = buildLogInStoredProcedureCommand(con, "LoginUser",email,password );

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        if (dataReader == null)
        {
            return null;
        }
        User u = new User();
        while (dataReader.Read())
        {
            u.Id = Convert.ToInt32(dataReader["ID"]);
            u.Email = dataReader["email"].ToString();
            u.FirstName = dataReader["firstName"].ToString();
            u.FamilyName = dataReader["lastName"].ToString();
            u.Password = dataReader["password"].ToString();
            u.IsActive = Convert.ToBoolean(dataReader["isActive"]);
            u.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);
        }

        if (con != null)
        {
            con.Close();
        }

        return u;

    }

    SqlCommand buildLogInStoredProcedureCommand(SqlConnection con, string spName, string email, string password)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@password", password);
        return cmd;

    }
    SqlCommand buildReadStoredProcedureCommand(SqlConnection con, string spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;

    }



    //Insert User
    public int InsertUser(User user)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw(ex);
        }
        cmd = CreateInsertUserWithStoredProcedure("sp_InsertUser", con, user);     // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command- 0/1
            return numEffected;
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Email already exists."))
            {
                // Handle the specific error, e.g., return a custom error code
                // or throw a custom exception with a user-friendly message
                return -1; // Custom code indicating email already exists
                           // throw new Exception("Email already exists.");
            }
            else
            {
                // Handle other database exceptions (e.g., log the error)
                throw; // Re-throw the exception for broader error handling
            }
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private SqlCommand CreateInsertUserWithStoredProcedure(String spName, SqlConnection con, User user)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@firstName", user.FirstName);
        cmd.Parameters.AddWithValue("@lastName", user.FamilyName);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@password", user.Password);
     
        return cmd;
    }

    //readUsers

    public List<User> Readusers()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<User> usersList = new List<User>();

        cmd = buildReadStoredProcedureCommand(con, "spGetUserList");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            User u = new User();

            u.Id = Convert.ToInt32(dataReader["ID"]);
            u.Email = dataReader["email"].ToString();
            u.FirstName = dataReader["firstName"].ToString();
            u.FamilyName = dataReader["lastName"].ToString();
            u.Password = dataReader["password"].ToString();
            u.IsActive= Convert.ToBoolean(dataReader["isActive"]);
            u.IsAdmin= Convert.ToBoolean(dataReader["isAdmin"]);

            usersList.Add(u);
        }

        if (con != null)
        {
            con.Close();
        }

        return usersList;

    }


    //ReadVacations

   
    public List<Vacation> ReadVacs()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Vacation> VacsList = new List<Vacation>();

        cmd = buildReadVacsStoredProcedureCommand(con, "GetVacations");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Vacation v = new Vacation();

           v.Id = Convert.ToInt32(dataReader["vac_id"]);
            v.FlatId = Convert.ToInt32(dataReader["flat_id"]);
            v.UserId = Convert.ToInt32(dataReader["user_id"]);
            v.UserEmail= dataReader["user_email"].ToString();
            v.StartDate = Convert.ToDateTime(dataReader["start_date"]);
            v.EndDate = Convert.ToDateTime(dataReader["end_date"]);
            VacsList.Add(v);
        }

        if (con != null)
        {
            con.Close();
        }

        return VacsList;

    }

    SqlCommand buildReadVacsStoredProcedureCommand(SqlConnection con, string spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;

    }

    public int InsertVac(Vacation vac)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        cmd = CreateInsertVacWithStoredProcedure("InsertVacation", con, vac);     // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command- 0/1
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private SqlCommand CreateInsertVacWithStoredProcedure(String spName, SqlConnection con, Vacation vac)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@flat_id", vac.FlatId);
        cmd.Parameters.AddWithValue("@user_id", vac.UserId);
        cmd.Parameters.AddWithValue("@user_email", vac.UserEmail);
        cmd.Parameters.AddWithValue("@start_date", vac.StartDate);
        cmd.Parameters.AddWithValue("@end_date", vac.EndDate);


        return cmd;
    }

    public int Update(User user)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateUserWithStoredProcedure("UpdateUser", con, user);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateUpdateUserWithStoredProcedure(String spName, SqlConnection con, User user)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@firstName", user.FirstName);
        cmd.Parameters.AddWithValue("@lastName", user.FamilyName);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@password", user.Password);
        cmd.Parameters.AddWithValue("@isActive", user.IsActive);

        return cmd;
    }


    public List<Object> GetAverage(int month)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Object> avgList = new List<Object>();

        cmd = buildReadVacsStoredProcedureCommand(con, "spGetAveragePricePerNightByCityAndMonth", month);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            avgList.Add(new
            {
                City = dataReader["City"].ToString(),
                averagePerCity = Convert.ToDouble(dataReader["AveragePricePerNight"]),
            });

        }

        if (con != null)
        {
            con.Close();
        }

        return avgList;

    }

    SqlCommand buildReadVacsStoredProcedureCommand(SqlConnection con, string spName, int month)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@month", month);
        return cmd;

    }

}
