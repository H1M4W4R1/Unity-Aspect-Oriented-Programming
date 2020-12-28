# About the project

**Unity AOP** also known as **Unity Aspect Oriented Programming** is a framework for Unity3D  that allows to use aspects in your games. It uses Mono.Cecil to rewrite compiled assembly basing on aspect attributes.

```cs

public class MyCustomAspect : Attribute, IMethodEnterAspect
{
	public void OnMethodEnter(MethodExecutionArguments args)
	{
		Debug.Log("Hello my dear method named '" + args.method.Name + "'");
	}
}

```

# Possible Attribute Usages
You can use Aspect Attributes such as following:
## Methods
* `IMethodEnterAspect`- executed when method is entered
* `IMethodExitAspect` - executed before method returns value to source of invocation
* `IMethodExceptionThrownAspect`- executed when method throws an exception during execution
* `IMethodBoundaryAspect` - combo for `IMethodEnterAspect` and `IMethodExitAspect`
 
## Properties
* `IPropertyGetEnterAspect` - executed when get method of property begins to be invoked
* `IPropertyGetExitAspect` - executed when get method of property finishes its invocation
* `IPropertySetEnterAspect` - executed when set method of property begins to be invoked
* `IPropertySetExitAspect` - executed when set method of property finishes its invocation
* `IPropertyExceptionThrownAspect` - executed when property throws an exception
 
## Events
* `IBeforeEventInvokedAspect` - executed before event is invoked
* `IAfterEventInvokedAspect` - executed after event is invoked
* **...InvokedAspect does not contain event arguments!**
* `IEventBeforeListenerAddedAspect` - executed before listener is added to event
* `IEventAfterListenerAddedAspect` - executed after listener is added to event
* `IEventBeforeListenerRemovedAspect` - executed before listener is removed from event
* `IEventAfterListenerRemovedAspect` - executed after listener is removed from event
* `IEventExceptionThrownAspect` - executed when event throws exception (excluding invocation)

## How to create aspect based on those interfaces?
It's simple. 
1. Create new class
2. Inherit from Attribute
3. Add desired interfaces
4. Implement methods
5. Profit
> It's recommended to use [AttributeUsage] on aspects to limit it to proper members.

## Can I move variables between aspects? For example MethodEnter i MethodExit?
Yes, you should use Variables. All arguments used in Aspects inherit BaseExecutionArgs. Inside you've methods:
* `AddVariable(string name, object value)` - which adds variable to local memory of aspect call
* `GetVariable<T>(string name)` - which gets variable using name, if none returns default

## Can I've one aspect for properties, methods, events etc.?
Yes, just implement all interfaces you desire :)

## Can I fork this project?
It's MIT licensed. Feel free to commit updated versions into Git. Feel free to send me a link, so I could implement your upgrades too.
