
using entain2.Utils;

namespace entain2
{
    /// <summary>
    /// Contains tests which will:
    /// Create a testPet and verify its creation
    /// Update the testPet with new info and verify that the changes have been applied
    /// Find the testPet from a list of pets
    /// Delete the testPet
    /// </summary>
    [TestClass]
    public sealed class CrudTest : Base
    {

        [TestMethod]
        public async Task CreatePet()
        {
            Pet localPet = PetGenerator.CreateValidPet();
            Console.WriteLine(localPet.Id);

            await client.AddPetAsync(localPet);

            var responsePet = await client.GetPetByIdAsync(localPet.Id);

            Assert.IsTrue(localPet.Name == responsePet.Name, "Response pet name differs from the one we created.");
            Assert.IsTrue(localPet.Id == responsePet.Id, "Response ID differs from the one we created.");
        }
    }


}


