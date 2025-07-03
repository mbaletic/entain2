
namespace entain2
{
    [TestClass]
    public sealed class Test1 : Base
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            var response = await client.GetPetByIdAsync(100);

            Console.WriteLine(response.Name);
        }
    }
}


