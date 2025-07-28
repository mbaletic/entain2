using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace entain2.Utils
{
    public static class PetHelper
    {
        public static ConcurrentBag<long> generatedPetIds = new();
        static readonly ThreadLocal<Random> random = new(() => new Random());
        public static string GenerateName()
        {

            String str = "abcdefghijklmnopqrstuvwxyz";
            int size = 10;

            String randomString = "";

            for (int i = 0; i < size; i++)
            {
                int x = random.Value.Next(26);
                randomString += str[x];
            }
            return randomString;
        }
        public static Pet CreateValidPet()
        {
            Pet pet = new()
            {
                Id = random.Value.Next(1000),
                Name = GenerateName(),
                Status = (PetStatus)random.Value.Next(0, 2),
                PhotoUrls = []
            };
            generatedPetIds.Add(pet.Id);
            return pet;
        }
    }
}
