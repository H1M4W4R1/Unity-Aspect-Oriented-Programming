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
# Known problems
* Not tested on: iOS, OSX, Linux (but it should work...)

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

## Can I move variables between aspects? For example MethodEnter & MethodExit?
Yes, you should use Variables. All arguments used in Aspects inherit BaseExecutionArgs. Inside you've methods:
* `AddVariable(string name, object value)` - which adds variable to local memory of aspect call
* `GetVariable<T>(string name)` - which gets variable using name, if none returns default

## Can I throw error from eg. IMethodEnterAspect::OnMethodEnter(<args>)?
Yes, you can. Just use `args.Throw(<System.Exception>);` and you're fine. It is recommended to use `return` after this method, otherwise the Aspect code will continue to execute.
	
## Can I create field aspects?
No, you can't, however you can encapsulate field into Property which is supported - it is less painful solution both for you and your processor :)
An example:
```cs
[Serializable]
public class Foo
{
    [SerializeField]
    private string name;
        
    public string Name { get => name; [Observable] set => name = value; }
}

public class ObservableAttribute : Attribute, IMethodEnterAspect
{
    public void OnMethodEnter(MethodExecutionArguments args)
    {
       args.Throw(new Exception("Hello world! Breaking things is fun!"));
       return;
       Debug.Log("Foo has been set");
    }
}
```

## Can I've one aspect for properties, methods, events etc.?
Yes, just implement all interfaces you desire :)

## Can I fork this project?
It's MIT licensed. Feel free to commit updated versions into Git. Feel free to send me a link, so I could implement your upgrades too.

## Wall of Fame
[494311870](https://github.com/494311870) - build compatibility (including Android), because I was too lazy to fix this... <br/><br/>
[blazejhanzel](https://github.com/blazejhanzel) - multi-DLL weaving implementation
