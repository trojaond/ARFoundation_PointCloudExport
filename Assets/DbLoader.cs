using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class DbLoader : MonoBehaviour
{

    private const string IP = "10.1.3.142";
    private const string LOCAL_IP = "127.0.0.1";

    private const string MONGO_URI = "mongodb://admin:password@" + LOCAL_IP + ":27017";
    public string DATABASE_NAME = "walk";
    public string COLLECTION_NAME = "points";
    private MongoClient client;
    private IMongoDatabase db;

    private IMongoCollection<Model_PointSet> pointsCollection;
    private int items;

    public enum Formats // your custom enumeration
    {
        ply
    };

    public enum Actions // your custom enumeration
    {
        Display,
        Export
    };
    public Actions ActionDropdown = Actions.Display;
    public Formats FormatDropdown = Formats.ply;

    public float animateRate = 0.3f;
    public GameObject addedPrefab;
    public GameObject updatedPrefab;
    //public bool display = true;

    private List<Model_PointSet> loadedData;
    private int loadedDataPointer = 0;

    

    void Start()
    {


        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
        Debug.Log(db);
        pointsCollection = db.GetCollection<Model_PointSet>(COLLECTION_NAME);
        if (pointsCollection == null) throw new Exception("cannot retireve collection " + COLLECTION_NAME);

         
        loadedData = pointsCollection.Find(t => true).ToList();
        if (loadedData.Count <= 0)
        {
            Debug.LogError("No Points in Collection " + COLLECTION_NAME);
            return;
        }
        
         Debug.Log("Loaded " + loadedData.Count +" items from the database");
        
        
        if (ActionDropdown == Actions.Display)
        {
            InvokeRepeating("LoadAndUpdate", animateRate, animateRate);
        }
        else if(FormatDropdown == Formats.ply)
        {
            
            Debug.Log("On start items: " + loadedData.Count);
            int countPoints = 0;
            List<String> header = new List<string>
             {
                 "ply",
                "format ascii 1.0",
                "element vertex 7",
                "property float x",
                "property float y",
                "property float z",
                "end_header"
             };
            foreach (var current in loadedData)
            {
                if (current.added.Count() > 0)
                {
                    foreach (var pointCloud in current.added)
                    {
                        foreach (var point in pointCloud)
                        {
                            countPoints++;
                            header.Add(point.x + " " + point.y + " " + point.z);
                        }
                    }
                }

                if (current.updated.Count() > 0)
                {
                    foreach (var pointCloud in current.updated)
                    {
                        foreach (var point in pointCloud)
                        {
                            countPoints++;
                            header.Add(point.x + " " + point.y + " " + point.z);
                        }
                    }
                }

            }

            header[2] = "element vertex "+countPoints;

            System.IO.File.WriteAllLines("space.ply", header);
            Debug.Log("Export is Done, counted points: " + countPoints);
        }
        else
        {
            Debug.Log("Not yet implemented");
           
        }



    }


    public void LoadAndUpdate()
    {
        var current = loadedData[loadedDataPointer];
        if (current == null) return;
        //GameObject add = displayPrefab;
        //add.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        if(current.added.Count() > 0)
        {
            foreach (var pointCloud in current.added)
            {
                foreach (var point in pointCloud)
                {
                    Instantiate(addedPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                }
            }
        }

        if (current.updated.Count() > 0)
        {
            foreach (var pointCloud in current.updated)
            {
                foreach (var point in pointCloud)
                {
                    Instantiate(updatedPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                }
            }
        }
       /* if (current.removed.Count() > 0)
        {
            foreach (var pointCloud in current.removed)
            {
                foreach (var point in pointCloud)
                {
                    Instantiate(addedPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity);
                }
            }
        }*/


        loadedDataPointer++;
        if(loadedDataPointer >= loadedData.Count)
        {
            CancelInvoke();
        }
        

    }


    private IEnumerable<Model_Point> ConvertPointCloud(ARPointCloud pointCloud)
    {
        if (!pointCloud.positions.HasValue) return null;
        List<Model_Point> ret = new List<Model_Point>();
        var length = pointCloud.positions.Value;
        foreach (var vector in pointCloud.positions.Value)
        {
            ret.Add(new Model_Point(vector.x, vector.y, vector.z));
        }
        return ret;
    }

}
