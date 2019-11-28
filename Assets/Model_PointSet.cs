using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;


public class Model_PointSet
{
    public Model_PointSet(BsonDateTime time, IEnumerable<IEnumerable<Model_Point>> added, IEnumerable<IEnumerable<Model_Point>> updated, IEnumerable<IEnumerable<Model_Point>> removed)
    {
        this.time = time;
        this.added = added;
        this.updated = updated;
        this.removed = removed;
    }

    public ObjectId _id { set; get; }

    public BsonDateTime time {set; get;}

    public IEnumerable<IEnumerable<Model_Point>> added { set; get; }
    public IEnumerable<IEnumerable<Model_Point>> updated { set; get; }
    public IEnumerable<IEnumerable<Model_Point>> removed { set; get; }

   

}
