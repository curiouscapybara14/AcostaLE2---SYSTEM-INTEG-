using BlogDataLibrary.Models;

namespace BlogDataLibrary.Data;

public interface ISqlData
{
    UserModel? Authenticate(string username, string password);
    void Register(string username, string firstName, string lastName, string password);
    void AddPost(PostModel post);
    List<ListPostModel> ListPosts();
    ListPostModel? ShowPostDetails(int id);
}