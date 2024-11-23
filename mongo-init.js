//This would be the information shown in the online book store catalog

db = db.getSiblingDB('BookDetailsDb');

db.createCollection('Books');

db.Books.insertMany([
    {
        ISBN: "111-1-11-111111-1",
        Title: "The Pragmatic Programmer",
        Author: "Andrew Hunt and David Thomas",
        Description: "A practical guide for programmers.",
        Price: 39.99
    },
    {
        ISBN: "111-1-11-111111-2",
        Title: "Clean Code",
        Author: "Robert C. Martin",
        Description: "A handbook of agile software craftsmanship.",
        Price: 49.99
    },
    {
        ISBN: "111-1-11-111111-3",
        Title: "The Mythical Man-Month",
        Author: "Fred Brooks",
        Description: "Essays on software engineering.",
        Price: 29.99
    },
    {
        ISBN: "111-1-11-111111-4",
        Title: "Introduction to Algorithms",
        Author: "Thomas H. Cormen",
        Description: "A comprehensive book on algorithms.",
        Price: 89.99
    }
]);
