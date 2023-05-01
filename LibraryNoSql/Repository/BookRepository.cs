using LibraryNoSql.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryNoSql.Repository
{
    public class BookRepository
    {
        private readonly IMongoCollection<Book> bookCollection;
        private readonly IMongoCollection<User> userCollection;
        public BookRepository(IConfiguration configuration)
        {
            var connString =
           configuration.GetConnectionString("MongoDBConnection");

            bookCollection = new MongoClient(connString)
            .GetDatabase("LibraryDB")
            .GetCollection<Book>("Book");

            userCollection = new MongoClient(connString)
            .GetDatabase("LibraryDB")
            .GetCollection<User>("User");

        }
        public Book Insert(Book book)
        {
            book.Id = ObjectId.GenerateNewId();
            bookCollection.InsertOne(book);
            return book;
        }
        public ICollection<Book> GetByUser(Guid userId)
        {
            return bookCollection
            .Find(x => x.GivenToUserId == userId)
            .ToList();
        }
        public Book GetById(ObjectId id)
        {
            return bookCollection
            .Find(x => x.Id == id)
            .FirstOrDefault();
        }
        public IReadOnlyCollection<Book> GetAll()
        {
            return bookCollection
            .Find(x => true)
            .ToList();
        }
        public void Delete(ObjectId bookId)
        {
            bookCollection.DeleteOne((x) => x.Id == bookId);
        }
        public Book GiveBookToUser(ObjectId bookId, ObjectId userId)
        {
            var book = GetById(bookId);
            if (book == null)
                throw new Exception("Book with this id does not exist");
            var user = userCollection.Find(x => x.Id == userId).FirstOrDefault();

            if (user == null)
                throw new Exception("User with this id does not exist");

            if (book.GivenToUserId != null && book.GivenToUserId.ToString() != "")
                throw new Exception("Book is already given to user number " + userId);

            var filter = Builders<Book>.Filter.Eq("id", bookId);
            var update = Builders<Book>.Update.Set("given_to_user_id", userId);

            var result = bookCollection.UpdateOne(filter, update);
            return book;
        }
        public Book RetrieveBookFromUser(ObjectId bookId)
        {
            var book = GetById(bookId);
            if (book == null)
                throw new Exception("Book with this id does not exist");

            var filter = Builders<Book>.Filter.Eq("id", bookId);
            var update = Builders<Book>.Update.Set("given_to_user_id", "");

            var result = bookCollection.UpdateOne(filter, update);
            return book;
        }
        public async void CreateIndexes()
        {
            await bookCollection.Indexes
            .CreateOneAsync(new CreateIndexModel<Book>(Builders<Book>.IndexKeys.Ascending(_ => _.Id)))
            .ConfigureAwait(false);
        }

    }
}
