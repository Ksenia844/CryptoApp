using System;
using System.IO;
using System.Security.Cryptography;

public static class KeyManager
{
    private const string ContainerFileName = "container_name.txt";
    private const string PublicKeyFileName = "public.xml";

    // Этот метод теперь просто возвращает имя контейнера (или null)
    public static string GenerateKeys()
    {
        try
        {
            string containerName = Guid.NewGuid().ToString();
            CspParameters cspParams = new CspParameters
            {
                ProviderType = 24,
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };

            using (var rsa = new RSACryptoServiceProvider(cspParams))
            {
                // Получаем ТОЛЬКО открытый ключ в формате XML
                string publicKeyXml = rsa.ToXmlString(false);
                File.WriteAllText(PublicKeyFileName, publicKeyXml);
                Console.WriteLine("✅ Открытый ключ сохранен в 'public.xml'");
            }

            File.WriteAllText(ContainerFileName, containerName);
            Console.WriteLine("✅ Контейнер ключей создан и сохранен.");
            return containerName; // Возвращаем имя, если все хорошо
        }
        catch (CryptographicException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Ошибка КриптоПро: {ex.Message}");
            Console.ResetColor();
            return null; // Возвращаем null, если ошибка
        }
    }

    // Этот метод просто читает файл и возвращает строку
    public static string LoadContainerName()
    {
        if (File.Exists(ContainerFileName))
        {
            string name = File.ReadAllText(ContainerFileName);
            Console.WriteLine("📂 Контейнер успешно загружен.");
            return name;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️ Файл контейнера не найден.");
            Console.ResetColor();
            return null;
        }
    }
}