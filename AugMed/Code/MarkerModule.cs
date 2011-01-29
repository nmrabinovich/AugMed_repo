using System;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class MarkerModule
    {
        List<MarkerNode> patients;
        List<MarkerNode> equipment;

        public MarkerModule()
        {
            patients = new List<MarkerNode>();
            equipment = new List<MarkerNode>();
        }

        public String getMarkerNode()
        {
            return "Got Marker";
        }

        //public MarkerNode getPtMarkerNode(Scene scene)
        //public List<MarkerNode> getPtMarkerNode(Scene scene)
        public List<MarkerNode> getPtMarkerNode(Scene scene, SqlCeConnection thisConnection)    
        {
            int[] ids = new int[1];
            //ids[0] = 0;
            //return new MarkerNode(scene.MarkerTracker, "PatientMarkerConfig.txt", ids);
            //return new MarkerNode(scene.MarkerTracker, "Patient2MarkerConfig.txt", ids);
            /*
            patients.Add(new MarkerNode(scene.MarkerTracker, "PatientMarkerConfig.txt", ids));
            
            ids[0] = 2;
            patients.Add(new MarkerNode(scene.MarkerTracker, "Patient2MarkerConfig.txt", ids));
            ids[0] = 3;
            patients.Add(new MarkerNode(scene.MarkerTracker, "Patient3MarkerConfig.txt", ids));
            */
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "SELECT ID, MarkerID FROM MarkerInventory WHERE Patient=\'True\'";

                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ids[0] = (int)rdr["ID"];
                    patients.Add(new MarkerNode(scene.MarkerTracker, (String)rdr["MarkerID"], ids));
                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return patients;
        
        }

        //public MarkerNode getEquipMarkerNode(Scene scene)
        public List<MarkerNode> getEquipMarkerNode(Scene scene, SqlCeConnection thisConnection)
        {
            int[] ids = new int[1];
            //ids[0] = 1;
            //return new MarkerNode(scene.MarkerTracker, "EquipementMarkerConfig.txt", ids); //ids[1]);
            try
            {
                SqlCeCommand cmd = thisConnection.CreateCommand();
                cmd.CommandText = "SELECT ID, MarkerID FROM MarkerInventory WHERE Patient=\'False\'";

                SqlCeDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ids[0] = (int)rdr["ID"];
                    equipment.Add(new MarkerNode(scene.MarkerTracker, (String)rdr["MarkerID"], ids));
                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return equipment;
        }

        public Object getMarkerID()
        {
            return null;
        }
    }
}
