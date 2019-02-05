# MGCodeExample
another code example for interviews, this time in .net core

have something like dotnet 2.2 runnable from your console 
```
git clone to your local

build with
dotnet build

run example data with
dotnet run --project mgcodeexample\MG_Test.csproj

run the three nunit based unit tests with
dotnet test

```

regarding the code itself - I parse through the csv line by line and build up an object that represents the json data. 
I then feed the object into Newtonsoft Json.net serializer to turn the object into json. 
