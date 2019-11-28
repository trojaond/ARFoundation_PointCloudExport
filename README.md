
#  AR Foundation PointCloud Export
Example project that use [*AR Foundation 3.0*] based on [ARFoundationSamples](https://github.com/Unity-Technologies/arfoundation-samples) repository.

It allows scan the enviroment in the mobile app, send information about reference (feature) points to the mongo database and then the special scene converts data from db to the file with `.ply`  format 
## Requirements

 1. This set of samples relies on `Unity 2019.2` with five packages:

	* ARSubsystems
	* ARCore XR Plugin
	* ARKit XR Plugin
	* ARKit Face Tracking
	* ARFoundation

 2. Unity project requires a runing `MongoDb` database opened on port 27017 with allowed firewall for incomming requests. 
 



## Getting Started

The project consists of 2 scenes
* ReferencePoints - serves for Scanning of the enviroment and sending the point to the database
* LoadedPoints - is for displaying/exporting the database points into PointCloud format `.ply`

**Scanning**

![enter image description here](https://i.ibb.co/NjJ2Mz1/Db-Connector.png)

In the Reference points scene see the `ARSessionOrigin` GameObject's component `DbConnector` where you should set the running database server IP address, database and collection name. 
Note: database and collection will be created automaticaly in case they are not present.

**Loading/Exporting**

![enter image description here](https://i.ibb.co/Yh51Kx3/DbLoader.png)

In the Load points scene see `db` GameObject's component `Db Loader` which by default connects to local ip address mongo server. You can again specify the database, collection and decide whether you want to generate points as game object in the Unity Scene *(Performance heavy!)* or to generate as a `.ply` file.

## Example of usage

1. Download the latest version of Unity 2019.2 or later.

2. Open Unity, and load the project at the *ReferencePoint* scene.

3. Pull  docker image [mongo](https://hub.docker.com/_/mongo) and run with 
`docker run -d --name mymongo -p 27017:27017  -e MONGO_INITDB_ROOT_USERNAME=admin    -e MONGO_INITDB_ROOT_PASSWORD=password mongo`

4. Allow incomming request in Firewall for port 27017
5. Find your server IP address and type it in DbConnector field
6. Build the application and use your mobile phone for scanning the enviroment.
7. Change to LoadPoint scene in Unity Editor, check the Export/Display action and run the Scene. You should see generated file `space.ply` in the AR Foundation_PointCloudExport folder
8. Use software such as [CloudCompare](https://www.danielgm.net/cc/) for file visualisation

Optional:

9. See the [AR Foundation Documentation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest?preview=1) for usage instructions and more information.

## Scanned enviroment
![enter image description here](https://i.ibb.co/RzCDvZ0/library3.png)
![enter image description here](https://i.ibb.co/WxYpWdY/lib4.png)

