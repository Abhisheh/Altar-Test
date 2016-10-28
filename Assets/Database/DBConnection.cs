using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Assets.Database
{
    class DBConnection
    {
        private static MongoDatabase _database;

        public static MongoDatabase getAltarDB()
        {
            //create a local instance of a mongo client
            var _client = new MongoClient();

            //get the server for this instance
            var _server = _client.GetServer();

            //connect to the DB
            _database = _server.GetDatabase("AltarDB");

            return _database;
        }

    }
}
