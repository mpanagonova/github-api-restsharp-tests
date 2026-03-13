using System;
using RestSharpServices;
using RestSharpServices.Models;
using NUnit.Framework;


namespace TestGitHubApi
{
    [TestFixture]
    public class CommentTests
    {
        private GitHubApiClient client;
        private const string Repo = "test-nakov-repo";

        [SetUp]
        public void Setup()
        {
            string username = Environment.GetEnvironmentVariable("GITHUB_USERNAME") ?? "your-username";
            string token = Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? "YOUR_GITHUB_TOKEN";

            client = new GitHubApiClient(
                "https://api.github.com/repos/testnakov/",
                username,
                token);
        }

        private Issue CreateTestIssue()
        {
            string title = $"Temp Issue {Guid.NewGuid()}";
            string body = "Temporary test issue for comment-related tests.";

            var issue = client.CreateIssue(Repo, title, body);

            Assert.That(issue, Is.Not.Null, "Test issue should be created successfully.");
            return issue!;
        }

        private Comment CreateTestComment(int issueNumber, string body = "Temporary test comment.")
        {
            var comment = client.CreateCommentOnGitHubIssue(Repo, issueNumber, body);

            Assert.That(comment, Is.Not.Null, "Test comment should be created successfully.");
            return comment!;
        }

        [Test]
        public void GetAllCommentsForIssue_ShouldReturnComments_WhenCommentsExist()
        {
            int issueNumber = 6;

            var comments = client.GetAllCommentsForIssue(Repo, issueNumber);

            Assert.That(comments, Is.Not.Null, "Comments collection should not be null.");
            Assert.That(comments, Is.Not.Empty, "Comments collection should not be empty.");

            foreach (var comment in comments!)
            {
                Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID should be greater than 0.");
                Assert.That(comment.Body, Is.Not.Null.And.Not.Empty, "Comment body should not be null or empty.");
            }
        }

        [Test]
        public void CreateComment_ShouldReturnCreatedComment_WhenValidDataIsProvided()
        {
            var issue = CreateTestIssue();
            string expectedBody = "This is a test comment created by the GitHub API client.";

            var comment = client.CreateCommentOnGitHubIssue(Repo, issue.Number, expectedBody);

            Assert.That(comment, Is.Not.Null, "Created comment should not be null.");
            Assert.That(comment!.Id, Is.GreaterThan(0), "Comment ID should be greater than 0.");
            Assert.That(comment.Body, Is.EqualTo(expectedBody), "Comment body should match.");
        }

        [Test]
        public void GetCommentById_ShouldReturnCorrectComment_WhenCommentExists()
        {
            var issue = CreateTestIssue();
            var createdComment = CreateTestComment(issue.Number, "Comment for GetById test.");

            var comment = client.GetCommentById(Repo, createdComment.Id);

            Assert.That(comment, Is.Not.Null, "Comment should not be null.");
            Assert.That(comment!.Id, Is.EqualTo(createdComment.Id), "Comment ID should match.");
            Assert.That(comment.Body, Is.EqualTo("Comment for GetById test."), "Comment body should match.");
        }

        [Test]
        public void EditComment_ShouldReturnUpdatedComment_WhenCommentExists()
        {
            var issue = CreateTestIssue();
            var createdComment = CreateTestComment(issue.Number, "Original comment body.");
            string newBody = "Edited comment body.";

            var editedComment = client.EditCommentOnGitHubIssue(Repo, createdComment.Id, newBody);

            Assert.That(editedComment, Is.Not.Null, "Edited comment should not be null.");
            Assert.That(editedComment!.Id, Is.EqualTo(createdComment.Id), "Edited comment ID should match.");
            Assert.That(editedComment.Body, Is.EqualTo(newBody), "Edited comment body should be updated.");
        }

        [Test]
        public void DeleteComment_ShouldReturnTrue_WhenCommentExists()
        {
            var issue = CreateTestIssue();
            var createdComment = CreateTestComment(issue.Number, "Comment to be deleted.");

            bool isDeleted = client.DeleteCommentOnGitHubIssue(Repo, createdComment.Id);

            Assert.That(isDeleted, Is.True, "Comment should be deleted successfully.");
        }
    }
}