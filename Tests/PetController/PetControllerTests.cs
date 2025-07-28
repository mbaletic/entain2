using entain2.Utils;
using Newtonsoft.Json;

namespace entain2.Tests.PetController
{
    /// <summary>
    /// Tests written to check if the pet store management system - basic features are working.
    /// These are core features and must always work.
    /// Contains tests which will:
    /// Create a test pet and verify its creation
    /// Create a pet and attach an image to it
    /// Create a test pet and delete it, verify that it's deleted
    /// Create, update the test pet with new info and verify that the changes have been applied
    /// Find pets by all 3 statuses
    /// </summary>
    [TestClass]
    [TestCategory("Pet operations")]
    public sealed class PetControllerTests : Base
    {
        [TestMethod]
        public async Task Should_CreatePet_When_PetIsValid()
        {
            Pet localPet = PetHelper.CreateValidPet();
            localPet.Status = PetStatus.Available;

            await client.AddPetAsync(localPet);

        }

        [TestMethod]
        public async Task Should_CreateAndRetrievePet_When_PetIsValid()
        {
            Pet localPet = PetHelper.CreateValidPet();

            await client.AddPetAsync(localPet);

            var responsePet = await client.GetPetByIdAsync(localPet.Id);

            Assert.IsTrue(localPet.Name == responsePet.Result.Name, "Response pet name differs from the one we created.");
            Assert.IsTrue(localPet.Id == responsePet.Result.Id, "Response ID differs from the one we created.");
        }

        [TestMethod]
        public async Task Should_DeletePet_When_PetExists()
        {
            Pet localPet = PetHelper.CreateValidPet();

            await client.AddPetAsync(localPet);

            await client.DeletePetAsync(ConfigManager.Settings.ApiKey, localPet.Id);
        }

        [TestMethod]
        public async Task Should_UpdatePet_When_PetIsModified()
        {
            Pet localPet = PetHelper.CreateValidPet();
            await client.AddPetAsync(localPet);

            localPet.Name = "Updated pet";
            await client.UpdatePetAsync(localPet);

            var updatedPet = await client.GetPetByIdAsync(localPet.Id);
            Assert.IsTrue(localPet.Name == updatedPet.Result.Name, "Name of the remote pet didn't update after patching it's name.");

        }
        [TestMethod]
        public async Task Should_AttachImageToPet_When_ImageIsProvided()
        {
            var imagePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Images", "shiba.jpg");
            var fileStream = File.OpenRead(imagePath);
            var file = new FileParameter(fileStream, "shiba.jpg", "image/jpeg");

            Pet localPet = PetHelper.CreateValidPet();
            await client.AddPetAsync(localPet);

            await client.UploadFileAsync(localPet.Id, "photo", file);

            var remotePet = await client.GetPetByIdAsync(localPet.Id);
            fileStream.Close();
            Assert.IsNotNull(remotePet.Result.PhotoUrls);
        }

        [DataTestMethod]
        [DataRow(PetStatus.Available)]
        [DataRow(PetStatus.Pending)]
        [DataRow(PetStatus.Sold)]
        public async Task Should_ReturnPetsByStatus_When_TheyExist(PetStatus status)
        {
            var response = await client.FindPetsByStatusAsync([status]);
            var pets = response.Result;

            Assert.IsNotNull(pets.Count(), $"There are no pets with status {status}.");
            foreach (var pet in pets)
            {
                Assert.IsTrue(pet.Status == status,
                    $"There is a pet with wrong status - {pet.Status.Value} in the {status} pet status list.");
            }
        }

        [TestMethod]
        public async Task Should_ReturnPets_When_FindingByTags()
        {
            var response = await client.FindPetsByTagsAsync(["test"]);
            Assert.IsNotNull(response, "Find by tags operation returned nothing.");
        }

    }


}


