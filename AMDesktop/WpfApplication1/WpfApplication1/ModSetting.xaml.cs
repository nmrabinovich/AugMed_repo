using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlServerCe;
using System.Data;
using System.Data.SqlClient;
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

using System.Data.SqlServerCe;
using System.Data.SqlClient;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ModSetting : Window
    {
        SqlCeConnection thisConnection;
        DataSet ds;
        public ModSetting(DataRowView data, DataSet dt, SqlCeConnection Connection)
        {
            InitializeComponent();
            //String selected = item.ToString();
            EquipmentTxt.Text = data.Row[0].ToString();
            ControlTxt.Text = data.Row[1].ToString();
            SettingTxt.Text = data.Row[2].ToString();
            thisConnection = Connection;
            ds = dt;
            //EquipmentTxt.Text = item.ToString();
        }

        private void SettingTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ChangeSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "UPDATE Patient SET Setting = \'" + SettingTxt.Text + "\' WHERE PtID=0 , EquipID=1, Control='Lever'" ;

                SqlCeDataAdapter adp = new SqlCeDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Update(ds.Tables[0]);
                //PatientView.DataContext = ds.Tables[0];

                //rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}
