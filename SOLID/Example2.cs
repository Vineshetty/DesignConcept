Here's an example scenario in an e-commerce system demonstrating each of the SOLID principles:

---

### Scenario: E-Commerce Order Processing System

Imagine we’re building an e-commerce application where users can place orders, receive invoices, and track their orders. Here’s how we’d apply each SOLID principle.

---

#### 1. **Single Responsibility Principle (SRP)**

Each class should have one job or responsibility. For example:

- **OrderProcessor**: Handles the logic for processing orders.
- **InvoiceGenerator**: Responsible for creating invoices.
- **NotificationService**: Sends notifications (email, SMS) to customers.

Instead of having an `OrderService` class that handles all these tasks, we divide the responsibilities into smaller, focused classes.

```csharp
public class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        // Logic to process the order
    }
}

public class InvoiceGenerator
{
    public Invoice GenerateInvoice(Order order)
    {
        // Logic to create an invoice for the order
        return new Invoice();
    }
}

public class NotificationService
{
    public void SendOrderNotification(Customer customer, Order order)
    {
        // Logic to send notification
    }
}
```

---

#### 2. **Open-Closed Principle (OCP)**

Classes should be open for extension but closed for modification. Suppose we want to apply discounts to orders. We create an `IDiscount` interface so that we can add new discount types without changing the `OrderProcessor`.

```csharp
public interface IDiscount
{
    decimal ApplyDiscount(decimal orderTotal);
}

public class SeasonalDiscount : IDiscount
{
    public decimal ApplyDiscount(decimal orderTotal)
    {
        return orderTotal * 0.90m; // 10% off
    }
}

public class OrderProcessor
{
    private IDiscount _discount;

    public OrderProcessor(IDiscount discount)
    {
        _discount = discount;
    }

    public void ProcessOrder(Order order)
    {
        var total = order.TotalAmount;
        total = _discount.ApplyDiscount(total);
        // Further order processing
    }
}
```

By injecting different `IDiscount` implementations, we can add new discount types without changing `OrderProcessor`.

---

#### 3. **Liskov Substitution Principle (LSP)**

Objects of a superclass should be replaceable with objects of a subclass without breaking the application. For example, if we have a `BaseNotification` class with `EmailNotification` and `SmsNotification` subclasses, both should behave in a way that doesn’t break functionality.

```csharp
public abstract class BaseNotification
{
    public abstract void Send(string message);
}

public class EmailNotification : BaseNotification
{
    public override void Send(string message)
    {
        // Send email
    }
}

public class SmsNotification : BaseNotification
{
    public override void Send(string message)
    {
        // Send SMS
    }
}

public class NotificationService
{
    public void Notify(BaseNotification notification, string message)
    {
        notification.Send(message); // Works with any subclass of BaseNotification
    }
}
```

In this example, `NotificationService` can work with any `BaseNotification` without needing to know the exact implementation.

---

#### 4. **Interface Segregation Principle (ISP)**

Clients shouldn’t be forced to implement interfaces they don’t use. Suppose we have an order interface `IOrderService`. Instead of creating a large interface with unrelated methods, we divide it into smaller, more specific interfaces.

```csharp
public interface IOrderPlacementService
{
    void PlaceOrder(Order order);
}

public interface IOrderTrackingService
{
    OrderStatus TrackOrder(int orderId);
}

public class OrderService : IOrderPlacementService, IOrderTrackingService
{
    public void PlaceOrder(Order order)
    {
        // Place order logic
    }

    public OrderStatus TrackOrder(int orderId)
    {
        // Track order logic
    }
}
```

By splitting the interfaces, a class that only needs order tracking, for example, can implement just `IOrderTrackingService` without being forced to include order placement functionality.

---

#### 5. **Dependency Inversion Principle (DIP)**

High-level modules shouldn’t depend on low-level modules but on abstractions. In our scenario, the `OrderProcessor` might depend on `IDiscount` and `INotificationService` interfaces, rather than concrete implementations. This makes the code flexible and allows us to switch implementations easily.

```csharp
public interface IDiscount
{
    decimal ApplyDiscount(decimal orderTotal);
}

public interface INotificationService
{
    void SendNotification(Customer customer, string message);
}

public class OrderProcessor
{
    private IDiscount _discount;
    private INotificationService _notificationService;

    public OrderProcessor(IDiscount discount, INotificationService notificationService)
    {
        _discount = discount;
        _notificationService = notificationService;
    }

    public void ProcessOrder(Order order)
    {
        var total = order.TotalAmount;
        total = _discount.ApplyDiscount(total);

        _notificationService.SendNotification(order.Customer, "Your order has been processed.");
    }
}
```

By depending on abstractions (`IDiscount` and `INotificationService`), `OrderProcessor` can work with any implementation of discount or notification, making the code flexible and extensible.

---

### Summary

In this e-commerce scenario:

- **SRP** keeps each class focused on a single responsibility.
- **OCP** allows us to extend functionality without altering existing code.
- **LSP** ensures subclasses can replace their base classes without issues.
- **ISP** breaks down interfaces into smaller, more specific ones.
- **DIP** makes our high-level modules independent of specific implementations, promoting flexibility. 

This approach results in code that is modular, flexible, and easier to maintain.