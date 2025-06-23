/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

namespace Core.Helpers;

public class RandomHelper
{
    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = chars[random.Next(chars.Length)];
        }

        return new string(result);
    }
}