using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Ryp�le.Models
{
    public class MongoRepo : IRepository
    {
        MongoClient client;
        IMongoDatabase database;
        IMongoCollection<Player> playerCollection;
        
        public MongoRepo()
        {
            client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("game");
            playerCollection = database.GetCollection<Player>("players");
        }

        public async Task<Player> CreatePlayer(Player player)
        {
            await playerCollection.InsertOneAsync(player);
            return player;  //Miksi palauttaa sama player? Miksei esim bool onnistuiko vai ei
        }

        public async Task<Player> GetPlayer(Guid playerId)
        {
            var filter = Builders<Player>.Filter.Eq("id", playerId);
            return await playerCollection.Find(filter).FirstAsync();
        }

        public async Task<Player[]> GetAllPlayers()
        {
            List<Player> players = await playerCollection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player> UpdatePlayer(Guid id, ModifiedPlayer player)
        {
            Player replacePlayer = GetPlayer(id).Result;
            replacePlayer.Score = player.Score;
            var filter = Builders<Player>.Filter.Eq("id", id);
            await playerCollection.ReplaceOneAsync(filter, replacePlayer);
            return replacePlayer;
        }

        public async Task<Player> DeletePlayer(Guid playerId)
        {
            var filter = Builders<Player>.Filter.Eq("id", playerId);
            playerCollection.DeleteOne(filter);
            return null;
        }

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            var temp = GetPlayer(playerId);
            temp.Result.itemList.Add(item);
            return item;
        }

public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            var temp = GetPlayer(playerId);

            foreach(var itemvar in temp.Result.itemList)
            {
                if(itemvar.ItemId  == itemId)
                {
                    return itemvar;
                }
            }
            return null;
        }
        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            return GetPlayer(playerId).Result.itemList.ToArray();
        }
        public async Task<Item> UpdateItem(Guid playerId, Item item)
        {
            var temp = GetPlayer(playerId);

            foreach(var itemvar in temp.Result.itemList)
            {
                if (itemvar.ItemId == item.ItemId)
                {
                    temp.Result.itemList.Remove(itemvar);
                    temp.Result.itemList.Add(item);
                    return item;
                }


            }
            return null;
        }
        public async Task<Item> DeleteItem(Guid playerId, Item item)
        {
            var temp = GetPlayer(playerId);

            foreach(var itemvar in temp.Result.itemList)
            {
                if (itemvar.ItemId == item.ItemId)
                {
                    temp.Result.itemList.Remove(itemvar);
                    return itemvar;
                }

            }
            return null;

        }








        
    }
}


