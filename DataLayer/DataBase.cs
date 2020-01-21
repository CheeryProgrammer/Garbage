using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataBase : IStorage
    {
        private static List<User> Users = new List<User>();

        public DataBase()
        {
            Users.Add(new User { Name = "Ivan", LastName = "Petrov" });
            Users.Add(new User { Name = "Petr", LastName = "Ivanov" });
            Users.Add(new User { Name = "Ivan", LastName = "Petrov" });
            Users.Add(new User { Name = "Ivan", LastName = "Petrov" });
        }

        public IEnumerable<User> GetUsers()
        {
            return Users;
        }
    }
    public class StorageMock : IStorage
    {
        private static List<User> Users = new List<User>();

        public StorageMock()
        {
            Users.Add(new User { Name = "Ivan", LastName = "Petrov" });
            Users.Add(new User { Name = "Petr", LastName = "Ivanov" });
        }

        public IEnumerable<User> GetUsers()
        {
            return Users;
        }
    }
}
