using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Window
    {
        public LogInPage()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            String role = "";
            try
            {
                SqlCeConnection thisConnection = new SqlCeConnection("Data Source=AugMedDB.sdf;Password=");
                thisConnection.Open();
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "SELECT Password, Role FROM Users WHERE Username=\'" + this.txtUserName.Text.Trim() + "\'";

                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (this.txtPassword.Password.Equals(rdr["Password"].ToString()))
                    {
                        //this.NavigationService.Navigate(new Uri("AddExpensesPage.xaml", UriKind.Relative));
                        role = rdr["Role"].ToString();
                        MainMenuPage menu = new MainMenuPage(role, thisConnection);
                        this.Close();
                        menu.Show();


                        //this.Close();

                    }
                    else
                    {
                        this.ErrorBox.Text = "Invalid Username and/or Password";
                    }

                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException err)
            {
                Console.WriteLine(err.Message);
            }
        }

    }
}


