using System;
using System.Security.Cryptography;
using System.IO;

public static class SignatureManager
{
    // Сделали ПУБЛИЧНЫМ, чтобы Program.cs мог видеть это имя файла
    public const string PublicKeyFileName = "public.xml";

    public static void SignFile(string filePath, string containerName, string sigPath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл для подписи не найден.");

            CspParameters cspParams = new CspParameters
            {
                ProviderType = 24,
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };

            using (var rsa = new RSACryptoServiceProvider(cspParams))
            {
                byte[] data = File.ReadAllBytes(filePath);
                byte[] signature = rsa.SignData(data, CryptoConfig.MapNameToOID("GOST3411"));
                File.WriteAllBytes(sigPath, signature);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("📝 Файл успешно подписан!");
                Console.WriteLine($"Подпись: {sigPath}");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Ошибка при подписи: {ex.Message}");
            Console.ResetColor();
        }
    }

    // Метод проверки теперь ищет файл сам, ему не нужно передавать ключ
    public static bool VerifyFile(string filePath, string sigPath)
    {
        try
        {
            if (!File.Exists(filePath) || !File.Exists(sigPath) || !File.Exists(PublicKeyFileName))
                throw new FileNotFoundException("Не найдены все необходимые файлы.");

            byte[] data = File.ReadAllBytes(filePath);
            byte[] signature = File.ReadAllBytes(sigPath);

            using (var rsa = new RSACryptoServiceProvider())
            {
                string publicKeyXml = File.ReadAllText(PublicKeyFileName);
                rsa.FromXmlString(publicKeyXml);

                bool isValid = rsa.VerifyData(data, CryptoConfig.MapNameToOID("GOST3411"), signature);
                return isValid;
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Ошибка при проверке: {ex.Message}");
            Console.ResetColor();
            return false;
        }
    }
}