# Cryptolens Licensing for .NET

This API serves as a layer that simplifies communication with Cryptolens Web API 2 and 3 (https://app.cryptolens.io/docs/api).

> You can access the documentation of the library at https://help.cryptolens.io/api/dotnet/.

Please check out our guide on common errors during integration: https://help.cryptolens.io/faq/index#troubleshooting-api-errors

## Getting started

### Install Nuget package

In Visual Studio package manager
```
PM> Install-Package Cryptolens.Licensing
```

Using dotnet CLI
```
dotnet add package Cryptolens.Licensing
```

**If you are targeting Mac, Linux or Unity/Mono, we recommend to use the cross platform version of tha package.**

In Visual Studio package manager
```
PM> Install-Package Cryptolens.Licensing.CrossPlatform
```

Using dotnet CLI
```
dotnet add package Cryptolens.Licensing.CrossPlatform
```



### Example code
* [Key verification](https://help.cryptolens.io/examples/key-verification)
* [Offline verification](https://help.cryptolens.io/examples/offline-verification)

## Compatibility

To get access to all of the featues in the library, .NET Framework 4.6 or above has to be used. We have summarized the functionality that is included in each framework. Note, .NET Standard means the library can run on multiple platforms (eg. .NET Core), based on the following [document](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

* **.NET Framework 4.0** - Verifying metadata signatures is not supported.
* **.NET Framework 4.6** - All features supported.
* **.NET Standard 2.0** - Computing machine codes is not supported. You can still use the available hash functions to compute a machine code, assuming you can collect machine specific information.
* **Unity/Linux/Mac** - You need to use the packages in the "Without System.Management" folder on the release page. You can find more info [here](https://help.cryptolens.io/getting-started/unity). 

Cryptolens.Licensing library does also work on Mono (eg. when running on Linux or Unity), however you need to use the version of the library that is compiled without System.Management. You can find pre-compiled binaries [here](https://github.com/Cryptolens/cryptolens-dotnet/releases) or compile it yourself by following the instructions below.

## Running without System.Management (Linux, Unity, Mono)
If you plan to use Cryptolens.Licensing on Linux, you need to use a version of the library that does not include `System.Management`. By excluding `System.Management`, calculating device fingerprints will not work (i.e. any method that depends on `SKM.getMachineCode`). You can still use the available hash functions and helper methods, as long as `SKM.getMachineCode` is not invoked.

To compile without System.Management, open `Cryptolens.Licensing.csproj` and remove the `<DefineConstants>SYSTEM_MANAGEMENT</DefineConstants>` below inside the `<PropertyGroup>` tag, i.e.

```
<PropertyGroup>
    ...

    <DefineConstants>SYSTEM_MANAGEMENT</DefineConstants>   <-- remove this
</PropertyGroup>
```

> Make sure there are no other `DefineConstants` definitions later in the file, as these will override the value.

## Other settings
### 'System.MethodAccessException' when calling Helpers.GetMachineCode
In some Windows environments (e.g. when developing Excel addins), it might not be feasible to call Helpers.GetMachineCode on .NET Framework 4.6. The reason for this is the call we make to `System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform`. To fix this, we have added a boolean flag in `Helpers` class. Before calling `Helpers.GetMachineCode` or `Helpers.IsOnRightMachine`, please set `Helpers.WindowsOnly=True`.

```cs
Helpers.WindowsOnly = true;
var machineCode = Helpers.GetMachineCode();
```

### Turn off KeepAlive
In order to perform API calls with KeepAlive disabled, please set `HelperMethods.KeepAlive=False`. If you want to remove all references to code that uses KeepAlive that is true, you can compile the library with the `KeepAliveDisabled` flag, which is added the same way as `SYSTEM_MANAGEMENT` mentioned earlier. If you have multiple flags, they can be separated with a semicolon.

### Proxy settings
To change proxy settings, you can use the variable `HelperMethods.proxy`.

## Old examples

### Check Against Time Rollback
In order to make sure that the local time (date and time) wasn't changed by the user, the following code can be used.
```
public void HasLocalTimeChanged()
{
    bool hasChanged = SKGL.SKM.TimeCheck();

    if(hasChanged)
    {
        Debug.WriteLine("The local time was changed by the user. Validation fails.");
    }
    else
    {
        Debug.WriteLine("The local time hasn't been changed. Continue validation.");
    }
}
```

### Calculating Machine code
Machine code can be calculated with the function below. Any other hash algorithm will do, as long as it only contains letters and digits only.
```
public void TestingHashes()
{
    //eg. "61843235" (getEightDigitsLongHash)
    //eg. "D38F13CAB8938AC3C393BC111E1A85BB4BA2CCC9" (getSHA1)
    string machineID1 = SKGL.SKM.getMachineCode(SKGL.SKM.getEightDigitsLongHash);
    string machineID2 = SKGL.SKM.getMachineCode(SKGL.SKM.getSHA1);
}
```
