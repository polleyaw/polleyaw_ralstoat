/*
 * Hello from Bill
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;

namespace ralstoat_polleyaw_assignment06
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private static System.Data.SqlClient.SqlConnection connection;
        private static SqlCommand command;
        private static SqlDataReader reader;

        private ArrayList storeIDs = new ArrayList();
        private int selectedStoreID;

        private ArrayList loyalNumberIDs = new ArrayList();
        private int selectedLoyaltyNumberID;

        private ArrayList employeeIDs = new ArrayList();
        private int selectedEmployeeID;

        private ArrayList transactionTypeIDs = new ArrayList();
        private int selectedTransactionTypeID;

        protected void Page_Load(object sender, EventArgs e)
        {
            openConnection();
            readData(ddStores, "SELECT StoreID, Store FROM tStore;", storeIDs);

            readData(ddLoyaltyNumber, "SELECT LoyaltyID, LoyaltyNumber FROM tLoyalty;", loyalNumberIDs);

            readData(ddEmployee, "SELECT EmplID, FirstName, LastName FROM tEmpl;", employeeIDs);

            readData(ddTransactionType, "SELECT TransactionTypeID, TransactionType FROM tTransactionType;", transactionTypeIDs);
            //Response.Write(Convert.ToDateTime(DateTime.Now.ToShortDateString()));
            //Response.Write(DateTime.Now.ToShortTimeString());
        }

        private void readData(DropDownList control, string query, ArrayList primaryKeys)
        {
            // Stores the results of the query.
            String results;
            String results2;
            int ids;

            // Establishes the command for the given query on the connection.
            command = new SqlCommand(query, connection);

            // Attempts to read from the database.
            try
            {
                // Reads from the database.
                reader = command.ExecuteReader();

                // Determines if anything was returned.
                if (reader.HasRows)
                {
                    // Adds spaces between the items returned.
                    control.Items.Add(" ");
                    primaryKeys.Add(-1);

                    // Loops through all items that match the query in the database.
                    while (reader.Read())
                    {
                        if (reader.FieldCount == 2)
                        {
                            // Stores the returns.
                            ids = reader.GetInt32(0);
                            results = reader.GetString(1);

                            // Adds returned ids to ArrayList.
                            primaryKeys.Add(ids);
                            // Adds what is returned to the drop down list.
                            control.Items.Add(results);
                        }
                        else
                        {
                            // Stores the returns.
                            ids = reader.GetInt32(0);
                            results = reader.GetString(1);
                            results2 = reader.GetString(2);

                            // Adds returned ids to ArrayList.
                            primaryKeys.Add(ids);
                            // Adds what is returned to the drop down list.
                            control.Items.Add(results + " " + results2);
                        }
                    }
                }

                // Attempts to close the reader.
                try { reader.Close(); }
                // Displays an error if there is an issue closing the reader.
                catch (Exception ex)
                {
                    Response.Write(" There was an error closing the connection.");
                }
            }

            catch (Exception ex)
            {
                Response.Write(" There was an error reading from the database.");
            }
        }

        // Defines the method to obtain the connection string from the web.config file.
        private System.Configuration.ConnectionStringSettings GetConnectionString(string nameOfString)
        {
            String path;
            // Establishes the path to the file.
            path = HttpContext.Current.Request.ApplicationPath + "/web.config";
            // Obtains the connection string.
            System.Configuration.Configuration webConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(path);
            // Returns the connection string.
            return webConfig.ConnectionStrings.ConnectionStrings[nameOfString];
        }

        // Defines the method to open a connectio to the database.
        private void openConnection()
        {

            try
            {
                // Creates a connection to the database that can be opened or closed by utilizing the connection string.
                connection = new System.Data.SqlClient.SqlConnection(GetConnectionString("GroceryStoreSimulator").ConnectionString);
                // Opens the connection to execute queries on the database.
                connection.Open();
            }
            catch (Exception ex)
            {
                // Prints an error message if the connection cannot be opened.
                Response.Write("There was an issue opening the connection to the database. Please try again later.");
            }
        }

        protected void ddStores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddStores.SelectedIndex != 0)
            {
                selectedStoreID = Convert.ToInt32(storeIDs[ddStores.SelectedIndex]);
            }
        }

        protected void ddLoyaltyNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddLoyaltyNumber.SelectedIndex != 0)
            {
                selectedLoyaltyNumberID = Convert.ToInt32(loyalNumberIDs[ddLoyaltyNumber.SelectedIndex]);
            }
        }

        protected void ddEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddEmployee.SelectedIndex != 0)
            {
                selectedEmployeeID = Convert.ToInt32(employeeIDs[ddEmployee.SelectedIndex]);
            }
        }

        protected void ddTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddTransactionType.SelectedIndex != 0)
            {
                selectedTransactionTypeID = Convert.ToInt32(transactionTypeIDs[ddTransactionType.SelectedIndex]);
            }
        }
    }
}