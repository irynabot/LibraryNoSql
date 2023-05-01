using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNoSql.ApiModel
{
    public class GiveBookToUserApiModel
    {
        public ObjectId bookId { get; set; }
        public ObjectId userId { get; set; }
    }
}
