﻿using entain2.Utils;
using System.Net.Http.Json;

namespace entain2.Tests
{
    /// <summary>
    /// Here we try different negative scenarios
    /// Using raw JSON requests by omitting required parameters
    /// </summary>
    [TestClass]
    [TestCategory("Regression")]
    [TestCategory("Negative")]
    public sealed class PetControllerNegativeTests : Base
    {
        [TestMethod]
        public async Task FindByNonExistantId()
        {
            int invalidID = -999;
            await Assert.ThrowsExceptionAsync<ApiException>(async () =>
            {
                await client.GetPetByIdAsync(invalidID);
            }
            ,
            $"Service either returned a pet which doesn't exist, or someone created a pet with test ID - {invalidID}.");
        }

        /// <summary>
        /// Bug:
        /// This will fail because every Pet should have a name according to Pet model
        /// but it passes 
        /// Then when we retrieve the list of pets, it returns the pet without name, causing deserialization error
        /// Probably bad implementation service-side
        /// </summary>
        [TestMethod]
        [TestCategory("Bugs")]
        public async Task CreatePetWithoutName()
        {
            var petId = 566678;
            var petWithoutNameRequest = @"{
        ""id"": 566678,
        ""photoUrls"": [""""],
        ""status"": ""available""
    }";

            PetGenerator.generatedPetIds.Add(petId);


            var response = await RawJsonClient.PostPetAsync(petWithoutNameRequest);
            await client.DeletePetAsync("", petId);


            Assert.Fail("Service allowed creating a pet without name being present in the request body.");

        }

        /// <summary>
        /// Bug:
        /// This will fail because every Pet should have a photo urls according to Pet model
        /// but it passes 
        /// Then when we retrieve the list of pets, it returns the pet without photo urls, causing deserialization error
        /// Probably bad implementation service-side
        /// </summary>

        [TestMethod]
        [TestCategory("Bugs")]
        public async Task CreatePetWithoutPhotoUrls()
        {
            var petWithoutPhotoUrls = @"{
            ""id"": 566679,
            ""name"": ""withoutPhotoUrls"",
            ""status"": ""available""
        }";

            PetGenerator.generatedPetIds.Add(566679);
            await Assert.ThrowsExceptionAsync<ApiException>(async () =>
            {
                await RawJsonClient.PostPetAsync(petWithoutPhotoUrls);
            }, "Service allows creating a pet without photo urls array being present in the request body.");

        }

        /// <summary>
        /// Bug:
        /// We're sending {} as JSON request body, which should result in a server side error or 400
        /// but it passes 
        /// With response
        /// {"id":13,"photoUrls":[],"tags":[]}
        /// </summary>
        [TestMethod]
        [TestCategory("Bugs")]
        public async Task TryCreatePetWithEmptyRequestBody()
        {
            var emptyBody = @"{
        }";

            var response = await RawJsonClient.PostPetAsync(emptyBody);
            await client.DeletePetAsync("", response.Content.ReadFromJsonAsync<Pet>().Result.Id);
            Assert.IsTrue((int)response.StatusCode != 200, "Endpoint allows empty request body");


        }

        [TestMethod]
        public async Task TryCreatePetNoBody()
        {

            var response = await RawJsonClient.PostPetAsync("");
            Assert.IsTrue((int)response.StatusCode == 405, "Endpoint allows sending no request body");

        }
    }
}





