using Faker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace entain2.Utils
{
    public static class UserHelper
    {
        
        static readonly ThreadLocal<Random> random = new(() => new Random());
        public static ConcurrentBag<User> users = new();
        
        public static User CreateValidUser()
        {
            return new User()
            {
                Username = NameFaker.LastName() + random.Value.Next(0,2000).ToString(),
                FirstName = NameFaker.FirstName(),
                LastName = NameFaker.LastName(),
                Email = InternetFaker.Email(),
                Id = random.Value.Next(),
                Password = StringFaker.AlphaNumeric(12),
                Phone = StringFaker.AlphaNumeric(12),
                UserStatus = random.Value.Next(0, 1)
            };
        }
    }
}
