using Faker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace entain2.Utils
{
    public static class UserHelper
    {
        
        static readonly Random random = new();
        public static List<User> users = [];
        
        public static User CreateValidUser()
        {
            return new User()
            {
                Username = NameFaker.LastName() + random.Next(0,2000).ToString(),
                FirstName = NameFaker.FirstName(),
                LastName = NameFaker.LastName(),
                Email = InternetFaker.Email(),
                Id = random.Next(),
                Password = StringFaker.AlphaNumeric(12),
                Phone = StringFaker.AlphaNumeric(12),
                UserStatus = random.Next(0, 1)
            };
        }
    }
}
