# EASV-DBD-Compulsary-Assignment-2

## Just my notes because it sucks to make the same errors T-T

### Port-mapping T-T
When the mssql database and the mongo database were both dockerized but BookStoreApp was not (I sent requests through Swagger), 
the requests should be sent to ``localhost,<the-port-the-database-is-mapped-to-on-the-host-machine>`` which for mssql_server is 1434 and for mongodb_server is 27017.(External port)

When BookStoreApp is also dockerized, and the services mssql_server, mongodb_server and bookstore-app are all on the same network. 
Then the requests from Postman should be sent to ``<name-of-service>, <the-internal-port-in-Docker>``


### MSSQL Server

**Guide: Install mssql-tools in the Docker Container to get sqlcmd**

To install the mssql-tools in my running SQL Server container:

Access the container using bash:
``docker exec -it mssql_server bash``

Install mssql-tools in the container by running the following commands one by one:
```
# Install the necessary dependencies for mssql-tools

apt-get update

apt-get install -y curl gnupg

# Download and install Microsoft SQL Server tools

curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -

curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list

apt-get update

apt-get install -y mssql-tools unixodbc-dev
```

Then i can use ``sqlcmd -S localhost,1434 -U sa -P 'MyStrong!Passw0rd'`` to access the "BookStoreDb" database.
This command returns 1> and then i can write sql. Remember to add GO after every command.  

**The "BookStoreDb" that i connect to in SSMS (localhost,1434) is the same as the one i connect to with this command: ``sqlcmd -S localhost,1434 -U sa -P 'MyStrong!Passw0rd'``**

**When the volume is deleted, the sql scripts for creating the database and adding test data has to be rerun.**


**If error: Cannot open database "BookStoreDb" requested by the login. The login failed. Login failed for user 'sa'.**
 Connect to db through SSMS/shell and run: 
 ```
 ALTER LOGIN sa ENABLE;
 ALTER LOGIN sa WITH PASSWORD = 'MyStrong!Passw0rd';
```



### MongoDB Server


This command ``docker run --name mongodb -d -p 27017:27017 mongodb/mongodb-community-server:7.0.5-ubuntu2204`` creates a MongoDB server in Docker 
that can be connected to through MongoDB Compass with this connection string: ``mongodb://localhost:27017``

This command would create the MongoDB server in docker-compose.yml: 
``docker run --name mongodb -d -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=user -e MONGO_INITDB_ROOT_PASSWORD=pass -v mongo_data:/data/db mongodb/mongodb-community-server:7.0.5-ubuntu2204``

**How to connect to the MongoDB shell (mongosh)**
``docker exec -it mongodb_server mongosh -u user -p pass --authenticationDatabase admin``

**If i can connect to the MongoDb shell but not connect through MongoDB Compass and the connection string looks correct, uninstall MongoDB Compass and install it again T-T**

When connected use lower-case MongoDB commands (is case-sensitive): 
``use BookDetailsDb;`` 

A collection e.g."Books" will also be created after an object is inserted into it and there was no collection to begin with: 
``db.Books.insertOne({ title: "Sample Book", stockLevel: 10 });``

Basic mongosh commands:
```
	List databases: show dbs;
	Switch to database: use <database_name>;
	List collections: show collections;
	Get all documents from a collection: db.<collection_name>.find();
	Make the output pretty: db.<collection_name>.find().pretty();
```



