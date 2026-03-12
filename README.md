# GitHub API Tests (RestSharp + NUnit)

This project demonstrates **API test automation in C#** using **RestSharp** and **NUnit**.

The tests interact with the **GitHub REST API** to validate issue-related functionality.

## Tech Stack

* C#
* .NET
* RestSharp
* NUnit
* GitHub REST API

## What Is Tested

* Get issue by number
* Create new issue
* Get labels for an issue
* Get comments for an issue
* Validate API response data

## Running the Tests

Set the required environment variables:

```
GITHUB_USERNAME
GITHUB_TOKEN
```

Run the tests with:

```
dotnet test
```

## Purpose

This project was created to practice **API testing and test automation using C#**.
