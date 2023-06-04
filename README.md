# Slim Down Your Aggregate

Let's slim down some complex aggregate implementation. It's an aggregate responsible for managing the book writing, editing and publishing process. 
See the original implementation in the:
- [C#](https://github.com/oskardudycz/slim-down-your-aggregate/blob/main/csharp/SlimDownYourAggregates.Tests/Original/Book.cs)
- [Java](https://github.com/oskardudycz/slim-down-your-aggregate/blob/main/java/src/main/java/io/eventdriven/slimdownaggregates/original/Book.java)

We'll be doing multiple transitions step by step to make it more focused on the business logic and make it smaller but more precise.

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

## Prerequisities

### C#

1. Clone this repository.
2. Install .NET 7 - https://dotnet.microsoft.com/en-us/download/dotnet/7.0.
3. Install Rider, Visual Sturdio, VSCode or other preferred IDE.
4. Open [csharp](./csharp/) folder as project.

### Java

1. Clone this repository.
2. Install Java JDK 17 (or later) - https://www.oracle.com/java/technologies/downloads/.
3. Install IntelliJ, Eclipse, VSCode or other preferred IDE.
4. Open [java](./java/) folder as project.
## Steps

1. Find the data that is used in IF statements or passed to events.
2. Copy the Book aggregate and rename it into BookModel, this will be our storage model, we still need to keep the backward compatibility.
3. Remove all methods from it, just keep state, we'll use it as a reference and need that for later.
4. Remove all the data that we don't need for IFs from Book aggregate.
5. Add missing events.
6. Group them in the same file to make it more readable (as they're form of documentation) and make managing it less tedious.