namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            Console.WriteLine(CountCopiesByAuthor(db));


        }


        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {

            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var titles = context.Books.Where(x => x.AgeRestriction == ageRestriction)
                      .Select(x =>  x.Title ).OrderBy(title => title).ToArray();


            var result = string.Join(Environment.NewLine, titles);




            return result;
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var bookType = Enum.Parse<EditionType>("Gold", true);

            var books = context.Books.Where(x => x.EditionType == bookType && x.Copies < 5000)
                                     .OrderBy(x => x.BookId).Select(x => x.Title);


            var result = string.Join(Environment.NewLine, books);


            return result;
        }

        public static string GetBooksByPrice(BookShopContext context)
        {

            var books = context.Books.Where(x => x.Price > 40).OrderByDescending(x => x.Price).Select(x => new { x.Title, x.Price }).ToList();

            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            
            var bookTitles = context.Books.Where(x => x.ReleaseDate.Value.Year != year).OrderBy(x => x.BookId).Select(x => x.Title).ToArray();


            var result = string.Join(Environment.NewLine, bookTitles);

            return result;
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            input = input.ToLower();
            var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var titles = context.Books.Where(x => x.BookCategories.Any(x => categories.Contains(x.Category.Name.ToLower())))
                                 .Select(x => x.Title).OrderBy(title => title).ToList();


            var result = string.Join(Environment.NewLine, titles);
            return result;
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {


            var dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", null);


            var books = context.Books.Where(x => x.ReleaseDate < dateTime).OrderByDescending(x => x.ReleaseDate)
                .Select(x => $"{x.Title} - {x.EditionType} - ${x.Price:F2}").ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorsNames = context.Authors.Where(x => x.FirstName.EndsWith(input)).OrderBy(x => x.FirstName).Select(x => $"{x.FirstName} {x.LastName}")
                .ToList();


            var result = string.Join(Environment.NewLine, authorsNames);

            return result;

        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {

            var booksTitles = context.Books.Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title).OrderBy(title => title).ToArray();

            var result = string.Join(Environment.NewLine, booksTitles);
            return result;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {

            var titlesAndAuthors = context.Books.Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(x => x.BookId).Select(x => $"{x.Title} ({x.Author.FirstName} {x.Author.LastName})").ToArray();
            var result = string.Join(Environment.NewLine, titlesAndAuthors);
            return result;
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {

            var booksCount = context.Books.Where(x => x.Title.Length > lengthCheck).Count();

            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors.Select(x => new
            {
                Copies = (x.Books.Select(x => x.Copies)).Sum(),
                name = $"{x.FirstName} {x.LastName}",
                

            }).OrderByDescending(x => x.Copies).ToList();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.name} - {author.Copies}");
            }


            
            return sb.ToString().TrimEnd();
        }

    }
}
