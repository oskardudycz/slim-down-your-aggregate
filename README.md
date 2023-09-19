# Slim Down Your Aggregate

Let's slim down some complex aggregate implementation. It's an aggregate responsible for managing the book writing, editing and publishing process.
See the original implementation in the:

- [C#](./csharp/Original/PublishingHouse.Domain/Books/Book.cs)
- [Java](https://github.com/oskardudycz/slim-down-your-aggregate/blob/main/java/src/main/java/io/eventdriven/slimdownaggregates/original/Book.java)
- [TypeScript](./java/src/main/java/io/eventdriven/slimdownaggregates/original/Book.java)

We'll be doing multiple transitions step by step to make it more focused on the business logic and make it smaller but more precise.

We'll be working on the copy of original version located in the slimmed folder

## Prerequisities

### C#

1. Clone this repository.
2. Install .NET 7 - https://dotnet.microsoft.com/en-us/download/dotnet/7.0.
3. Install Rider, Visual Studio, VSCode or other preferred IDE.
4. Open [SlimDownAggregates.sln](./csharp/SlimDownAggregates.sln) solution.
5. Run build.
6. To run integration tests you either need to run `docker-compose up` to setup Postgres container. Or set `TEST_IN_MEMORY` environment variable to true if you'd like to run them in memory.

### Java

1. Clone this repository.
2. Install Java JDK 17 (or later) - https://www.oracle.com/java/technologies/downloads/.
3. Install IntelliJ, Eclipse, VSCode or other preferred IDE.
4. Open [java](./java/) folder as project.

### TypeScript

1. Clone this repository.
2. Install Node.js 18 (or later) - https://node.js.org/en/download/ (Or better using NVM).
3. Install VSCode, WebStorm or other preferred IDE.
4. Open [node.js](./node.js/) folder as project.
5. Run `npm run build` to verify that code is compiling.
6. Run `npm run test` to verify if all is working.

## Business Rules:

1. A book's title cannot be empty or null.
2. A book's author cannot be empty or null.
3. A book's genre cannot be empty or null.
4. A book has a list of reviewers.
5. The book must pass the committee's approval before moving to printing.
6. A book can be moved from one state to another only in the order of Writing, Editing, Printing, Published, OutOfPrint.
7. A book cannot be moved to the Printing state if the committee does not approve it.
8. A book can only be moved to the Published state if it has at least five translations.
9. A book cannot move to the OutOfPrint state if more than 10% of total copies across all formats are unsold.
10. A book can have up to five translations.
11. Each format of a book should be unique. A format can be added only if it does not already exist in the list of formats.
12. A format can be removed only if it exists in the list of formats.
