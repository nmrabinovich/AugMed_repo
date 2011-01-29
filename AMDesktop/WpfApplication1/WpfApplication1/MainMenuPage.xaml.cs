using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Window
    {
        SqlCeConnection thisConnection;
        DataSet ds;
        ObservableCollection<PtSettings> Pts = new ObservableCollection<PtSettings>(); 

        public MainMenuPage(String Role, SqlCeConnection Connection)
        {
            InitializeComponent();
            this.thisConnection = Connection;
        }

        public ObservableCollection<PtSettings> PtS
        { get { return PtS; } }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /* on exit go back to login page*/
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LogInPage login = new LogInPage();
            login.Show();
            this.Close();

        }

        private void SearchByMarker_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /*This button is to modify settings */
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //create new modSetting window based on active item in list
            ModSetting modify = new ModSetting((DataRowView)PatientView.SelectedItem, ds, thisConnection);
            modify.Show();
 
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

        }

        /* this is to search by first and last name */
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            int id = -1;
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "SELECT ID FROM MarkerInventory WHERE LastName=\'" + this.lastNameTxt.Text.Trim() + "\' and FirstName=\'" + this.firstNameTxt.Text.Trim() + "\'";
                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    id = (int)rdr["ID"];
                }
                cmd.CommandText = "SELECT m.Equipment, p.Control, p.Setting FROM Patient p, MarkerInventory m WHERE m.ID=p.EquipID and p.PtID=" + id;
                
                SqlCeDataAdapter adp = new SqlCeDataAdapter(cmd);
                ds = new DataSet();
                adp.Fill(ds);
                PatientView.DataContext = ds.Tables[0];

                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException err)
            {
                Console.WriteLine(err.Message);
            }
        }

        private void NewMarker_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PatientView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


    }

    public class PtSettings
    {
        public string Equipment { get; set; }
        public string Control { get; set; }
        public string Setting { get; set; }
    }
}
