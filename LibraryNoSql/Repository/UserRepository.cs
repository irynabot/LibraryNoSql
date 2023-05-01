using LibraryNoSql.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace LibraryNoSql.Repository
{
    public class UserRepository {
        private readonly IMongoCollection<User> collection;
        public UserRepository(IConfiguration configuration)
        {
            var connString =
           configuration.GetConnectionString("MongoDBConnection");
            collection = new MongoClient(connString)
            .GetDatabase("LibraryDB")
            .GetCollection<User>("User");
        }
        public User Insert(User user)
        {
            var existingUser = GetByLogin(user.Login);
            if (existingUser != null)
                throw new Exception("User with same login already exists");
    
            user.Id = Guid.NewGuid();
            user.Password = HashPassword(user.Password);
            collection.InsertOne(user);
            return user;
        }
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public IReadOnlyCollection<User> GetAll()
        {
            return collection
            .Find(x => true)
           .ToList();
        }
        public User GetById(Guid id)
        {
            return collection
            .Find(x => x.Id == id)
            .FirstOrDefault();
        }
        public User GetByLogin(string login)
        {
            return collection
            .Find(x => x.Login == login)
           .FirstOrDefault();
        }
        public User GetByLoginAndPassword(string login,
                                          string password)
        {
            return collection
            .Find(x => x.Login == login &&
            x.Password == HashPassword(password))
            .FirstOrDefault();
        }
        public async void CreateIndexes()
        {
            await collection.Indexes
            .CreateOneAsync(new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(_ => _.Id)))
            .ConfigureAwait(false);

            await collection.Indexes
            .CreateOneAsync(new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(_ => _.Login)))
            .ConfigureAwait(false);
        }
    }

}
