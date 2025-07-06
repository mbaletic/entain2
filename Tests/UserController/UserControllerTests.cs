using entain2.Utils;
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
        [TestMethod]
        public async Task LoginTestUser()
        {
            var user = UserHelper.CreateValidUser();

            await client.CreateUserAsync(user);

        }
    }
}
