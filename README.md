A little helper library that given an (optionaly weighted) list of methods chooses one at random. Useful if you are making a game or something I guess.

```csharp
	//add actions to the selector
	var selctor = RandomSelector
			.AddAction(() => { doSomething(); })
			.AddAction(() => { doSomeOtherThing(); });

	selector.Choose(); //randomly picks and action to run
```

You can also optionally weight each action.

```csharp
	//add actions to the selector (must add up to 1.0)
	var selctor = RandomSelector
			.AddAction(0.7, () => { probablyDoThis(); })
			.AddAction(0.2, () => { maybeDoThis(); })
			.AddAction(0.1, () => { smallChanceOfThis(); });

	selector.Choose(); //randomly picks and action to run
```