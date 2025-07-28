using entain2.Utils;
using System.Net.Http.Json;

namespace entain2.Tests.PetController
{
    /// <summary>
    /// Here we try different negative scenarios
    /// Using raw JSON requests by omitting required parameters
    /// </summary>
    [TestClass]
    [TestCategory("Pet operations")]
    public sealed class PetControllerNegativeTests : Base
    {
        [TestMethod]
        public async Task Should_ThrowException_When_PetIdDoesNotExist()
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
        public async Task Should_Fail_When_CreatingPetWithoutName()
        {
            Pet petWithoutNameRequest = PetHelper.CreateValidPet();
            petWithoutNameRequest.Name = null;

            await client.AddPetAsync(petWithoutNameRequest);

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
            Pet petWithoutPhotoUrls = PetHelper.CreateValidPet();
            petWithoutPhotoUrls.PhotoUrls = null;
            await Assert.ThrowsExceptionAsync<ApiException>(async () =>
            {
                await client.AddPetAsync(petWithoutPhotoUrls);
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

            var pet = await response.Content.ReadFromJsonAsync<Pet>();
            if (pet == null)
            {
                Assert.Fail("Response deserialization failed");
                return;
            }

            PetHelper.generatedPetIds.Add(pet.Id);

            Assert.IsTrue((int)response.StatusCode != 200, "Endpoint allows empty request body");
        }

        [TestMethod]
        public async Task TryCreatePetNoBody()
        {
            var response = await RawJsonClient.PostPetAsync("");
            Assert.IsTrue((int)response.StatusCode == 405, "Endpoint allows sending no request body");
        }

        [TestMethod]
        public async Task TryCreateEmptyPet()
        {
            Pet emptyPet = new();
            await Assert.ThrowsExceptionAsync<ApiException>(async () =>
            {
                await client.AddPetAsync(emptyPet);
            },"Service allows creation of pet with empty pet object fields");
            
        }
    }
}





