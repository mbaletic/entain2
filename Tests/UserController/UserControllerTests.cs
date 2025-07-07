using entain2.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entain2.Tests.UserController
{
    [TestClass]
    public sealed class UserControllerTests : Base
    {
        private User defaultUser;
        private string defaultUserJson = @"{
             ""id"": 1,
             ""username"": ""user1"",
             ""firstName"": ""first name 1"",
             ""lastName"": ""last name 1"",
             ""email"": ""email1@test.com"",
             ""password"": ""XXXXXXXXXXX"",
             ""phone"": ""123-456-7890"",
             ""userStatus"": 1
                }";


        public UserControllerTests()
        {
           defaultUser = JsonConvert.DeserializeObject<User>(defaultUserJson) ?? throw new Exception("Can't deserialize default user.");
        }

        [TestMethod]
        public async Task CheckDefaultUser()
        {
            var remoteUser = await client.GetUserByNameAsync(defaultUser.Username);

            Assert.IsNotNull(remoteUser);
            Assert.IsTrue(remoteUser.Result.ToJson() == defaultUser.ToJson(),"Default user values have been changed.");
        }

        [TestMethod]
        public async Task LoginDefaulUser()
        {
            var response = await client.LoginUserAsync(defaultUser.Username, defaultUser.Password);
            
            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == 200);
            Assert.IsTrue(response.Result.Contains("logged in user"));

        }
    }
}
