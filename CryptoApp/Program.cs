using System;
using System.Security.Cryptography;

namespace CryptoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Финальная проверка ===");
            Console.WriteLine("Пытаемся создать алгоритмы через стандартные методы .NET...");

            // --- Проверка хэширования ---
            // Используем стандартный метод .NET. КриптоПро перехватит этот вызов.
            using (HashAlgorithm gostHash = HashAlgorithm.Create("GOST3411"))
            {
                if (gostHash != null)
                {
                    Console.WriteLine("УСПЕХ: Алгоритм хэширования ГОСТ Р 34.11-2012 найден!");
                    Console.WriteLine("Полное имя объекта: " + gostHash.ToString());
                }
                else
                {
                    Console.WriteLine("ОШИБКА: Алгоритм хэширования не найден.");
                }
            }

            // --- Проверка подписи ---
            // Используем стандартный метод .NET. КриптоПро перехватит этот вызов.
            using (AsymmetricAlgorithm gostSign = AsymmetricAlgorithm.Create("GOST3410"))
            {
                if (gostSign != null)
                {
                    Console.WriteLine("УСПЕХ: Алгоритм подписи ГОСТ Р 34.10-2012 найден!");
                    Console.WriteLine("Полное имя объекта: " + gostSign.ToString());
                }
                else
                {
                    Console.WriteLine("ОШИБКА: Алгоритм подписи не найден.");
                }
            }

            Console.WriteLine("\nНажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}
