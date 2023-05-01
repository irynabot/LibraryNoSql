using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNoSql.Model
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        [BsonElement("login")]
        public string Login { get; set; }
        
        [BsonElement("password")]
        public string Password { get; set; }
    }
}
