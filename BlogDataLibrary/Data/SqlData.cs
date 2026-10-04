using BlogDataLibrary.Database;
using BlogDataLibrary.Models;

namespace BlogDataLibrary.Data;

public class SqlData : ISqlData
{
    private readonly ISqlDataAccess _db;
    private const string connectionStringName = "SqlDb";

    public SqlData(ISqlDataAccess db)
    {
        _db = db;
    }

    public UserModel? Authenticate(string username, string password)
    {
        List<UserModel> result = _db.LoadData<UserModel, dynamic>(
            "SELECT * FROM dbo.Users WHERE UserName = @UserName AND Password = @Password",
            new { UserName = username, Password = password },
            connectionStringName,
            false);

        return result.FirstOrDefault();
    }

    public void Register(string username, string firstName, string lastName, string password)
    {
        _db.SaveData(
            "INSERT INTO dbo.Users (UserName, FirstName, LastName, Password) VALUES (@UserName, @FirstName, @LastName, @Password)",
            new { UserName = username, FirstName = firstName, LastName = lastName, Password = password },
            connectionStringName,
            false);
    }

    public void AddPost(PostModel post)
    {
        _db.SaveData(
            "INSERT INTO dbo.Posts (UserId, Title, Body, DateCreated) VALUES (@UserId, @Title, @Body, @DateCreated)",
            new { post.UserId, post.Title, post.Body, post.DateCreated },
            connectionStringName,
            false);
    }

    public List<ListPostModel> ListPosts()
    {
        string sql = @"SELECT p.Id, p.Title, p.Body, p.DateCreated, u.UserName, u.FirstName, u.LastName 
                       FROM dbo.Posts p 
                       INNER JOIN dbo.Users u ON p.UserId = u.Id";

        return _db.LoadData<ListPostModel, dynamic>(sql, new { }, connectionStringName, false);
    }

    public ListPostModel? ShowPostDetails(int id)
    {
        string sql = @"SELECT p.Id, p.Title, p.Body, p.DateCreated, u.UserName, u.FirstName, u.LastName 
                       FROM dbo.Posts p 
                       INNER JOIN dbo.Users u ON p.UserId = u.Id 
                       WHERE p.Id = @Id";

        List<ListPostModel> result = _db.LoadData<ListPostModel, dynamic>(sql, new { Id = id }, connectionStringName, false);
        return result.FirstOrDefault();
    }
}