using entain2.Utils;

namespace entain2.Tests
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
    [TestCategory("Regression")]
    [TestCategory("Positive")]
    public sealed class PetControllerTests : Base
    {
        [TestMethod]
        public async Task CreatePet()
        {
            Pet localPet = PetGenerator.CreateValidPet();
            localPet.Status = PetStatus.Available;

            await client.AddPetAsync(localPet);

        }

        [TestMethod]
        public async Task CreatePetAndVerify()
        {
            Pet localPet = PetGenerator.CreateValidPet();

            await client.AddPetAsync(localPet);

            var responsePet = await client.GetPetByIdAsync(localPet.Id);

            Assert.IsTrue(localPet.Name == responsePet.Name, "Response pet name differs from the one we created.");
            Assert.IsTrue(localPet.Id == responsePet.Id, "Response ID differs from the one we created.");
        }

        [TestMethod]
        public async Task CreatePetAndDelete()
        {
            Pet localPet = PetGenerator.CreateValidPet();

            await client.AddPetAsync(localPet);

            await client.DeletePetAsync(ConfigManager.Settings.ApiKey, localPet.Id);
        }

        [TestMethod]
        public async Task CreatePetAndModify()
        {
            Pet localPet = PetGenerator.CreateValidPet();
            await client.AddPetAsync(localPet);

            localPet.Name = "Updated pet";
            await client.UpdatePetAsync(localPet);

            var updatedPet = await client.GetPetByIdAsync(localPet.Id);
            Assert.IsTrue(localPet.Name == updatedPet.Name, "Name of the remote pet didn't update after patching it's name.");

        }
        [TestMethod]
        public async Task CreatePetAndAttachImage()
        {
            var imagePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Images", "shiba.jpg");
            var fileStream = File.OpenRead(imagePath);
            var file = new FileParameter(fileStream, "shiba.jpg", "image/jpeg");

            Pet localPet = PetGenerator.CreateValidPet();
            await client.AddPetAsync(localPet);

            await client.UploadFileAsync(localPet.Id, "photo", file);

            var remotePet = await client.GetPetByIdAsync(localPet.Id);
            fileStream.Close();
            Assert.IsNotNull(remotePet.PhotoUrls);
        }

        [TestMethod]
        public async Task CheckIfThereAreAvailablePets()
        {
            IEnumerable<Pet> availablePets;
            availablePets = await client.FindPetsByStatusAsync([PetStatus.Available]);

            Assert.IsNotNull(availablePets.Count(), "There are no available pets.");
            foreach (var pet in availablePets)
            {
                Assert.IsTrue(pet.Status == PetStatus.Available,
                    $"There is a pet with wrong status - {pet.Status.Value} in the available pet status list.");
            }
        }

        [TestMethod]
        public async Task CheckIfThereArePendingPets()
        {
            IEnumerable<Pet> pendingPets;
            pendingPets = await client.FindPetsByStatusAsync([PetStatus.Pending]);

            Assert.IsNotNull(pendingPets.Count(), "There are no pending pets.");
            foreach (var pet in pendingPets)
            {
                Assert.IsTrue(pet.Status == PetStatus.Pending,
                    $"There is a pet with wrong status - {pet.Status.Value} in the pending pet status list.");
            }

        }

        [TestMethod]
        public async Task CheckIfThereAreSoldPets()
        {
            IEnumerable<Pet> soldPets;
            soldPets = await client.FindPetsByStatusAsync([PetStatus.Sold]);

            Assert.IsNotNull(soldPets.Count(), "There are no sold pets.");
            foreach (var pet in soldPets)
            {
                Assert.IsTrue(pet.Status == PetStatus.Sold,
                    $"There is a pet with wrong status - {pet.Status.Value} in the sold pet status list.");
            }
        }

        [TestMethod]
        public async Task FindByTags()
        {
            var response = await client.FindPetsByTagsAsync(["test"]);
            Assert.IsNotNull(response, "Find by tags operation returned nothing.");
        }

    }


}


