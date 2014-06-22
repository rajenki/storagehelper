# StorageHelper

StorageHelper is a helper class for Windows Phone and Windows apps to easily save, load and delete data from storage. By default, XML (de)serialization is used, but overrides are available for JSON (de)serialization instead.

## How to use

* Download or clone the source code from the GitHub repository or use the [NuGet package] (http://www.nuget.org/packages/StorageHelper/)

### Windows Phone Silverlight

* In Windows Phone Silverlight projects, use the following syntax
  * NOTE: The persistent storage APIs use the [LocalFolder] (http://msdn.microsoft.com/en-US/library/windows/apps/windows.storage.applicationdata.localfolder.aspx) container for Silverlight apps
```csharp
// Use the application state dictionary (temporary storage)
Storage.SaveState("MYKEY", MYOBJ);
Storage.LoadState<TYPE>("MYKEY");
Storage.DeleteState("MYKEY");

// Use the persistent storage (using files)
[await] Storage.SaveAsync("MYKEY", MYOBJ);
[await] Storage.SaveAsync("MYKEY", MYOBJ, true); // use JSON
[await] Storage.LoadAsync<TYPE>("MYKEY");
[await] Storage.LoadAsync<TYPE>("MYKEY", true); // use JSON
[await] Storage.DeleteAsync("MYKEY");
```

### Windows Store (Windows 8.1, Windows Phone 8.1, Universal)

* In Windows Store projects, use the following syntax
  * NOTE: The storage APIs use the [RoamingFolder] (http://msdn.microsoft.com/en-US/library/windows/apps/windows.storage.applicationdata.roamingfolder.aspx)/[RoamingSettings] (http://msdn.microsoft.com/en-US/library/windows/apps/windows.storage.applicationdata.roamingsettings.aspx) containers for Windows Store apps, but overrides are available to use their local counterparts instead.
```csharp
// Use the persistent storage (using files)
[await] Storage.SaveAsync("MYKEY", MYOBJ);
[await] Storage.SaveAsync("MYKEY", MYOBJ, true); // use JSON
[await] Storage.SaveAsync(ApplicationData.Current.LocalFolder, "MYKEY", MYOBJ);
[await] Storage.LoadAsync<TYPE>("MYKEY");
[await] Storage.LoadAsync<TYPE>("MYKEY", true); // use JSON
[await] Storage.LoadAsync<TYPE>(ApplicationData.Current.LocalFolder, "MYKEY");
[await] Storage.DeleteAsync("MYKEY");
[await] Storage.DeleteAsync(ApplicationData.Current.LocalFolder, "MYKEY");

// Use the persistent settings storage (using dictionary)
Storage.SaveSetting("MYKEY", MYOBJ);
Storage.SaveSetting(ApplicationData.Current.LocalSettings, "MYKEY", MYOBJ);
Storage.LoadSetting("MYKEY");
Storage.LoadSetting(ApplicationData.Current.LocalSettings, "MYKEY");
Storage.DeleteSetting("MYKEY");
Storage.DeleteSetting(ApplicationData.Current.LocalSettings, "MYKEY");
```

## Credits

This library has been written by [Rajen Kishna] (https://twitter.com/rajen_k) with input from [Dave Smits] (https://twitter.com/davesmits).
This library has been created as a side-project to assist the community and is provided "as is" with no warranty whatsoever and has no relations to our employers (Microsoft and Sparked).
