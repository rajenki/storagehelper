StorageHelper 1.1.6 | Rajen Kishna
===============================================================================

StorageHelper is a helper class for Windows Phone and Windows apps to easily 
save, load and delete data from storage.

The Windows Phone Silverlight methods save to the local storage of the device 
by default, as this is the only API available, while the Windows and Windows
Phone (Store/Universal) methods default to roaming storage. You can point to 
local storage by using the overloads of the methods outlined below.

Any questions/feedback/suggestion, let me know on Twitter:
@rajen_k

Or contribute directly:
https://github.com/rajenki/storagehelper

===============================================================================
Usage:
===============================================================================

Windows Phone 8 / 8.1 (Silverlight)
-------------------------------------------------------------------------------
Application state (temporary):
------------------------------
Storage.SaveState("MYKEY", MYOBJ);
Storage.LoadState<TYPE>("MYKEY");
Storage.DeleteState("MYKEY");

Persistent storage (using files):
---------------------------------
[await] Storage.SaveAsync("MYKEY", MYOBJ);
[await] Storage.SaveAsync("MYKEY", MYOBJ, true); // use JSON
[await] Storage.LoadAsync<TYPE>("MYKEY");
[await] Storage.LoadAsync<TYPE>("MYKEY", true); // use JSON
[await] Storage.DeleteAsync("MYKEY");
-------------------------------------------------------------------------------

Windows 8 / Windows Phone 8.1 (Store/Universal)
-------------------------------------------------------------------------------
Persistent storage (using files):
---------------------------------
[await] Storage.SaveAsync("MYKEY", MYOBJ);
[await] Storage.SaveAsync("MYKEY", MYOBJ, true); // use JSON
[await] Storage.SaveAsync(ApplicationData.Current.LocalFolder, "MYKEY", MYOBJ);
[await] Storage.LoadAsync<TYPE>("MYKEY");
[await] Storage.LoadAsync<TYPE>("MYKEY", true); // use JSON
[await] Storage.LoadAsync<TYPE>(ApplicationData.Current.LocalFolder, "MYKEY");
[await] Storage.DeleteAsync("MYKEY");
[await] Storage.DeleteAsync(ApplicationData.Current.LocalFolder, "MYKEY");

Persistent settings storage (using dictionary):
-----------------------------------------------
Storage.SaveSetting("MYKEY", MYOBJ);
Storage.SaveSetting(ApplicationData.Current.LocalSettings, "MYKEY", MYOBJ);
Storage.LoadSetting("MYKEY");
Storage.LoadSetting(ApplicationData.Current.LocalSettings, "MYKEY");
Storage.DeleteSetting("MYKEY");
Storage.DeleteSetting(ApplicationData.Current.LocalSettings, "MYKEY");
-------------------------------------------------------------------------------