# slim-down-your-aggregate

## Business Rules:

1. A book's title cannot be empty or null.
2. A book's author cannot be empty or null.
3. A book's genre cannot be empty or null.
4. A book has a list of reviewers.
5. The book must pass the committee approval before moving to printing.
6. A book can be moved from one state to another only in the order of Writing, Editing, Printing, Published, OutOfPrint.
7. A book cannot be moved to the Printing state if it is not approved by the committee.
8. A book cannot be moved to the Published state if it does not have at least five translations.
9. A book cannot move to the OutOfPrint state if more than 10% of total copies across all formats are unsold.
10. A book cannot have more than five translations.
11. Each format of a book should be unique. A format can be added only if it does not already exist in the list of formats.
12. A format can be removed only if it exists in the list of formats.