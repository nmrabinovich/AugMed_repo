using System;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
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
    public class CentralUnit
    {
        List<MarkerNode> patientMarkers;
        Hashtable equipmentMarkers;
        MarkerModule markerM;
        PatientModule patient;
        EquipmentModule equipment;
        SqlCeConnection thisConnection;
        Object currentMarkerID = null;
        String currentName = null;

        public CentralUnit()
        {
            patientMarkers = new List<MarkerNode>();
            equipmentMarkers = new Hashtable();
            markerM = new MarkerModule();
            patient = new PatientModule();
            equipment = new EquipmentModule();
            SqlDBConnect();
        }

        
        public void SqlDBConnect()
        {
            try
            {
                thisConnection = new SqlCeConnection("Data Source=AugMedDB.sdf;Password=");
                thisConnection.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public String getPatientName(Object markerID)
        {
            if ((currentMarkerID == null) || (!currentMarkerID.Equals(markerID)))
            {
                //currentName = patient.getPatientName(markerID);
                currentName = patient.getPatientName(markerID, thisConnection);
                currentMarkerID = markerID;
            }
            return currentName;
        }

        /*
        public String getPatientName(Object markerID)
        {
            //return patient.getPatientName(markerID);
            String name = "";

            if ((currentMarkerID==null) || (!currentMarkerID.Equals(markerID)))
            {
                try
                {
                    thisConnection = new SqlCeConnection("Data Source=AugMedDB.sdf;Password=");
                    thisConnection.Open();
                    SqlCeCommand cmd = thisConnection.CreateCommand();
                    cmd.CommandText = "SELECT LastName, FirstName FROM MarkerInventory WHERE MarkerID="+"\'"+markerID.ToString()+"\'";

                    SqlCeDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        name = rdr["LastName"] + ", " + rdr["FirstName"];
                    }
                    currentName = name;
                    currentMarkerID = markerID;
                    rdr.Close();
                    cmd.Dispose();
                    thisConnection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return currentName;
        }
        */

        public void RemoveAllChildren(Object markerID)
        {
            //get the equipment associated with the pt with id = markerID
            //getEquipment(patient markerID) -> adds equipment
            //remove their children
        }

        public List<MarkerNode> getAllPatientMarkers(Scene scene)
        {
            //MarkerNode temp = markerM.getPtMarkerNode(scene);
            //patientMarkers.Add(temp);
            //patientMarkers = markerM.getPtMarkerNode(scene);
            patientMarkers = markerM.getPtMarkerNode(scene, thisConnection);
            return patientMarkers;
        }

        public Hashtable getAllEquipmentMarkers(Scene scene)
        {
            List<MarkerNode> temp = markerM.getEquipMarkerNode(scene, thisConnection);
            foreach (MarkerNode marker in temp)
            {
                equipmentMarkers.Add(marker.MarkerID, marker);
            }
            return equipmentMarkers;
        }

        public List<Object> getEquipmentForPatient(Object PtMarkerID)
        {
            return patient.getListEquip(PtMarkerID, thisConnection);
        }

        public Hashtable getInformation(Object PtMarkerID)
        {
            Hashtable hsh = new Hashtable();
            List<Object> equip = patient.getListEquip(PtMarkerID, thisConnection);
            foreach (Object eqID in equip)
            {
                List<Text3D> nodes = AggregateData(PtMarkerID, eqID);
                hsh.Add(eqID, nodes);
            }
            return hsh;
        }

        public List<Text3D> AggregateData(Object PtID, Object EqID)
        {
            List<Text3D> infoList = new List<Text3D>();
            foreach (String control in patient.getControls(PtID,EqID,thisConnection))
            {
                Text3D info = new Text3D();
                //info.Text = equipment.getText(EqID, control) + patient.getSetting(PtID, EqID, control);
                info.Text = equipment.getText(EqID, control, thisConnection) + patient.getSetting(PtID, EqID, control, thisConnection);
                info.Transform = equipment.getLocation(EqID, control, thisConnection);
                infoList.Add(info);
            }
            return infoList;
        }

    }   
}
