using System.Security.Cryptography;

namespace Hashing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] strings = { "hello", "world", "foo", "bar" };

            // calculate the hash for each string and store in a dictionary
            var dict = new Dictionary<string, Tuple<byte[], string>>();
            foreach (var str in strings)
            {
                // generate a unique salt for each string
                byte[] salt = new byte[16];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // hash the string with the salt
                byte[] hash = HashString(str, salt);

                // store the salt and hash in the dictionary
                dict[str] = Tuple.Create(salt, Convert.ToBase64String(hash));
            }

            // retrieve a string using its hash
            string key = "world";
            if (dict.TryGetValue(key, out var value))
            {
                byte[] salt = value.Item1;
                string hashStr = value.Item2;
                byte[] hash = Convert.FromBase64String(hashStr);

                // hash the input string with the stored salt
                byte[] inputHash = HashString(key, salt);

                // compare the input hash with the stored hash in constant time
                if (CryptographicOperations.FixedTimeEquals(hash, inputHash))
                {
                    Console.WriteLine("Value for string {0}: {1}", key, value);
                }
                else
                {
                    Console.WriteLine("String {0} not found", key);
                }
            }
            else
            {
                Console.WriteLine("String {0} not found", key);
            }
        }

        static byte[] HashString(string str, byte[] salt)
        {
            using var sha256 = SHA256.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] saltedInputBytes = new byte[inputBytes.Length + salt.Length];
            Array.Copy(inputBytes, saltedInputBytes, inputBytes.Length);
            Array.Copy(salt, 0, saltedInputBytes, inputBytes.Length, salt.Length);
            return sha256.ComputeHash(saltedInputBytes);
        }
    }
}