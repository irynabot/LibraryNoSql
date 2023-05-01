using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNoSql.Model
{
    public class Book
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("pages")]
        public int Pages { get; set; }
        [BsonElement("author")]
        public string Author { get; set; }
        [BsonElement("given_to_user_id")]
        public Guid GivenToUserId { get; set; }
    }
}
