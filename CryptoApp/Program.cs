using System;
using System.IO;

namespace CryptoApp
{
    class Program
    {
        static string containerName;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ ===");
                Console.WriteLine("1. Сгенерировать ключи");
                Console.WriteLine("2. Загрузить ключи");
                Console.WriteLine("3. Подписать файл");
                Console.WriteLine("4. Проверить подпись");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": GenerateKeys(); break;
                    case "2": LoadKeys(); break;
                    case "3": SignAFile(); break;
                    case "4": VerifyAFile(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        PressEnter();
                        break;
                }
            }
        }

        private static void GenerateKeys()
        {
            Console.Clear();
            Console.WriteLine("🔐 Генерация новой ключевой пары...");

            // --- 2. СОХРАНЯЕМ РЕЗУЛЬТАТ В ГЛОБАЛЬНУЮ ПЕРЕМЕННУЮ ---
            containerName = KeyManager.GenerateKeys();

            if (containerName != null)
                Console.WriteLine($"Имя контейнера: {containerName}");

            PressEnter();
        }

        private static void LoadKeys()
        {
            Console.Clear();
            Console.WriteLine("📂 Загрузка ключей из файла...");

            // --- 3. СОХРАНЯЕМ РЕЗУЛЬТАТ В ГЛОБАЛЬНУЮ ПЕРЕМЕННУЮ ---
            containerName = KeyManager.LoadContainerName();

            if (containerName != null)
                Console.WriteLine($"Загружен контейнер: {containerName}");

            PressEnter();
        }

        private static void SignAFile()
        {
            // --- 4. ИСПОЛЬЗУЕМ ГЛОБАЛЬНУЮ ПЕРЕМЕННУЮ ---
            if (string.IsNullOrEmpty(containerName))
            {
                ShowError("Ошибка: Сначала нужно сгенерировать или загрузить ключи.");
                PressEnter();
                return;
            }

            Console.Clear();
            Console.Write("Введите путь к файлу для подписи: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                ShowError("Файл не найден.");
                PressEnter();
                return;
            }

            string sigPath = filePath + ".sig";

            // Передаем глобальную переменную в метод подписи
            SignatureManager.SignFile(filePath, containerName, sigPath);

            PressEnter();
        }

        private static void VerifyAFile()
        {
            // Проверяем наличие файла с открытым ключом через публичную константу
            if (!File.Exists(SignatureManager.PublicKeyFileName))
            {
                ShowError($"Ошибка: Файл с открытым ключом ({SignatureManager.PublicKeyFileName}) не найден.");
                PressEnter();
                return;
            }

            Console.Clear();
            Console.Write("Введите путь к исходному файлу: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                ShowError("Файл не найден.");
                PressEnter();
                return;
            }

            string sigPath = filePath + ".sig";

            if (!File.Exists(sigPath))
            {
                ShowError($"Файл подписи {sigPath} не найден.");
                PressEnter();
                return;
            }

            bool isValid = SignatureManager.VerifyFile(filePath, sigPath);

            if (isValid)
            {
                ShowSuccess("✅ Подпись ВЕРНА!");
            }
            else
            {
                ShowError("❌ Подпись НЕВЕРНА или файл был изменен!");
            }

            PressEnter();
        }

        // --- Вспомогательные методы ---
        private static void PressEnter()
        {
            Console.WriteLine("\nНажмите Enter для возврата в меню...");
            Console.ReadLine();
        }

        private static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}