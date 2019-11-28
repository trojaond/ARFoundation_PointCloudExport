using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;


public class Model_Point
{
    public Model_Point(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    //public ObjectId _id { set; get; }

    //public int ActiveConnection { set; get; }
    public float x { set; get; }
    public float y { set; get; }
    public float z { set; get; }


    public override string ToString()
    {
        return "(" + x + ", " + y + "," + z + ")";
    }

}
