using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entain2.Utils
{
    public static class PetGenerator
    {
        public static string GenerateName()
        {
            Random res = new Random();
            String str = "abcdefghijklmnopqrstuvwxyz";
            int size = 10;

            String randomString = "";

            for (int i = 0; i < size; i++)
            {
                int x = res.Next(26);
                randomString = randomString + str[x];
            }
            return randomString;
        }
        public static Pet CreateValidPet()
        {
            Random random = new Random();
            Pet pet = new Pet();
            pet.Id = random.Next(1000);
            pet.Name = GenerateName();
            return pet;
        }
    }
}
