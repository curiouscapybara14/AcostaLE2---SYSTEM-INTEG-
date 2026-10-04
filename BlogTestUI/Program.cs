using Microsoft.Extensions.Configuration;
using BlogDataLibrary.Data;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;

namespace BlogTestUI;

internal class Program
{
    static void Main(string[] args)
    {
        SqlData db = GetConnection();

        Console.WriteLine("=== BLOG TEST UI ===\n");

        // 1. Register a new user
        UserModel? registeredUser = Register(db);

        // 2. Authenticate the newly registered user
        if (registeredUser != null)
        {
            Authenticate(db, registeredUser.UserName, registeredUser.Password);

            // 3. Add a post using the newly registered user
            AddPost(db, registeredUser);
        }

        // 4. List all posts
        ListPosts(db);

        // 5. Show a specific post
        ShowPostDetails(db);

        Console.WriteLine("\nExecution finished successfully.");
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }

    static SqlData GetConnection()
    {
        var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json");

        IConfiguration config = builder.Build();
        ISqlDataAccess dbAccess = new SqlDataAccess(config);

        return new SqlData(dbAccess);
    }

    private static UserModel? Register(SqlData db)
    {
        Console.WriteLine("--- REGISTER NEW USER ---");

        Console.Write("Username: ");
        string username = Console.ReadLine() ?? "";

        Console.Write("First name: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Last name: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("\nRegistration failed: all fields are required.");
            return null;
        }

        // Check if username already exists
        UserModel? existingUser = db.Authenticate(username, password);

        if (existingUser != null)
        {
            Console.WriteLine("\nThat username already exists.");
            return existingUser;
        }

        db.Register(username, firstName, lastName, password);

        Console.WriteLine("\nUser registered successfully!");
        Console.WriteLine($"Name: {firstName} {lastName}");
        Console.WriteLine($"Username: {username}");

        return db.Authenticate(username, password);
    }

    private static void Authenticate(
        SqlData db,
        string username,
        string password)
    {
        Console.WriteLine("\n--- AUTHENTICATION ---");

        UserModel? user = db.Authenticate(username, password);

        if (user == null)
        {
            Console.WriteLine("Authentication failed.");
            return;
        }

        Console.WriteLine(
            $"Welcome, {user.FirstName} {user.LastName}!");
        Console.WriteLine(
            $"Logged in as: {user.UserName}");
    }

    private static void AddPost(
        SqlData db,
        UserModel user)
    {
        Console.WriteLine("\n--- ADD NEW POST ---");

        Console.Write("Post title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Post body: ");
        string body = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(title) ||
            string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Post was not added: title and body are required.");
            return;
        }

        PostModel post = new PostModel
        {
            Title = title,
            Body = body,
            DateCreated = DateTime.Now,
            UserId = user.Id
        };

        db.AddPost(post);

        Console.WriteLine("\nPost added successfully!");
        Console.WriteLine($"Author: {user.FirstName} {user.LastName}");
    }

    private static void ListPosts(SqlData db)
    {
        List<ListPostModel> posts = db.ListPosts();

        Console.WriteLine("\n--- ALL POSTS ---");

        if (posts.Count == 0)
        {
            Console.WriteLine("No posts found.");
            return;
        }

        foreach (var p in posts)
        {
            Console.WriteLine(
                $"ID {p.Id}: '{p.Title}' by " +
                $"{p.FirstName} {p.LastName} " +
                $"({p.UserName}) on {p.DateCreated}");
        }
    }

    private static void ShowPostDetails(SqlData db)
    {
        Console.WriteLine("\n--- SHOW POST DETAILS ---");

        Console.Write("Enter Post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid Post ID.");
            return;
        }

        ListPostModel? post = db.ShowPostDetails(postId);

        if (post == null)
        {
            Console.WriteLine($"No post found with ID {postId}.");
            return;
        }

        Console.WriteLine($"\n--- POST DETAILS (ID: {post.Id}) ---");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine(
            $"Author: {post.FirstName} {post.LastName} ({post.UserName})");
        Console.WriteLine($"Date: {post.DateCreated}");
        Console.WriteLine($"Body: {post.Body}");
    }
}