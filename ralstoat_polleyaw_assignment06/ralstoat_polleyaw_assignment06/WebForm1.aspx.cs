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
using System.Text.RegularExpressions;

namespace ralstoat_polleyaw_assignment06
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private static System.Data.SqlClient.SqlConnection connection;
        private static SqlCommand command;
        private static SqlDataReader reader;

        private ArrayList storeIDs = new ArrayList();
        private int selectedStoreID;

        private ArrayList loyaltyNumberIDs = new ArrayList();
        private int selectedLoyaltyNumberID;

        private ArrayList employeeIDs = new ArrayList();
        private int selectedEmployeeID;

        private ArrayList transactionTypeIDs = new ArrayList();
        private int selectedTransactionTypeID;

        private ArrayList productIDs = new ArrayList();
        private int selectedProductID;
        private ArrayList initialPrices = new ArrayList();
        private string chosenProductPrice;

        protected void Page_Load(object sender, EventArgs e)
        {
            openConnection();
            ddStores.Items.Clear();
            ddLoyaltyNumber.Items.Clear();
            ddTransactionType.Items.Clear();
            ddEmployee.Items.Clear();
            ddProducts.Items.Clear();
            tbNumberOfProduct.Text = "";
            tbComment.Text = "";
            readData(ddStores, "SELECT COUNT(*) FROM tStore; SELECT StoreID, Store FROM tStore;", storeIDs);

            readData(ddLoyaltyNumber, "SELECT COUNT(*) FROM tLoyalty; SELECT LoyaltyID, LoyaltyNumber FROM tLoyalty;", loyaltyNumberIDs);

            readData(ddEmployee, "SELECT COUNT(*) FROM tEmpl; SELECT EmplID, FirstName, LastName FROM tEmpl;", employeeIDs);

            readData(ddTransactionType, "SELECT COUNT(*) FROM tTransactionType; SELECT TransactionTypeID, TransactionType FROM tTransactionType;", transactionTypeIDs);

            readData(ddProducts, "SELECT COUNT(*) FROM tProduct; SELECT tProduct.ProductID, tBrand.Brand, tName.Name, tProduct.Description, tProduct.InitialPricePerSellableUnit FROM tProduct INNER JOIN tBrand ON tProduct.BrandID = tBrand.BrandID INNER JOIN tName ON tProduct.NameID = tName.NameID;", productIDs);
        }

        private void readData(DropDownList control, string query, ArrayList primaryKeys)
        {
            // Stores the results of the query.
            String results;
            String results2;
            String results3;
            String results4;
            int ids;
            int count = -1;

            // Establishes the command for the given query on the connection.
            command = new SqlCommand(query, connection);

            // Attempts to read from the database.
            try
            {
                // Reads from the database.
                reader = command.ExecuteReader();

                // Adds spaces between the items returned.
                control.Items.Add(" ");
                primaryKeys.Add(-1);

                // Determines if anything was returned.
                while (reader.HasRows)
                {
                    // Loops through all items that match the query in the database.
                    while (reader.Read())
                    {
                        if (reader.FieldCount == 1)
                        {
                            count = reader.GetInt32(0);
                        }
                        else if (reader.FieldCount == 2)
                        {
                            // Stores the returns.
                            ids = reader.GetInt32(0);
                            results = reader.GetString(1);

                            // Adds returned ids to ArrayList.
                            primaryKeys.Add(ids);
                            // Adds what is returned to the drop down list.
                            if (control.Items.Count != count + 2)
                            {
                                control.Items.Add(results);
                            }    
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
                            if (control.Items.Count != count + 2)
                            {
                                control.Items.Add(results + " " + results2);
                            }
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
                            if (control.Items.Count != count + 2)
                            {
                                control.Items.Add(results + " " + results2 + " " + results3);
                            }

                            // Adds extra information requested to the appropriate class member.
                            initialPrices.Add(results4);
                        }
                    }
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
                selectedLoyaltyNumberID = Convert.ToInt32(loyaltyNumberIDs[ddLoyaltyNumber.SelectedIndex]);
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

        protected void ddProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddProducts.SelectedIndex != 0)
            {
                int stuff = ddProducts.SelectedIndex;
                int stuff2 = productIDs.Count;
                selectedProductID = Convert.ToInt32(productIDs[ddProducts.SelectedIndex]);
                chosenProductPrice = Convert.ToString(initialPrices[ddProducts.SelectedIndex - 1]);
            }
        }

        protected int retrieveQuantity()
        {
            if (tbNumberOfProduct.Text == "")
            {
                return 0;
            }
            else if (Regex.IsMatch(tbNumberOfProduct.Text, "\\D") != true)
            {
                int quantity = Convert.ToInt32(tbNumberOfProduct.Text);
                if (quantity >= 1 || quantity <= 200)
                {
                    return quantity;
                }
            }
            return -1;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int quantity = retrieveQuantity();
            if (ddStores.SelectedIndex != 0 && ddLoyaltyNumber.SelectedIndex != 0 && ddEmployee.SelectedIndex != 0 && ddTransactionType.SelectedIndex != 0 && ddProducts.SelectedIndex != 0 && quantity != -1 && quantity != 0)
            {
                DateTime date = DateTime.Now.Date;
                TimeSpan time = DateTime.Now.TimeOfDay;

                tbTransactionDetails.Text = "This transaction was executed on " + date.ToShortDateString() + " at " + time.ToString().Substring(0, time.ToString().IndexOf(".")) + "."
                    + Environment.NewLine + "The store selected is " + ddStores.SelectedItem.ToString().Trim() + " with its store ID as " + selectedStoreID + "."
                    + Environment.NewLine + "The loyalty number selected is " + ddLoyaltyNumber.SelectedItem.ToString().Trim() + " with its loyalty number ID as " + selectedLoyaltyNumberID + "."
                    + Environment.NewLine + "The employee selected is " + ddEmployee.SelectedItem.ToString().Trim() + " with their employee ID as " + selectedEmployeeID + "."
                    + Environment.NewLine + "The type of transaction selected is " + ddTransactionType.SelectedItem.ToString().Trim() + " with its transaction type ID as " + selectedTransactionTypeID + "."
                    + Environment.NewLine + "The product chosen is \"" + ddProducts.SelectedItem.ToString().Trim() + "\" with its product ID as " + selectedProductID + "."
                    + Environment.NewLine + "The quantity of product specified is " + quantity.ToString() + "."
                    + Environment.NewLine + "The price per unit for the product is " + chosenProductPrice + "."
                    + Environment.NewLine + "No coupon is given so the price per unit per customer is the same as the line above."
                    + Environment.NewLine + "The comment given is as follows: " + tbComment.Text.ToString();

                SqlCommand storedProc = new SqlCommand("spAddTransactionAndDetail", connection);
                storedProc.CommandType = System.Data.CommandType.StoredProcedure;

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
                storedProc.Parameters["@TransactionComment"].Value = tbComment.Text.ToString();
                storedProc.Parameters.Add("@TransactionDetailComment", System.Data.SqlDbType.NVarChar);
                storedProc.Parameters["@TransactionDetailComment"].Value = tbComment.Text.ToString();
                storedProc.Parameters.Add("@CouponDetailID", System.Data.SqlDbType.Int);
                storedProc.Parameters["@CouponDetailID"].Value = 0;
                storedProc.Parameters.Add("@TransactionID", System.Data.SqlDbType.Int);
                storedProc.Parameters["@TransactionID"].Value = 0;

                //storedProc.ExecuteNonQuery(); 
            }
            else
            {
                if (quantity == -1)
                {
                    tbTransactionDetails.Text = "The quantity must be a nonnegative number between 1 and 200";
                }
                else
                {
                    tbTransactionDetails.Text = "Error: Please make selections from the drop downs and fill in the text boxes before submitting.";

                }
            }
        }
    }
}