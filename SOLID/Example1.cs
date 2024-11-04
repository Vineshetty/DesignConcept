Let’s use a **Book Management System** as a scenario to apply all the **SOLID** principles with an example for each principle.

---

### Scenario: Book Management System

Suppose you’re building a library management system. This system allows librarians to manage books, check them in and out, and keep a record of books and borrowers.

---

### 1. **Single Responsibility Principle (SRP)**

Each class should have only one reason to change. In this system, a `Book` class should only handle book-related data, while another class might handle checkout functionality.

**Non-SRP Code:**

```csharp
public class Book {
    public string Title { get; set; }
    public string Author { get; set; }

    // Violates SRP: Handles checkout logic in the same class
    public void CheckOut(string memberName) {
        Console.WriteLine($"{Title} checked out to {memberName}");
    }
}
```

**SRP-Compliant Code:**

```csharp
public class Book {
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsCheckedOut { get; set; }
}

public class CheckoutService {
    public void CheckOut(Book book, string memberName) {
        if (!book.IsCheckedOut) {
            book.IsCheckedOut = true;
            Console.WriteLine($"{book.Title} checked out to {memberName}");
        }
    }
}
```

Now, the `Book` class only represents data, while `CheckoutService` handles checkout logic, ensuring each class has a single responsibility.

---

### 2. **Open/Closed Principle (OCP)**

Classes should be open for extension but closed for modification. We should be able to add new book types without changing existing code.

**Non-OCP Code:**

```csharp
public class BookManager {
    public void ProcessBook(Book book, string type) {
        if (type == "EBook") {
            Console.WriteLine("Processing eBook...");
        } else if (type == "AudioBook") {
            Console.WriteLine("Processing audiobook...");
        }
    }
}
```

**OCP-Compliant Code:**

```csharp
public interface IBookProcessor {
    void Process();
}

public class EBookProcessor : IBookProcessor {
    public void Process() {
        Console.WriteLine("Processing eBook...");
    }
}

public class AudioBookProcessor : IBookProcessor {
    public void Process() {
        Console.WriteLine("Processing audiobook...");
    }
}

public class BookManager {
    public void ProcessBook(IBookProcessor processor) {
        processor.Process();
    }
}
```

Now, we can add new types of books (e.g., `PrintedBookProcessor`) without modifying `BookManager`, making it OCP-compliant.

---

### 3. **Liskov Substitution Principle (LSP)**

Derived classes should be substitutable for their base classes. If we have a base `Book` class, subclasses like `EBook` and `AudioBook` should behave consistently with `Book` without introducing unexpected behaviors.

**Non-LSP Code:**

```csharp
public class Book {
    public virtual void DisplayContent() {
        Console.WriteLine("Displaying book content...");
    }
}

public class AudioBook : Book {
    public override void DisplayContent() {
        throw new NotImplementedException("AudioBook cannot display content as text.");
    }
}
```

**LSP-Compliant Code:**

Separate behaviors into different classes or interfaces to avoid non-functional overrides.

```csharp
public interface IReadableBook {
    void DisplayContent();
}

public interface IAudioPlayable {
    void PlayAudio();
}

public class TextBook : IReadableBook {
    public void DisplayContent() {
        Console.WriteLine("Displaying book content...");
    }
}

public class AudioBook : IAudioPlayable {
    public void PlayAudio() {
        Console.WriteLine("Playing audiobook...");
    }
}
```

This way, `TextBook` and `AudioBook` only implement behaviors they can perform, keeping them substitutable.

---

### 4. **Interface Segregation Principle (ISP)**

Classes should not be forced to implement interfaces they don’t use. In this system, we could split the functionality related to borrowers and librarians.

**Non-ISP Code:**

```csharp
public interface ILibraryUser {
    void BorrowBook();
    void ManageInventory();
}

public class Librarian : ILibraryUser {
    public void BorrowBook() {
        // Librarians typically don’t borrow books
    }
    
    public void ManageInventory() {
        Console.WriteLine("Managing inventory...");
    }
}
```

**ISP-Compliant Code:**

Split the interface into more focused ones:

```csharp
public interface IBorrower {
    void BorrowBook();
}

public interface IInventoryManager {
    void ManageInventory();
}

public class Librarian : IInventoryManager {
    public void ManageInventory() {
        Console.WriteLine("Managing inventory...");
    }
}

public class Member : IBorrower {
    public void BorrowBook() {
        Console.WriteLine("Borrowing a book...");
    }
}
```

Now, `Librarian` and `Member` only implement the methods they need.

---

### 5. **Dependency Inversion Principle (DIP)**

High-level modules should not depend on low-level modules but on abstractions. In this system, a `Library` class should not depend directly on a `BookDatabase`.

**Non-DIP Code:**

```csharp
public class BookDatabase {
    public void SaveBook(Book book) {
        Console.WriteLine("Saving book to database...");
    }
}

public class Library {
    private BookDatabase bookDatabase = new BookDatabase();
    
    public void AddBook(Book book) {
        bookDatabase.SaveBook(book);
    }
}
```

**DIP-Compliant Code:**

Define an abstraction (interface) and depend on it instead.

```csharp
public interface IBookRepository {
    void Save(Book book);
}

public class BookDatabase : IBookRepository {
    public void Save(Book book) {
        Console.WriteLine("Saving book to database...");
    }
}

public class Library {
    private IBookRepository bookRepository;
    
    public Library(IBookRepository bookRepo) {
        this.bookRepository = bookRepo;
    }

    public void AddBook(Book book) {
        bookRepository.Save(book);
    }
}
```

Now, `Library` depends on the `IBookRepository` abstraction rather than a concrete `BookDatabase` implementation, which can be swapped with other implementations, making the system more flexible.