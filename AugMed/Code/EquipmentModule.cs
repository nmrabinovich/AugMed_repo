using System;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
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
    public class EquipmentModule
    {
        public EquipmentModule()
        {
        }

        /*
        public Vector3 getLocation(Object EqID, String Control, SqlCeConnection thisConnection)
        {
            Vector3 vect; 
            if (EqID.Equals("EquipementMarkerConfig.txt"))
            {
                if (Control.Equals("RateButton"))
                {
                    vect = new Vector3(-35, 18, 8);
                    //return new Vector3(-35, 18, 8);
                    return vect;
                }
                else if (Control.Equals("Lever"))
                {
                    vect = new Vector3(35, -18, 8);
                    //return new Vector3(35, -18, 8);
                    return vect;
                }
            }
            vect = Vector3.One;
            //return Vector3.One;
            return vect;
        }
        */

        
        public Vector3 getLocation(Object EqID, String ControlButton, SqlCeConnection thisConnection)
        {
            int EquipID = 0;
            Vector3 vect;
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "SELECT ID FROM MarkerInventory WHERE MarkerID=\'" + EqID.ToString() + "\'";
                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EquipID = (int)rdr["ID"];
                }

                cmd.CommandText = "SELECT X, Y, Z FROM Equipment WHERE ID=" + EquipID + " and Control=\'" + ControlButton.ToString() + "\'";
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["X"] != DBNull.Value && rdr["Y"] != DBNull.Value && rdr["Z"] != DBNull.Value)
                    {
                        float X = System.Convert.ToSingle(rdr["X"].ToString());
                        float Y = System.Convert.ToSingle(rdr["Y"].ToString());
                        float Z = System.Convert.ToSingle(rdr["Z"].ToString());
                        vect = new Vector3(X, Y, Z);
                        //return new Vector3((float)rdr["X"], (float)rdr["Y"], (float)rdr["Z"]);
                        return vect;
                    }
                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            vect = Vector3.One;
            //return Vector3.One;
            return vect;
        }
        

        /*
        public String getText(Object EqID, String Control)
        {
            if (EqID.Equals("EquipementMarkerConfig.txt"))
            {
                if (Control.Equals("RateButton"))
                {
                    return "The rate should be: ";
                }
                else if (Control.Equals("Lever"))
                {
                    return "The Lever should be: ";
                }
            }
            return "This control should be: ";
        }
        */

        public String getText(Object EqID, String ControlButton, SqlCeConnection thisConnection)
        {
            //return patient.getPatientName(markerID);
            String text = "";
            int EquipID = 0;
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "SELECT ID FROM MarkerInventory WHERE MarkerID=\'" + EqID.ToString() + "\'";
                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EquipID = (int)rdr["ID"];
                }

                cmd.CommandText = "SELECT Text FROM Equipment WHERE ID=" + EquipID + " and Control=\'" + ControlButton.ToString() + "\'";
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    text = (String)rdr["Text"]; ;
                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return text;
        }
        
    }
}
