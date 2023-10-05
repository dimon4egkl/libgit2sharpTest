using System.Diagnostics;
using System.Globalization;
using LibGit2Sharp;

//const string pathToRepository = "/home/dmytro/work/poc-git-library/test_git_library";
const string pathToRepository = "/app/test_git_library";

Stopwatch stopwatch = new();
stopwatch.Start();
using var repository = new Repository(pathToRepository);

RepositoryStatus repositoryStatus = repository.RetrieveStatus();
DateTime localDate = DateTimeOffset.Now.LocalDateTime;
var contentOfFile = $"content with timestamp {localDate}";
File.WriteAllText($"{pathToRepository}/test.txt", contentOfFile);

repository.Index.Add("test.txt");
repository.Index.WriteToTree(); // totally need this one


Signature author = new Signature("dmytro", "dimon4egkl55@gmail.com", localDate);
var commitMessage = $"another test commit {localDate}";
var newCommit = repository.Commit(commitMessage, author, author);
Console.WriteLine($"Committed with message: {commitMessage} and commitId: {newCommit.Id}");
var options = new PushOptions
{
    CredentialsProvider = (_,_,_) =>
        new UsernamePasswordCredentials()
        {
            Username = "dimon4egkl55@gmail.com",
            Password = "password"
        }
};
repository.Network.Push(repository.Branches["main"], options);
Console.WriteLine("Commit was pushed");
stopwatch.Stop();

IEnumerable<Commit> lastCommits = repository.Commits.Take(15);
Console.WriteLine("Dumping commits history...");
foreach (var commit in lastCommits)
{
    Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
        "{0} {1}",
        commit.Id.ToString(7),
        commit.MessageShort));
}

Console.WriteLine($"Execution took: {stopwatch.Elapsed.Milliseconds}");
