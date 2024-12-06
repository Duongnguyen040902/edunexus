using sep.backend.v1.Common.Const;

namespace sep.backend.v1.Common.Helpers
{
    public static class RandomCodeHelper
    {
        public static string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(Configs.MIN_RANDOM_CODE, Configs.MAX_RANDOM_CODE + 1).ToString();
        }
    }
}