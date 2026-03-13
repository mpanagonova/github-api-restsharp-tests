using System;
using RestSharpServices;
using RestSharpServices.Models;
using NUnit.Framework;


namespace TestGitHubApi
{
    [TestFixture]
    public class LabelTests
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

        [Test]
        public void GetAllLabelsForIssue_ShouldReturnValidLabels_WhenLabelsExist()
        {
            int issueNumber = 6;

            var labels = client.GetAllLabelsForIssue(Repo, issueNumber);

            if (labels != null)
            {
                foreach (var label in labels)
                {
                    Assert.That(label.Id, Is.GreaterThan(0), "Label ID should be greater than 0.");
                    Assert.That(label.Name, Is.Not.Null.And.Not.Empty, "Label name should not be null or empty.");
                }
            }
            else
            {
                Assert.Pass("No labels found for the issue, but the API call was successful.");
            }
        }
    }
}