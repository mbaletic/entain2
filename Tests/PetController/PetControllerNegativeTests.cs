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

        [DataTestMethod]
        [DataRow(InvalidPetType.NoName)]
        [DataRow(InvalidPetType.NoPhotoUrls)]
        [DataRow(InvalidPetType.EmptyPet)]
        public async Task Should_ThrowException_When_CreatingInvalidPet(InvalidPetType invalidType)
        {
            Pet pet = null;
            switch (invalidType)
            {
                case InvalidPetType.NoName:
                    pet = PetHelper.CreateValidPet();
                    pet.Name = null;
                    break;
                case InvalidPetType.NoPhotoUrls:
                    pet = PetHelper.CreateValidPet();
                    pet.PhotoUrls = null;
                    break;
                case InvalidPetType.EmptyPet:
                    pet = new Pet();
                    break;
            }
            await Assert.ThrowsExceptionAsync<ApiException>(async () =>
            {
                await client.AddPetAsync(pet);
            }, $"Service allows creation of invalid pet: {invalidType}");
        }

        /// <summary>
        /// Bug:
        /// We're sending {} as JSON request body, which should result in a server side error or 400
        /// but it passes 
        /// With response
        /// {"id":13,"photoUrls":[],"tags":[]}
        /// </summary>
        [DataTestMethod]
        [DataRow(@"{
    }")]
        [DataRow("")]
        public async Task Should_ReturnError_When_CreatingPetWithInvalidBody(string requestBody)
        {
            var response = await RawJsonClient.PostPetAsync(requestBody);
            if (!string.IsNullOrEmpty(requestBody))
            {
                var pet = await response.Content.ReadFromJsonAsync<Pet>();
                if (pet == null)
                {
                    Assert.Fail("Response deserialization failed");
                    return;
                }
                PetHelper.generatedPetIds.Add(pet.Id);
                Assert.AreNotEqual(200, (int)response.StatusCode, "Endpoint allows empty request body");
            }
            else
            {
                Assert.AreEqual(405, (int)response.StatusCode, "Endpoint allows sending no request body");
            }
        }
    }
}





