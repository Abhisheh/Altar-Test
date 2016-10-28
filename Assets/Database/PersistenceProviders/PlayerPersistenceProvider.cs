using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Database.PersistenceProviders
{
    class PlayerPersistenceProvider
    {
        /// <summary>
        /// Gets all the users (players) from the DB
        /// </summary>
        /// <returns></returns>
        public static MongoCollection<BsonDocument> getAllUsersCollection()
        {
            return DBConnection.getAltarDB().GetCollection<BsonDocument>("players");
        }

        /// <summary>
        /// Get the total number of players from the DB
        /// </summary>
        /// <returns></returns>
        public static int getPlayerCount()
        {
            return (int)getAllUsersCollection().Count();
        }

        /// <summary>
        /// Obtain an unused ID to assign to a new player from the DB
        /// </summary>
        /// <returns></returns>
        public static int getUnusedID()
        {
            return (getPlayerCount() > 0)
                ? (int)getAllUsersCollection().Find(Query.Null).SetSortOrder(SortBy.Ascending("_id")).Last()["_id"] + 1
                : 1;
        }

        /// <summary>
        /// Check if a player does not exist
        /// </summary>
        /// <param name="username">Basis of comparison</param>
        /// <returns></returns>
        public static bool playerDoesNotExist(string username)
        {
            return (getAllUsersCollection().Find(Query.EQ("pl_usernameLower", username.ToLower())).Count() == 0);
        }

        /// <summary>
        /// Save the information of a new player
        /// </summary>
        /// <param name="username">Their username</param>
        /// <param name="password">Their password (Hashed and salted in method)</param>
        public static void addNewPlayer(string username, string password)
        {
            byte[] salt = PasswordHashAndSalt.CreateSalt();

            var player = new BsonDocument
            {
                { "_id", getUnusedID() },
                { "pl_username", username},
                { "pl_usernameLower", username.ToLower()},
                { "pl_password", PasswordHashAndSalt.GenerateSaltedHash(PasswordHashAndSalt.getBytes(password), salt)},
                { "pl_passwordSalt", salt},
                { "pl_joinDate", DateTime.Today.ToString("dd.MM.yyyy") },
                { "pl_records", new BsonArray { createPlayerRecord(1, "Registered for the game.") } }
            };

            getAllUsersCollection().Insert(player);

        }

        /// <summary>
        /// Retrieve the info of a specific player
        /// in the form of a BSonDocument
        /// (Used for logged in player)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static BsonDocument getSpecificPlayer(string username, string password)
        {

            var player = getAllUsersCollection().FindOne(Query.EQ("pl_username", username));

            return player["pl_password"].
                Equals(PasswordHashAndSalt.GenerateSaltedHash(PasswordHashAndSalt.getBytes(password), player["pl_passwordSalt"].AsByteArray))
                ? player: null;
        }

        /// <summary>
        /// Retrieve the info of a specific player
        /// in the form of a BSonDocument
        /// (Used for  other opertations)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BsonDocument getSpecificPlayer(long id)
        {

            if(getAllUsersCollection().FindOne(Query.EQ("_id", id)) == null)
                throw new MongoQueryException("The ID is invalid.");
            else 
                return getAllUsersCollection().FindOne(Query.EQ("_id", id));
        }

        /// <summary>
        /// Check whether a user(player) already has a playername
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool doesPlayerHaveAPlayername(BsonDocument player)
        {
            return player.Any(f => f.Name == "pl_playername");
        }

        /// <summary>
        /// Check if a provided doc (claiming to be a player)
        /// is actually in the database or not
        /// </summary>
        /// <param name="playerDoc"></param>
        /// <returns></returns>
        public static bool docIsAPlayer(BsonDocument playerDoc)
        {
            return getAllUsersCollection().Find(Query.Null).Any(doc => doc == playerDoc);
        }


        /// <summary>
        /// Create a new record
        /// for a player's action history
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="recordMessage"></param>
        /// <returns></returns>
        public static BsonDocument createPlayerRecord(long recordID, string recordMessage)
        {
            return new BsonDocument {
                            { "_recordID",  recordID},
                            { "pl_recordSummary", recordMessage },
                            { "pl_recordDate", DateTime.Today.ToString("dd.MM.yyyy") }
                        };
        }

        /// <summary>
        /// Verify if the specified playername already exists
        /// </summary>
        /// <param name="playername"></param>
        /// <returns></returns>
        public static bool doesThePlayernameAlreadyExist(string playername)
        {
            return getAllUsersCollection().Find(Query.EQ("pl_playername", playername)).Count() > 0;
        }
        
        /// <summary>
        /// Set the name for the player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="playername"></param>
        public static void setPlayerName(BsonDocument player, string playername)
        {
            player.Add(new BsonElement("pl_playername", playername));
            player.Add("pl_level", 1);
            long playerID = player["_id"].ToInt64();

            BsonDocument newRecord = createPlayerRecord(getUnusedRecordID(playerID), "Became a player");
            
            player["pl_records"].AsBsonArray.Add(newRecord);

            savePlayer(player);
        }
        

        /// <summary>
        /// Get a record ID that does not exist for a specific player
        /// (Records are saved as arrays in a player's data)
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public static long getUnusedRecordID(long playerID)
        {
            return
                getSpecificPlayer(playerID)["pl_records"].AsBsonArray.Last()["_recordID"].ToInt64();
        }

        /// <summary>
        /// Save a player after some information about them has changed
        /// </summary>
        /// <param name="player"></param>
        private static void savePlayer(BsonDocument player)
        {
            getAllUsersCollection().Save(player);
            LoggedInPlayer.Player = player;
        }

    }
}
