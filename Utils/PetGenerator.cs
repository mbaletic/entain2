using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entain2.Utils
{
    public static class PetGenerator
    {
        public static List<long> generatedPetIds = new List<long>();
        static Random random = new Random();
        public static string GenerateName()
        {

            String str = "abcdefghijklmnopqrstuvwxyz";
            int size = 10;

            String randomString = "";

            for (int i = 0; i < size; i++)
            {
                int x = random.Next(26);
                randomString = randomString + str[x];
            }
            return randomString;
        }
        public static Pet CreateValidPet()
        {
            Pet pet = new Pet();
            pet.Id = random.Next(1000);
            pet.Name = GenerateName();
            pet.Status = (PetStatus)random.Next(0, 2);
            generatedPetIds.Add(pet.Id);
            return pet;
        }
    }
}
