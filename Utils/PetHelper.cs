using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entain2.Utils
{
    public static class PetHelper
    {
        public static List<long> generatedPetIds = [];
        static readonly Random random = new();
        public static string GenerateName()
        {

            String str = "abcdefghijklmnopqrstuvwxyz";
            int size = 10;

            String randomString = "";

            for (int i = 0; i < size; i++)
            {
                int x = random.Next(26);
                randomString += str[x];
            }
            return randomString;
        }
        public static Pet CreateValidPet()
        {
            Pet pet = new()
            {
                Id = random.Next(1000),
                Name = GenerateName(),
                Status = (PetStatus)random.Next(0, 2),
                PhotoUrls = []
            };
            generatedPetIds.Add(pet.Id);
            return pet;
        }
    }
}
