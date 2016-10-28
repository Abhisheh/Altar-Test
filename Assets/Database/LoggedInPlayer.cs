using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Assets.Database.PersistenceProviders;

namespace Assets.Database
{
    class LoggedInPlayer
    {
        private static BsonDocument player;

        public static BsonDocument Player
        {
            get
            {
                return player;
            }

            set
            {
                if (value == null || PlayerPersistenceProvider.docIsAPlayer(value))
                    player = value;
                else
                    throw new BsonException("The document value provided was not a player and hence invalid!");
            }
        }
    }
}
