using System;
using RestSharpServices;
using RestSharpServices.Models;
using NUnit.Framework;

namespace TestGitHubApi
{
    [TestFixture]
    public class IssueTests
    {
        private GitHubApiClient client;
        private const string Repo = "test-nakov-repo";

        [SetUp]
        public void Setup()
        {
            string username = Environment.GetEnvironmentVariable("GITHUB_USERNAME") ?? "your-username";
            string token = Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? "your-username";

            client = new GitHubApiClient(
                "https://api.github.com/repos/testnakov/",
                username,
                token);
        }

        [Test]
        public void GetAllIssues_ShouldReturnIssues_WhenRepositoryExists()
        {
            var issues = client.GetAllIssues(Repo);

            Assert.That(issues, Is.Not.Null, "Issues collection should not be null.");
            Assert.That(issues, Is.Not.Empty, "Issues collection should not be empty.");

            foreach (var issue in issues!)
            {
                Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
                Assert.That(issue.Number, Is.GreaterThan(0), "Issue number should be greater than 0.");
                Assert.That(issue.Title, Is.Not.Null.And.Not.Empty, "Issue title should not be null or empty.");
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(100)]
        public void GetIssueByNumber_ShouldReturnCorrectIssue_WhenIssueExists(int issueNumber)
        {
            var issue = client.GetIssueByNumber(Repo, issueNumber);

            Assert.That(issue, Is.Not.Null, "Issue should not be null.");
            Assert.That(issue!.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
            Assert.That(issue.Number, Is.EqualTo(issueNumber), $"Issue number should be {issueNumber}.");
        }

        [Test]
        public void CreateIssue_ShouldReturnCreatedIssue_WhenValidDataIsProvided()
        {
            string expectedTitle = $"Test Issue {Guid.NewGuid()}";
            string expectedBody = "This is a portfolio test issue created by RestSharp and NUnit.";

            var createdIssue = client.CreateIssue(Repo, expectedTitle, expectedBody);

            Assert.That(createdIssue, Is.Not.Null, "Created issue should not be null.");

            Assert.Multiple(() =>
            {
                Assert.That(createdIssue!.Id, Is.GreaterThan(0), "Created issue ID should be greater than 0.");
                Assert.That(createdIssue.Number, Is.GreaterThan(0), "Created issue number should be greater than 0.");
                Assert.That(createdIssue.Title, Is.EqualTo(expectedTitle), "Created issue title should match.");
                Assert.That(createdIssue.Body, Is.EqualTo(expectedBody), "Created issue body should match.");
            });
        }
    }
}
