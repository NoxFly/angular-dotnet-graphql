/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Core.Helpers.Crypto;

class RSAHelper
{
    private static string PublicKey { get; set; } = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAuF8mFm6FemAGWRYu3HDx+txUlGf/aYP7AGK2YiqsM8ux/3WTyrsaLrzUKQve8LHzifv+cO7hR23auCphy4yhmYBMQ9dyx8EdYG5Ycs9+YcbdVd+UWKjk1M7VWc2hHAwT5WqDVjnHMBwEiRWfy+VXXG+VPbPkjT2syi1erM8Wue3lD3pNt2LZnM4QFaBM1mtrLwKLWwNLdgf+c+bHoICVc2Zix1wZLC/008g6vxl2L/eZ52HLZAR+gU4iwlsz236zFaI1o74vzZn8UEcXO1KssFc2pScB9GwvwTZfxnuPA28ShbVuzzTswqa+YjwXVIc8jS2RXhOFKkzOPUci7AdaCQIDAQAB";
    private static string PrivateKey = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC4XyYWboV6YAZZFi7ccPH63FSUZ/9pg/sAYrZiKqwzy7H/dZPKuxouvNQpC97wsfOJ+/5w7uFHbdq4KmHLjKGZgExD13LHwR1gblhyz35hxt1V35RYqOTUztVZzaEcDBPlaoNWOccwHASJFZ/L5Vdcb5U9s+SNPazKLV6szxa57eUPek23YtmczhAVoEzWa2svAotbA0t2B/5z5seggJVzZmLHXBksL/TTyDq/GXYv95nnYctkBH6BTiLCWzPbfrMVojWjvi/NmfxQRxc7UqywVzalJwH0bC/BNl/Ge48DbxKFtW7PNOzCpr5iPBdUhzyNLZFeE4UqTM49RyLsB1oJAgMBAAECggEARTYivCT9InVNrL+xE7uqevufHJ71uIx8+yYTRUKzpjX/OeDPfDlEmOr10frM1BpZTCE24Porw36vrpbqY1kFiPQqrQUB547NElgaB+9wiA/5IrVtoOK2FFUYrj+NDVRTW7niV8oUxjCeWz+s3wPXCbIxmhof4A2ZAcSKV+DdrSnHP/oVQbRg34yvxpyD1ttS1VkG+umZUmvN+feLXVEA66BtXKlwn8pNSU+NDjySaGSz+E6aZa5sAFQx3LRiWX+7KYLTBoY0D35Fy0NMn9hwGjVXyiM7mmNLx2ApXgnyJyLkHW3cCaETqwSE8qR99jPmDA7nD+FaMYWHCwg+ue4pCQKBgQDIYlHLFVuYYO9MGqXQRfJ6v4/T5Fb0j0VrX3whyp2Ld3vnUZSHcTRfl8xT9qRrlQWMge8rr4seR//1iTTNrpwoJ3pA005cZkdQCk4ZpaKYjWeXj9g7yWlJ8zfrvbG3TJ3gHCP3OmCxYkxsqFHGBIYjX85Zjea+9UaNFZiuRknwjwKBgQDrix4szywAaEk+RmVJGsBjPpBT5yGr1Qm0FsCgVRTxK59NL2h9You7UrOEE8GkJB9Fg45GVpXz/0gJCzeED8ThM3qNA56v0cT0jk0nF2TIqvxBACxwKKZ5Dv0Cca9DvAJXGXHm4rbVNm/WqCX04dn4cQUH6w8ODzvmdBZT036n5wKBgBDuBZ0T294+6ljSs4mqLzdhseQkGZil9iaBNbqhJLhgyu4bPWJIFTWyXIgHgZK5T3O0kqFtPmK7kIuNIRHzm6vk0fZiI++4kHPZuwl9rgQXhlYOyCUc+jFORD49k6M57Oi/V+aOXxb9iBlyCx6gyTh54rQ8Qe+GR4Sp+3FV7O9PAoGBAODDzjWgNDLnUQ02C6zo9DFAIGWQJ1dbYXkSRRo6VlOU9tymoqUThpdoYZllaOEC/zxMP1Xrhghh0BGhPIaurYbgos8xZxoLqFBeAk2NS0UZtMEqMz2L7N+UZ+cPKNoP7IPE6iYSrbljyvEhX6cAZglngA+ARMt7ygMtP6kaJsB9AoGBAJoHJ/9563wit7O/XkthLQPB5mHaaZaX/ZyOaEndCgRfzK9LEQXrDPequV0Aze51Z+/4SwdjuJNQusLRY9jA4sPFvNJuUxx+cJigEh7YvB1aSrKh9ZfZOi3CriTDtFia9mBHD7nu/DFNqnBV4usu6qEIfUOevuz/6E5/QRAq2hia";

    public static string Encrypt(string text)
    {
        using RSA rsa = RSA.Create();

        rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(PublicKey), out _);
        byte[] encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.OaepSHA256);

        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string encryptedText)
    {
        using RSA rsa = RSA.Create();

        rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(PrivateKey), out _);
        byte[] decryptedBytes = rsa.Decrypt(Convert.FromBase64String(encryptedText), RSAEncryptionPadding.OaepSHA256);

        return Encoding.UTF8.GetString(decryptedBytes);
    }

    public static void GenerateKeys()
    {
        using RSA rsa = RSA.Create(2048);

        string publicKey, privateKey;

        publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
        privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());

        Debug.WriteLine($"Public key: {publicKey}\n\n");
        Debug.WriteLine($"Private key: {privateKey}");

        PublicKey = publicKey;
        PrivateKey = privateKey;
    }
}
