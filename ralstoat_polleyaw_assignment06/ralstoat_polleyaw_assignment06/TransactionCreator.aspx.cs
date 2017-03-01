/***********************************************************************************************************************************************************************************************
 * Assignment 6
 * Adam Ralston (ralstoat@mail.uc.edu) and Andrew Polley (polleyaw@mail.uc.edu)
 * IT3047C Web Server App Dev
 * The purpose of this assignment was to create a web form that accepted inputs from a user which upon submission
 * will execute a stored procedure with the inputted values to create a record in the transaction table and a record
 * in the transaction detail table of the grocery store simulator database.
 * Due Date: 3/1/2017
 *
 * Citations: The stored procedure was provided to us by Professor Bill Nicholson to reference.
 **********************************************************************************************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using System.Text.RegularExpressions;

namespace ralstoat_polleyaw_assignment06
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private static System.Data.SqlClient.SqlConnection connection;
        private static SqlCommand command;
        private static SqlDataReader reader;

        // Stores all the primary keys for the stores table returned from the database.
        private ArrayList storeIDs = new ArrayList();
        // Records the primary key for the currently selected store.
        private int selectedStoreID;

        // Stores all the primary keys for the loyalty numbers table returned from the database.
        private ArrayList loyaltyNumberIDs = new ArrayList();
        // Records the primary key for the currently selected loyalty number.
        private int selectedLoyaltyNumberID;

        // Stores all the primary keys for the employees table returned from the database.
        private ArrayList employeeIDs = new ArrayList();
        // Records the primary key for the currently selected employee.
        private int selectedEmployeeID;

        // Stores all the primary keys for the transaction types table returned from the database.
        private ArrayList transactionTypeIDs = new ArrayList();
        // Records the primary key for the currently selected transaction type.
        private int selectedTransactionTypeID;

        // Stores all the primary keys for the products table returned from the database.
        private ArrayList productIDs = new ArrayList();
        // Records the primary key for the currently selected product.
        private int selectedProductID;

        // Stores all the initial prices per unit of product returned from the database.
        private ArrayList initialPrices = new ArrayList();
        // Records the initial price for the currently selected product.
        private string chosenProductPrice;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Opens connection to the database.
                openConnection();

                // Retrieves and stores the positions of the selected items from the drop down lists 
                // so they persist after clearing the drop down lists.
                int maintainStores = ddStores.SelectedIndex;
                int maintainLoyaltyNumber = ddLoyaltyNumber.SelectedIndex;
                int maintainTransactionType = ddTransactionType.SelectedIndex;
                int maintainEmployee = ddEmployee.SelectedIndex;
                int maintainProducts = ddProducts.SelectedIndex;

                // Clears the drop down lists. This prevents records from continously being appended to the 
                // drop down lists with each postback. Restricting postback causes errors with query executions
                // so this is next best fix.
                ddStores.Items.Clear();
                ddLoyaltyNumber.Items.Clear();
                ddTransactionType.Items.Clear();
                ddEmployee.Items.Clear();
                ddProducts.Items.Clear();

                // Reads data from stores table.
                readData(ddStores, "SELECT StoreID, Store, Address1, City, State, Zip FROM tStore;", storeIDs);

                // Reads data from loyalty numbers table.
                readData(ddLoyaltyNumber, "SELECT LoyaltyID, LoyaltyNumber FROM tLoyalty;", loyaltyNumberIDs);

                // Reads data from employees table.
                readData(ddEmployee, "SELECT EmplID, FirstName, LastName FROM tEmpl;", employeeIDs);

                // Reads data from transaction types table.
                readData(ddTransactionType, "SELECT TransactionTypeID, TransactionType FROM tTransactionType;", transactionTypeIDs);

                // Reads data from products table.
                readData(ddProducts, "SELECT tProduct.ProductID, tBrand.Brand, tName.Name, tProduct.Description, tProduct.InitialPricePerSellableUnit FROM tProduct INNER JOIN tBrand ON tProduct.BrandID = tBrand.BrandID INNER JOIN tName ON tProduct.NameID = tName.NameID;", productIDs);

                // Reassigns positions of selection on drop down lists after they are populated.
                ddStores.SelectedIndex = maintainStores;
                ddLoyaltyNumber.SelectedIndex = maintainLoyaltyNumber;
                ddTransactionType.SelectedIndex = maintainTransactionType;
                ddEmployee.SelectedIndex = maintainEmployee;
                ddProducts.SelectedIndex = maintainProducts;
            }
            catch(Exception ex)
            {
                tbTransactionDetails.Text = "There appears to have been an intermittent error. Please enter your transaction information and press submit again.";
            }

        }

        // Defines the method that reads data from the database in response to a drop down list, query, and
        // arraylist of primary keys passed to the method.
        private void readData(DropDownList control, string query, ArrayList primaryKeys)
        {
            // Stores the results of the query.
            String results;
            String results2;
            String results3;
            String results4;
            string results5;
            int ids;

            // Establishes the command for the given query on the connection.
            command = new SqlCommand(query, connection);

            // Attempts to read from the database.
            try
            {
                // Reads from the database.
                reader = command.ExecuteReader();

                // Adds spaces between the items returned.
                control.Items.Add("Please make a selection");
                primaryKeys.Add(-1);

                // Determines if anything was returned.
                while (reader.HasRows)
                {
                    // Loops through all items that match the query in the database.
                    while (reader.Read())
                    {
                        // Makes a decision on what should be the next step based on the number of fields
                        // returned from the query.
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
                        else if (reader.FieldCount == 3)
                        {
                            // Stores the returns.
                            ids = reader.GetInt32(0);
                            results = reader.GetString(1).Trim();
                            results2 = reader.GetString(2).Trim();

                            // Adds returned ids to ArrayList.
                            primaryKeys.Add(ids);
                            // Adds what is returned to the drop down list.
                            control.Items.Add(results + " " + results2);
                        }
                        else if (reader.FieldCount == 5)
                        {
                            // Stores the returns.
                            ids = reader.GetInt32(0);
                            results = reader.GetString(1).Trim();
                            results2 = reader.GetString(2).Trim();
                            results3 = reader.GetString(3).Trim();
                            results4 = reader.GetSqlMoney(4).ToString().Trim();

                            // Adds returned ids to ArrayList.
                            primaryKeys.Add(ids);
                            // Adds what is returned to the drop down list.
                            control.Items.Add(results + " " + results2 + " " + results3);

                            // Adds extra information requested to the appropriate class member.
                            initialPrices.Add(results4);
                        }
                        else if (reader.FieldCount == 6)
                        {
                            // Stores the returns.
                            ids = reader.GetInt32(0);
                            results = reader.GetString(1).Trim();
                            results2 = reader.GetString(2).Trim();
                            results3 = reader.GetString(3).Trim();
                            results4 = reader.GetString(4).Trim();
                            results5 = reader.GetString(5).Trim();

                            // Adds returned ids to ArrayList.
                            primaryKeys.Add(ids);
                            // Adds what is returned to the drop down list.
                            control.Items.Add(results + " " + results2 + " " + results3 + " " + results4 + " " + results5);
                        }
                    }
                    // Reads the results of the next SQL statement executed for a given query execution.
                    reader.NextResult();
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
                tbTransactionDetails.Text = "There was an error reading from the database.";
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
                tbTransactionDetails.Text = "There was an issue opening the connection to the database. Please try again later.";
            }
        }

        // Defines the event handler responsible for when a selection is made in the Stores drop down list.
        protected void ddStores_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Makes sure the first index is not selected before further processing as it has as it has no item of value at that index.
                if (ddStores.SelectedIndex != 0)
                {
                    // Retrieves and stores the primary key for the selected store item.
                    selectedStoreID = Convert.ToInt32(storeIDs[ddStores.SelectedIndex]);
                }
            }
            catch(Exception ex)
            {
                tbTransactionDetails.Text = "There appears to have been an intermittent error. Please enter your transaction information and press submit again.";
            }
        }

        // Defines the event handler responsible for when a selection is made in the Loyalty Numbers drop down list.
        protected void ddLoyaltyNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Makes sure the first index is not selected before further processing as it has no item of value at that index.
                if (ddLoyaltyNumber.SelectedIndex != 0)
                {
                    // Retrieves and stores the primary key for the selected loyalty number item.
                    selectedLoyaltyNumberID = Convert.ToInt32(loyaltyNumberIDs[ddLoyaltyNumber.SelectedIndex]);
                }
            }
            catch(Exception ex)
            {
                tbTransactionDetails.Text = "There appears to have been an intermittent error. Please enter your transaction information and press submit again.";
            }
        }

        // Defines the event handler responsible for when a selection is made in the Employees drop down list.
        protected void ddEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Makes sure the first index is not selected before further processing as it has no item of value at that index.
                if (ddEmployee.SelectedIndex != 0)
                {
                    // Retrieves and stores the primary key for the selected employee item.
                    selectedEmployeeID = Convert.ToInt32(employeeIDs[ddEmployee.SelectedIndex]);
                }
            }
            catch(Exception ex)
            {
                tbTransactionDetails.Text = "There appears to have been an intermittent error. Please enter your transaction information and press submit again.";
            }
        }

        // Defines the event handler responsible for when a selection is made in the transaction types drop down list.
        protected void ddTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Makes sure the first index is not selected before further processing as it has no item of value at that index.
                if (ddTransactionType.SelectedIndex != 0)
                {
                    // Retrieves and stores the primary key for the selected transaction type item.
                    selectedTransactionTypeID = Convert.ToInt32(transactionTypeIDs[ddTransactionType.SelectedIndex]);
                }
            }
            catch(Exception ex)
            {
                tbTransactionDetails.Text = "There appears to have been an intermittent error. Please enter your transaction information and press submit again.";
            }
        }

        // Defines the event handler responsible for when a selection is made in the products drop down list.
        protected void ddProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Makes sure the first index is not selected before further processing as it has no item of value at that index.
                if (ddProducts.SelectedIndex != 0)
                {
                    // Retrieves and stores the primary key for the selected product item.
                    selectedProductID = Convert.ToInt32(productIDs[ddProducts.SelectedIndex]);
                    // Retrieves and stores the initial price for the selected product.
                    chosenProductPrice = Convert.ToString(initialPrices[ddProducts.SelectedIndex - 1]);
                }
            }
            catch(Exception ex)
            {
                tbTransactionDetails.Text = "There appears to have been an intermittent error. Please enter your transaction information and press submit again.";
            }
        }

        // Defines the method responsible for validating the input in the quantity text box.
        protected int retrieveQuantity()
        {
            // Checks if anything has been inputted.
            if (tbNumberOfProduct.Text == "")
            {
                // Returns an error flag.
                return 0;
            }
            // Checks that only numbers (digits) have been inputted.
            else if (Regex.IsMatch(tbNumberOfProduct.Text, "\\D") != true)
            {
                // Does the necessary conversion and checks if the value is within the required range.
                int quantity = Convert.ToInt32(tbNumberOfProduct.Text);
                if (quantity >= 1 || quantity <= 200)
                {
                    // Returns the value entered.
                    return quantity;
                }
            }
            // Returns a different error flag.
            return -1;
        }

        // Defines the method responsible for validating the contents of the comments text box.
        protected string retrieveComment()
        {
            // Checks if the comment is not too long.
            if (tbComment.Text.Length >= 200)
            {
                // Returns an error flag if message is too long.
                return "*ERROR*";
            }
            // Otherwise, returns the contents of the text box.
            return tbComment.Text;
        }

        // Defines the event handler responsible for when the submit button is clicked.
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Retrieves the input for the quantity text box.
            int quantity = retrieveQuantity();
            // Retrieves the input for the comments text box.
            string comment = retrieveComment();

            // Ensures that all input fields have valid values.
            if (ddStores.SelectedIndex != 0 && ddLoyaltyNumber.SelectedIndex != 0 && ddEmployee.SelectedIndex != 0 && ddTransactionType.SelectedIndex != 0 && ddProducts.SelectedIndex != 0 && quantity != -1 && quantity != 0 && !comment.Equals("*Error*" ))
            {
                // Determines the date of the transaction.
                DateTime date = DateTime.Now.Date;
                // Determines the time of the transaction.
                TimeSpan time = DateTime.Now.TimeOfDay;

                // Prints a confirmation message detailing what was updated to the database.
                tbTransactionDetails.Text = "This transaction was executed on " + date.ToShortDateString() + " at " + time.ToString().Substring(0, time.ToString().IndexOf(".")) + "."
                    + Environment.NewLine + "The store selected is " + ddStores.SelectedItem.ToString().Trim() + " with its store ID as " + selectedStoreID + "."
                    + Environment.NewLine + "The loyalty number selected is " + ddLoyaltyNumber.SelectedItem.ToString().Trim() + " with its loyalty number ID as " + selectedLoyaltyNumberID + "."
                    + Environment.NewLine + "The employee selected is " + ddEmployee.SelectedItem.ToString().Trim() + " with their employee ID as " + selectedEmployeeID + "."
                    + Environment.NewLine + "The type of transaction selected is " + ddTransactionType.SelectedItem.ToString().Trim() + " with its transaction type ID as " + selectedTransactionTypeID + "."
                    + Environment.NewLine + "The product chosen is \"" + ddProducts.SelectedItem.ToString().Trim() + "\" with its product ID as " + selectedProductID + "."
                    + Environment.NewLine + "The quantity of product specified is " + quantity.ToString() + "."
                    + Environment.NewLine + "The price per unit for the product is " + chosenProductPrice + "."
                    + Environment.NewLine + "No coupon is given so the price per unit per customer is the same as the line above."
                    + Environment.NewLine + "The comment given is as follows: " + comment;

                // Calls a stored procedure from the database.
                SqlCommand storedProc = new SqlCommand("spAddTransactionAndDetail", connection);
                storedProc.CommandType = System.Data.CommandType.StoredProcedure;

                // Passes the values generated by the inputs to the stored procedure.
                storedProc.Parameters.Add("@LoyaltyID", System.Data.SqlDbType.Int);
                storedProc.Parameters["@LoyaltyID"].Value = selectedLoyaltyNumberID;
                storedProc.Parameters.Add("@DateOfTransaction", System.Data.SqlDbType.Date);
                storedProc.Parameters["@DateOfTransaction"].Value = date;
                storedProc.Parameters.Add("@TimeOfTransaction", System.Data.SqlDbType.Time);
                storedProc.Parameters["@TimeOfTransaction"].Value = time;
                storedProc.Parameters.Add("@TransactionTypeID", System.Data.SqlDbType.Int);
                storedProc.Parameters["@TransactionTypeID"].Value = selectedTransactionTypeID;
                storedProc.Parameters.Add("@StoreID", System.Data.SqlDbType.Int);
                storedProc.Parameters["@StoreID"].Value = selectedStoreID;
                storedProc.Parameters.Add("@EmplID", System.Data.SqlDbType.Int);
                storedProc.Parameters["@EmplID"].Value = selectedEmployeeID;
                storedProc.Parameters.Add("@ProductID", System.Data.SqlDbType.Int);
                storedProc.Parameters["@ProductID"].Value = selectedProductID;
                storedProc.Parameters.Add("@Qty", System.Data.SqlDbType.Int);
                storedProc.Parameters["@Qty"].Value = quantity;
                storedProc.Parameters.Add("@PricePerSellableUnitAsMarked", System.Data.SqlDbType.Money);
                storedProc.Parameters["@PricePerSellableUnitAsMarked"].Value = chosenProductPrice;
                storedProc.Parameters.Add("@PricePerSellableUnitToTheCustomer", System.Data.SqlDbType.Money);
                storedProc.Parameters["@PricePerSellableUnitToTheCustomer"].Value = chosenProductPrice;
                storedProc.Parameters.Add("@TransactionComment", System.Data.SqlDbType.NVarChar);
                storedProc.Parameters["@TransactionComment"].Value = comment;
                storedProc.Parameters.Add("@TransactionDetailComment", System.Data.SqlDbType.NVarChar);
                storedProc.Parameters["@TransactionDetailComment"].Value = comment;
                storedProc.Parameters.Add("@CouponDetailID", System.Data.SqlDbType.Int);
                storedProc.Parameters["@CouponDetailID"].Value = 0;
                storedProc.Parameters.Add("@TransactionID", System.Data.SqlDbType.Int);
                storedProc.Parameters["@TransactionID"].Value = 0;

                // Executes the stored procedure.
                storedProc.ExecuteNonQuery();
            }
            // Displays an error message if invalid inputs exist.
            else
            {
                // Prints a message if an invalid number is entered in the quantity text box.
                if (quantity == -1)
                {
                    tbTransactionDetails.Text = "The quantity must be a nonnegative number between 1 and 200 inclusively.";
                }
                // Prints a message if one or more input fields lack an inputted value.
                else
                {
                    tbTransactionDetails.Text = "Error: Please make selections from all drop down lists and fill in all text boxes before submitting.";

                }
            }

            // Resets the selected index positions and clears the text boxes for new inputs.
            ddStores.SelectedIndex = 0;
            ddLoyaltyNumber.SelectedIndex = 0;
            ddTransactionType.SelectedIndex = 0;
            ddEmployee.SelectedIndex = 0;
            ddProducts.SelectedIndex = 0;
            tbNumberOfProduct.Text = "";
            tbComment.Text = "";
        }
    }
}