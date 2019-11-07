# Timeta

Kymeta WPF coding challenge.

## Purpose
Timeta is meant to demonstrate two primary methodologies of interacting with a background service from a WPF UI. In addition it contains an example of a simple WPF control. It implements the MVVM pattern by keeping the ViewModel and other domain logic completely separate from the view layer. In fact the `Timeta.Domain` assembly has no reference to either the the view assembly, nor to any of the WPF assemblies. This approach aids in clean separation between view and logic layers. And additional benefit is that the ViewModel layer could feasibly be reused with another UI technology.

## ThreadTimerService
The service implementation starts a background thread that loops endlessly, waiting 1000ms between each loop. Access to the thread data is controlled using the C# `lock` statement to prevent race conditions.

### Alternatives to Using `Thread`
There are few reasons that a WPF app should ever interact with a thread directly. The easiest implementation of this service would make use of a `DispatcherTimer` and prevent concurrency issues through the use of Dispatcher.Invoke to schedule UI updates on the UI thread.


## Approaches
The app has a UI section for each method. Note that both are interacting with the same instance of the timer service. Let's examine the structure of each approach, along their pros and cons.

### ViewModel RelayCommand Binding
In this pattern, we use a ViewModel to control the state of the View, as well as mediate its interactions with the rest of the application. 

The ViewModel exposes implementations of `System.Windows.ICommand` that are available for the view to bind to. The ViewModel is in full control of any available actions, as well as if they can be executed. The View can then consume the public properties of the ViewModel in order to construct the UI.

We implement `ICommand` here with `Timeta.Domain.Framework.RelayCommand`.

#### Pros
- Straightforward
- Easy to validate
#### Cons
- Not easily reusable
- Requires boilerplate if it's just a facade for a service

This approach is best when a ViewModel represents a unique way of interacting with the application domain, if it requires complex data transformations or calculations, or if you want to implement `INotifyDataErrorInfo` for binding validation.

### RoutedCommand Service Invocation
Instead of using a ViewModel to control a particular service, it is instead instantiated as an application resource, then the service's interfaces are wired up during app startup with `CommandBinding`s at the top of the visual tree.

A `RoutedCommand` is fired by a `CommandSource` such as a button or menu item, then traverses up the visual tree until it finds an element with a `CommandBinding` that can handle it. In this way, a command can be issued to the service from anywhere in the visual tree, whether it be in a `UserControl` or a custom `ControlTemplate`, without any additional boilerplate.

When the view needs to consume the state of the service, it can use a binding, as the service instance is a resource:
```
Text="{Binding Seconds, Source={StaticResource TimerService}}"
```

#### Pros
- Universal
- Easily reusable
- Flexible
#### Cons
- More initial setup
- More complicated in general

This approach is best suited towards commonly used services that may be invoked from multiple places in the application. It requires more work to process any of the bound values before they're passed to the service.

### Incrementer Control
This basic control also uses `RoutedCommand`s to allow the control template to invoke actions in the control itself. This gives the control template freedom to invoke the commands from where is needed.

### Shortcomings
As this was a short coding project, I didn't have time to implement many of the techniques I would use in a production application. 
- Implementing `INotifyDataErrorInfo` in a base class can let ViewModels use `ValidationAttribute`s to validate their public properties, as well as provide error messaging to view layer. Instead I derived from `ValidationRule` to create validation logic, and manually added a rule instance to each binding that needed validation.
- Keeping your service in a seperate assembly without reference to WPF assemblies means you have to create `CommandBinding`s in application startup code. The right way to do this is create an interface for your services to derive from that provides a collection of adapter objects that can easily be transformed into command bindings. This way each service is responsible for defining it's command bindings, and the application can iterate over a list when constructing them. Instead I used an ugly little kludge in the app startup code.

