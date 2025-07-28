using entain2.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace entain2.Tests.UserController
{
    /// <summary>
    /// Tests for user management endpoints of the API covering both standard workflows and known edge‑case bugs.
    /// Validates retrieving the default user by username and comparing all fields.
    /// Confirms successful login with valid credentials.
    /// Attempts logout to expose the unimplemented or faulty endpoint.
    /// Creates a new valid user and ensures the operation succeeds.
    /// Verifies that creating an empty user payload does not return 200 OK.
    /// Ensures that deleting a non‑existent user throws an error.
    /// </summary>
    [TestClass]
    [TestCategory("User operations")]
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
            defaultUser = JsonConvert.DeserializeObject<User>(defaultUserJson) ?? throw new Exception("Can't deserialize default localUser.");
        }

        [TestMethod]
        public async Task Should_ReturnDefaultUser_When_UsernameIsValid()
        {
            var remoteUser = await client.GetUserByNameAsync(defaultUser.Username);

            Assert.IsNotNull(remoteUser, "Remote user should not be null.");
            Assert.AreEqual(defaultUser.ToJson(), remoteUser.Result.ToJson(), "Default remote user values have been changed.");
        }

        [TestMethod]
        public async Task Should_LoginSuccessfully_When_CredentialsAreValid()
        {
            var response = await client.LoginUserAsync(defaultUser.Username, defaultUser.Password);

            Assert.IsNotNull(response.Result, "Login response result should not be null.");
            Assert.AreEqual(200, response.StatusCode, "Login should return status code 200.");
            StringAssert.Contains(response.Result.Message, "logged in", "Login response message should contain 'logged in'.");
        }

        /// <summary>
        /// Bug:
        /// What does this endpoint even do?
        /// Accepts no body, headers, always returns ok in the response
        /// Probably not finished or a bug
        /// </summary>
        [TestMethod]
        [TestCategory("Bugs")]
        public async Task Should_ReturnOk_When_LogoutIsCalled()
        {
            var response = await client.LogoutUserAsync();

            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode == 200);

            Assert.Fail("Endpoint is bugged or not implemented");
        }

        [TestMethod]
        public async Task Should_CreateUser_When_UserIsValid()
        {
            User localUser = UserHelper.CreateValidUser();
            var response = await client.CreateUserAsync(localUser);

            Assert.IsNotNull(response, "Create user response should not be null.");
            Assert.AreEqual(200, response.StatusCode, "Create user should return status code 200.");


        }
        /// <summary>
        /// Bug:
        /// Allows sending empty request body, ends in 200 OK nonetheless
        /// </summary>
        [TestMethod]
        [TestCategory("Bugs")]
        public async Task Should_ReturnError_When_CreatingEmptyUser()
        {
            User localUser = new();
            var response = await client.CreateUserAsync(localUser);

            Assert.IsNotNull(response, "Create user response should not be null.");
            Assert.AreNotEqual(200, response.StatusCode, "Endpoint allows creating with empty user request.");
        }

        /// <summary>
        /// Bug:
        /// We're passing a username which we just generated, which doesn't exist remotely
        /// Then trying to delete it
        /// </summary>
        [TestMethod]
        [TestCategory("Bugs")]
        public async Task Should_ThrowException_When_DeletingNonExistentUser()
        {
            User localUser = UserHelper.CreateValidUser();
            await Assert.ThrowsExceptionAsync<ApiException>(async () =>
            {
                await client.DeleteUserAsync(localUser.Username);
            }, "Endpoint allows deleting user which doesn't exist.");
        }
    }
}
