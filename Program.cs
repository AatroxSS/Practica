using Practica;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Prractica
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            DoublyLinkedList<Book> library = new DoublyLinkedList<Book>();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Додати книгу");
                Console.WriteLine("2. Видалити книгу з початку");
                Console.WriteLine("3. Вивести список книг");
                Console.WriteLine("4. Пошук наукової фантастики після 2010 року");
                Console.WriteLine("5. Сортувати за роком видання");
                Console.WriteLine("6. Зберегти список у файл");
                Console.WriteLine("7. Завантажити список з файлу");
                Console.WriteLine("8. Вийти");
                Console.WriteLine("9. Пошук книги за індексом");
                Console.Write("Виберіть опцію");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Невірний ввід.Спробуйте ще раз.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            AddBook(library);
                            break;
                        case 2:
                            Book removed = library.RemoveFromStart();
                            Console.WriteLine($"Видалено книгу: {removed}");
                            library.Printable("Оновлений список книг:");
                            break;
                        case 3:
                            library.Printable("Список книг:");
                                break;
                        case 4:
                            SearchScienceFiction(library);
                            break;
                        case 5:
                            library.Sort((a, b) => a.PublicationYear.CompareTo(b.PublicationYear));
                            library.Printable("Список книг відсортований за роком видання:");
                            break;
                        case 6:
                            library.Serialize("library.dat");
                            Console.WriteLine("Список збережено у файл library.dat");
                            break;
                        case 7:
                            library = DoublyLinkedList<Book>.Deserialize("library.dat");
                            Console.WriteLine("Список завантажено з файлу library.dat");
                            library.Printable("Завантажений список книг:");
                            break;
                        case 8:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                            break;
                        case 9:
                            Console.Write("Введіть індекс книги: ");
                            int index = int.Parse(Console.ReadLine());
                            try
                            {
                                Book book = library.GetByIndex(index);
                                Console.WriteLine($"Знайдена книга: {book}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Помилка: {ex.Message}");
                            }
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }

        static void AddBook(DoublyLinkedList<Book> library)
        {
            Console.WriteLine("Додавання нової книги:");

            Console.WriteLine("Доступні жанри:");
            foreach (Genre genre in Enum.GetValues(typeof(Genre)))
            {
                Console.WriteLine($"{(int)genre}. {genre}");
            }
            Console.WriteLine("Виберіть жанр (число): ");
            Genre genreChoice = (Genre)int.Parse(Console.ReadLine());

            Console.WriteLine("Рік видання: ");
            int year = int.Parse(Console.ReadLine());

            Console.WriteLine("Доступність (true/false): ");
            bool avaible = bool.Parse(Console.ReadLine());

            library.AddToEnd(new Book(genreChoice, year, avaible));
            library.Printable("Оновлений список кни: ");
        }

        static void SearchScienceFiction(DoublyLinkedList<Book> library)
        {
            var results = library.FindAll(book =>
            book.Genre == Genre.ScienceFiction &&
            book.PublicationYear > 2010 &&
            book.Available);

            Console.WriteLine("Результати пошуку (наукова фантастика після 2010 року,доступні):");
            Console.WriteLine(new string ('-',40));
            Console.WriteLine($"{ "Жанр",-15} { "Рік",-10} { "Доступність",-10}");
            Console.WriteLine(new string('-',40));

            foreach (var book in results)
            {
                Console.WriteLine(book.ToString());
            }

            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"Знайдено книг: {results.Count()}");
            Console.WriteLine();
        }
    }
}
