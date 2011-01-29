using System;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GoblinXNA;
using GoblinXNA.Graphics;
using GoblinXNA.SceneGraph;
using Model = GoblinXNA.Graphics.Model;
using GoblinXNA.Graphics.Geometry;
using GoblinXNA.Device.Capture;
using GoblinXNA.Device.Vision;
using GoblinXNA.Device.Vision.Marker;
using GoblinXNA.Device.Util;
using GoblinXNA.Physics;
using GoblinXNA.Helpers;

namespace AugMed
{
    public class PatientModule
    {
        public PatientModule()
        {
        }

        /*
        public String getSetting(Object PtID, Object EqID, String Control)
        {
            if (PtID.Equals("PatientMarkerConfig.txt"))
            {
                if (EqID.Equals("EquipementMarkerConfig.txt"))
                {
                    if (Control.Equals("RateButton"))
                    {
                        return "5 drops per min";
                    }
                    else if (Control.Equals("Lever"))
                    {
                        return "Up";
                    }
                }
            }
            else if (PtID.Equals("Patient2MarkerConfig.txt"))
            {
                if (EqID.Equals("EquipementMarkerConfig.txt"))
                {
                    if (Control.Equals("RateButton"))
                    {
                        return "2 drops per min";
                    }
                    else if (Control.Equals("Lever"))
                    {
                        return "Down";
                    }
                }
            }
            return "no setting set";
        }*/


        public String getSetting(Object PatientID, Object EqID, String ControlButton, SqlCeConnection thisConnection)
        {
            int PatID = 0, EquipmentID = 0;
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "SELECT ID FROM MarkerInventory WHERE MarkerID=\'" + PatientID.ToString() + "\'";
                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PatID = (int)rdr["ID"];
                }
                cmd.CommandText = "SELECT ID FROM MarkerInventory WHERE MarkerID=\'" + EqID.ToString() + "\'";
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EquipmentID = (int)rdr["ID"];
                }
                cmd.CommandText = "SELECT Setting FROM Patient WHERE PtID=" + PatID + " and EquipID=" + EquipmentID + " and Control=\'" + ControlButton.ToString() + "\'";
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    return (String)rdr["Setting"];
                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return "no setting set";
        }

        /*
        public List<String> getControls(Object PtID, Object EqID)
        {
            List<String> controls = new List<String>();
            controls.Add("RateButton");
            controls.Add("Lever");
            return controls;
        }
        */

        public List<String> getControls(Object PatientID, Object EqID, SqlCeConnection thisConnection)
        {
            int PatID = 0, EquipmentID = 0;
            List<String> controls = new List<String>();
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "SELECT ID FROM MarkerInventory WHERE MarkerID=\'" + PatientID.ToString() + "\'";
                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PatID = (int)rdr["ID"];
                }
                cmd.CommandText = "SELECT ID FROM MarkerInventory WHERE MarkerID=\'" + EqID.ToString() + "\'";
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EquipmentID = (int)rdr["ID"];
                }
                cmd.CommandText = "SELECT Control FROM Patient WHERE PtID=" + PatID + " and EquipID=" + EquipmentID;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    controls.Add((String)rdr["Control"]);
                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return controls;
        }

        /*
        public List<Object> getListEquip(Object PtID)
        {
            List<Object> equip = new List<object>();
            equip.Add("EquipementMarkerConfig.txt");
            return equip;
        }
        */

        public List<Object> getListEquip(Object PatID, SqlCeConnection thisConnection)
        {
            List<Object> equip = new List<Object>();
            //int[] ids = new int[1];
            //ids[0] = 1;
            //return new MarkerNode(scene.MarkerTracker, "EquipementMarkerConfig.txt", ids); //ids[1]);
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                //cmd.CommandText = "SELECT MarkerID FROM MarkerInventory WHERE Patient=\'False\'";
                //cmd.CommandText = "SELECT MarkerID FROM MarkerInventory WHERE (ID IN (SELECT EquipID FROM Patient WHERE (PtID IN (SELECT ID FROM MarkerInventory WHERE MarkerID=\' " + PatID.ToString() + "\'))))";
                cmd.CommandText = "SELECT MarkerID FROM MarkerInventory WHERE (ID IN (SELECT EquipID FROM Patient WHERE (PtID IN (SELECT ID FROM MarkerInventory AS MarkerInventory_1 WHERE (MarkerID = \'"+ PatID.ToString() +"\')))))";
                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    //ids[0] = (int)rdr["ID"];
                    //equipment.Add(new MarkerNode(scene.MarkerTracker, (String)rdr["MarkerID"], ids));
                    equip.Add((String)rdr["MarkerID"]);
                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return equip;
        }

        /*
        public String getPatientName(Object markerID)
        {
            if (markerID.Equals("PatientMarkerConfig.txt"))
                return "Smith, John";
            else if (markerID.Equals("Patient2MarkerConfig.txt"))
                return "Doe, Jane";
            else if (markerID.Equals("Patient3MarkerConfig.txt"))
                return "Doe, Jade";
            return "No Patient";
        }
        */

        
        public String getPatientName(Object markerID, SqlCeConnection thisConnection)
        {
            //return patient.getPatientName(markerID);
            String name = "";
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "SELECT LastName, FirstName FROM MarkerInventory WHERE MarkerID="+"\'"+markerID.ToString()+"\'";
                
                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    name = rdr["LastName"] + ", " + rdr["FirstName"];
                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return name;
        }
        
    }
}
