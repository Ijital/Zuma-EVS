namespace ZumaService.Helpers
{
    using System.Security.Cryptography;
    using System.Text;

    public static class HashHelper
    {
        /// <summary>
        /// Gets hash of specified value
        /// </summary>
        public static string GetHash(string value)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            var builder = new StringBuilder();
            bytes.ToList().ForEach(bit => builder.Append(bit.ToString("x2")));
            return builder.ToString();
        }
    }
}
