using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARReferencePointManager))]
[RequireComponent(typeof(ARPointCloudManager))]
public class DbConnector : MonoBehaviour
{


    private List<ARReferencePoint> m_ReferencePoints;
    private ARReferencePointManager m_ReferencePointManager;
    private ARPointCloudManager m_ReferencePointCloudManager;


    // Start is called before the first frame update

    public string IP = "10.6.209.186";
    private const string LOCAL_IP = "127.0.0.1";

    private string MONGO_URI = "";
    public string DATABASE_NAME = "walk";
    public string COLLECTION_NAME = "points";
    private MongoClient client;
    private IMongoDatabase db;

    private IMongoCollection<Model_PointSet> pointsCollection;


    void Awake()
    {
        m_ReferencePointManager = GetComponent<ARReferencePointManager>();
        m_ReferencePointCloudManager = GetComponent<ARPointCloudManager>();
        m_ReferencePoints = new List<ARReferencePoint>();
    }

    void Start()
    {

        MONGO_URI = "mongodb://admin:password@" + IP + ":27017";

        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
        Debug.Log(db);
        pointsCollection = db.GetCollection<Model_PointSet>(COLLECTION_NAME);
        if(pointsCollection == null) db.CreateCollection(COLLECTION_NAME);

        pointsCollection = db.GetCollection<Model_PointSet>("points");
        if (pointsCollection == null) throw new Exception("cannot create and retireve collection " + COLLECTION_NAME);


        var data = pointsCollection.Find(t => true).ToList();
        Debug.Log("On start items: "+ data.Count);

        

        var loadedFile = pointsCollection.Find(t => true).ToList();
        foreach (var item in loadedFile){
            Debug.Log(item.ToString());
        }
       m_ReferencePointCloudManager.pointCloudsChanged += delegate (ARPointCloudChangedEventArgs args)
       {
           Debug.Log("EVENT fired");
           Publish(args.added, args.updated, args.removed);
       };
    }


    public void Publish(List<ARPointCloud> added, List<ARPointCloud> updated, List<ARPointCloud> removed)
    {
        pointsCollection.InsertOne(new Model_PointSet(
            new BsonDateTime(DateTime.Now), 
            added.Select(x => ConvertPointCloud(x)) , 
            updated.Select(x => ConvertPointCloud(x)), 
            removed.Select(x => ConvertPointCloud(x))));

    }


    private IEnumerable<Model_Point> ConvertPointCloud(ARPointCloud pointCloud)
    {
        if (!pointCloud.positions.HasValue) return null;
        List<Model_Point> ret = new List<Model_Point>();
        var length = pointCloud.positions.Value;
        foreach ( var vector in pointCloud.positions.Value)
        {
            ret.Add(new Model_Point(vector.x, vector.y, vector.z));
        }
        return ret;
    }
 
}
