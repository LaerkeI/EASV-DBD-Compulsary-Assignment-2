# EASV-DBD-Compulsary-Assignment-2

## Project Setup and Delivery:

### Briefly explain your design choices for using the relational database, NoSQL store, and caching.

I have used a relational database for placed orders because a relational database stores data in a structured way, is well-suited for transactions, 
and enforces ACID priciple and that's important when payments are being handled.  I chose to use MSSQL specifically because I have experience in using it. 

I have used a NoSQL database (MongoDB) for displaying book details as book details may have varying attributes. MongoDB is optimized for handling large amounts of data
and is well-suited for semi-structured and unstructured data and different data formats. MongoDB also scales well horizontally and that is useful as the book store expands their book catalog.
I chose to use MongoDB specifically because I have experience in using it.

I have used Redis Database for caching because it is an in-memory data store that is optimized for fast data retrieval.
though the GET request to fetch books is currently only a few milliseconds faster with caching compared to without: 

Without cache: 10-12 ms

With cache (cached for 10 seconds): 7-9 ms


### Provide instructions on how to set up and run the application.

1. Build and run the docker compose file. 


2. Run this command in the Developer Powershell terminal (or whatever is similar to that if not using Visual Studio): 
``sqlcmd -S localhost,1434 -U sa -P 'MyStrong!Passw0rd'`` 
Make sure to install mssql-tools if this command doesn't work. 


3. Run the script in mssql-init.sql in the root directory



4. Send a GET request to ``http://localhost:5000/api/book`` to display all books 



5. Send a POST request to ``http://localhost:5000/api/order`` to place an order in this JSON format:
```
{
    "customerId": <int>,
    "bookISBN": <string>, 
    "quantity": <int>
}
```

**END OF "Project Setup and Delivery"**


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


### Redis CLI


Monitor the Redis server’s activity to check if keys are being set and fetched:
``docker exec -it redis redis-cli keys *``

Enable monitoring in Redis to see real-time commands being executed:
``docker exec -it redis redis-cli monitor``

Get info on Redis' performance:
``docker exec -it redis redis-cli INFO stats``
