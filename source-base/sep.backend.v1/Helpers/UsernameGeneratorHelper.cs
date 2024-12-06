using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Services.UnitOfWork;
using System.Globalization;
using System.Text;

namespace sep.backend.v1.Helpers
{
    public static class UsernameGeneratorHelper
    {
        public static async Task<string> GenerateUsernameAsync<TEntity>(
        IUnitOfWork unitOfWork,
        string firstName,
        string lastName
        ) where TEntity : BaseUserEntity
        {
            string lastNameInitials = string.Concat(lastName.Split(' ')
                                                 .Where(word => word.Length > 0)
                                                 .Select(word => word[0].ToString().ToLower()));

            string firstNameProcessed = RemoveDiacritics(firstName.ToLower());
            string initialsProcessed = RemoveDiacritics(lastNameInitials);
            string usernameBase = firstNameProcessed + initialsProcessed;

            var existingUsers = await unitOfWork.GetRepository<TEntity>().Where(
                u => u.Username.StartsWith(usernameBase)).ToListAsync();

            int maxSuffix = 0;

            foreach (var user in existingUsers)
            {
                string username = user.Username;
                if (username.Length > usernameBase.Length &&
                    int.TryParse(username.Substring(usernameBase.Length), out int suffix))
                {
                    maxSuffix = Math.Max(maxSuffix, suffix);
                }
            }

            int newSuffix = maxSuffix + 1;
            string newUsername = usernameBase + newSuffix;

            return newUsername;
        }

        private static string RemoveDiacritics(string text)
        {
            var replacements = new Dictionary<char, char>
            {
                {'Đ', 'd'}, {'đ', 'd'},
                {'À', 'a'}, {'Á', 'a'}, {'Ả', 'a'}, {'Ã', 'a'}, {'Ạ', 'a'},
                {'Â', 'a'}, {'Ầ', 'a'}, {'Ấ', 'a'}, {'Ẩ', 'a'}, {'Ẫ', 'a'}, {'Ậ', 'a'},
                {'Ă', 'a'}, {'Ằ', 'a'}, {'Ắ', 'a'}, {'Ẳ', 'a'}, {'Ẵ', 'a'}, {'Ặ', 'a'},
                {'à', 'a'}, {'á', 'a'}, {'ả', 'a'}, {'ã', 'a'}, {'ạ', 'a'},
                {'â', 'a'}, {'ầ', 'a'}, {'ấ', 'a'}, {'ẩ', 'a'}, {'ẫ', 'a'}, {'ậ', 'a'},
                {'ă', 'a'}, {'ằ', 'a'}, {'ắ', 'a'}, {'ẳ', 'a'}, {'ẵ', 'a'}, {'ặ', 'a'},
                {'È', 'e'}, {'É', 'e'}, {'Ẻ', 'e'}, {'Ẽ', 'e'}, {'Ẹ', 'e'},
                {'Ê', 'e'}, {'Ề', 'e'}, {'Ế', 'e'}, {'Ể', 'e'}, {'Ễ', 'e'}, {'Ệ', 'e'},
                {'è', 'e'}, {'é', 'e'}, {'ẻ', 'e'}, {'ẽ', 'e'}, {'ẹ', 'e'},
                {'ê', 'e'}, {'ề', 'e'}, {'ế', 'e'}, {'ể', 'e'}, {'ễ', 'e'}, {'ệ', 'e'},
                {'Ì', 'i'}, {'Í', 'i'}, {'Ỉ', 'i'}, {'Ĩ', 'i'}, {'Ị', 'i'},
                {'ì', 'i'}, {'í', 'i'}, {'ỉ', 'i'}, {'ĩ', 'i'}, {'ị', 'i'},
                {'Ò', 'o'}, {'Ó', 'o'}, {'Ỏ', 'o'}, {'Õ', 'o'}, {'Ọ', 'o'},
                {'Ô', 'o'}, {'Ồ', 'o'}, {'Ố', 'o'}, {'Ổ', 'o'}, {'Ỗ', 'o'}, {'Ộ', 'o'},
                {'Ơ', 'o'}, {'Ờ', 'o'}, {'Ớ', 'o'}, {'Ở', 'o'}, {'Ỡ', 'o'}, {'Ợ', 'o'},
                {'ò', 'o'}, {'ó', 'o'}, {'ỏ', 'o'}, {'õ', 'o'}, {'ọ', 'o'},
                {'ô', 'o'}, {'ồ', 'o'}, {'ố', 'o'}, {'ổ', 'o'}, {'ỗ', 'o'}, {'ộ', 'o'},
                {'ơ', 'o'}, {'ờ', 'o'}, {'ớ', 'o'}, {'ở', 'o'}, {'ỡ', 'o'}, {'ợ', 'o'},
                {'Ù', 'u'}, {'Ú', 'u'}, {'Ủ', 'u'}, {'Ũ', 'u'}, {'Ụ', 'u'},
                {'Ư', 'u'}, {'Ừ', 'u'}, {'Ứ', 'u'}, {'Ử', 'u'}, {'Ữ', 'u'}, {'Ự', 'u'},
                {'ù', 'u'}, {'ú', 'u'}, {'ủ', 'u'}, {'ũ', 'u'}, {'ụ', 'u'},
                {'ư', 'u'}, {'ừ', 'u'}, {'ứ', 'u'}, {'ử', 'u'}, {'ữ', 'u'}, {'ự', 'u'},
                {'Ý', 'y'}, {'ý', 'y'}, {'Ỷ', 'y'}, {'ỹ', 'y'}, {'ỵ', 'y'}
            };

            var stringBuilder = new StringBuilder();
            foreach (var c in text)
            {
                if (replacements.TryGetValue(c, out char replacement))
                {
                    stringBuilder.Append(replacement);
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
