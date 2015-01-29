using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

#if WINDOWS_PHONE
using Microsoft.Phone.Shell;

namespace StorageHelper.WindowsPhoneSilverlight
#elif WINDOWS_PHONE_APP
namespace StorageHelper.WindowsStore
#elif WINDOWS_APP
namespace StorageHelper.WindowsStore
#elif NETFX_CORE
namespace StorageHelper.WindowsStore
#endif
{
    public class Storage
    {
#if WINDOWS_PHONE
        private static StorageFolder DefaultFolder = ApplicationData.Current.LocalFolder;
#elif NETFX_CORE
        private static StorageFolder DefaultFolder = ApplicationData.Current.RoamingFolder;
        private static ApplicationDataContainer DefaultSettings = ApplicationData.Current.RoamingSettings;
#endif



        #region Save Storage
        /// <summary>
#if WINDOWS_PHONE
        /// Saves an entry with the provided path and value to the application's persisted local storage.
#elif NETFX_CORE
        /// Saves an entry with the provided path and value to the application's persisted roaming storage.
#endif
        /// </summary>
        /// <param name="path">The path for the entry to save.</param>
        /// <param name="data">The object to save.</param>
        /// <param name="useJson">Optional parameter to use JSON instead of XML</param>
        /// <returns>True if the entry has been saved.</returns>
        public static Task<bool> SaveAsync(string path, object data, bool useJson = false)
        {
            return SaveAsync(DefaultFolder, path, data, useJson);
        }

        /// <summary>
        /// Saves an entry with the provided path and value to the application's persisted specified storage.
        /// </summary>
        /// <param name="folder">The folder to save the entry to.</param>
        /// <param name="path">The path for the entry to save.</param>
        /// <param name="data">The object to save.</param>
        /// <param name="useJson">Optional parameter to use JSON instead of XML</param>
        /// <returns>True if the entry has been saved.</returns>
        public async static Task<bool> SaveAsync(StorageFolder folder, string path, object data, bool useJson = false)
        {
            const CreationCollisionOption option = CreationCollisionOption.ReplaceExisting;

            try
            {
                var file = await folder.CreateFileAsync(path, option).AsTask().ConfigureAwait(false);
                var saveData = new MemoryStream();

                if (!useJson)
                {
                    var x = new XmlSerializer(data.GetType());
                    x.Serialize(saveData, data);
                }
                else
                {
                    var j = new DataContractJsonSerializer(data.GetType());
                    j.WriteObject(saveData, data);
                }

                if (saveData.Length > 0)
                {
                    using (var fileStream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
                    {
                        saveData.Seek(0, SeekOrigin.Begin);
                        await saveData.CopyToAsync(fileStream);
                        await fileStream.FlushAsync();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Load Storage
        /// <summary>
#if WINDOWS_PHONE
        /// Gets the value associated with the specified path from the application's persisted local storage.
#elif NETFX_CORE
        /// Gets the value associated with the specified path from the application's persisted roaming storage.
#endif
        /// </summary>
        /// <typeparam name="T">The type of the object at the specified path.</typeparam>
        /// <param name="path">The path for the entry to retrieve.</param>
        /// <param name="useJson">Optional parameter to use JSON instead of XML.</param>
        /// <returns>The retrieved element or an empty object of type T if no entry was found at the specified path.</returns>
        public static Task<T> LoadAsync<T>(string path, bool useJson = false)
        {
            return LoadAsync<T>(DefaultFolder, path, useJson);
        }

        /// <summary>
        /// Gets the value associated with the specified path from the application's persisted specified storage.
        /// </summary>
        /// <typeparam name="T">The type of the object at the specified path.</typeparam>
        /// <param name="folder">The folder to retrieve the entry from.</param>
        /// <param name="path">The path for the entry to retrieve.</param>
        /// <param name="useJson">Optional parameter to use JSON instead of XML.</param>
        /// <returns>The retrieved element or an empty object of type T if no entry was found at the specified path.</returns>
        public async static Task<T> LoadAsync<T>(StorageFolder folder, string path, bool useJson = false)
        {
            try
            {
                var file = await folder.GetFileAsync(path).AsTask().ConfigureAwait(false);

                using (var inStream = await file.OpenSequentialReadAsync().AsTask().ConfigureAwait(false))
                {
                    if (!useJson)
                    {
                        var x = new XmlSerializer(typeof(T));
                        return (T)x.Deserialize(inStream.AsStreamForRead());
                    }
                    else
                    {
                        var j = new DataContractJsonSerializer(typeof(T));
                        return (T)j.ReadObject(inStream.AsStreamForRead());
                    }
                }
            }
            catch
            {
                return default(T);
            }
        }
        #endregion

        #region Delete Storage
        /// <summary>
#if WINDOWS_PHONE
        /// Removes the entry with the specified path from the application's persisted local storage.
#elif NETFX_CORE
        /// Removes the entry with the specified path from the application's persisted roaming storage.
#endif
        /// </summary>
        /// <param name="path">The path for the entry to remove.</param>
        /// <returns>True if the entry has been removed.</returns>
        public static Task<bool> DeleteAsync(string path)
        {
            return DeleteAsync(DefaultFolder, path);
        }

        /// <summary>
        /// Removes the entry with the specified path from the application's persisted specified storage.
        /// </summary>
        /// <param name="folder">The folder to remove the entry from.</param>
        /// <param name="path">The path for the entry to remove.</param>
        /// <returns>True if the entry has been removed.</returns>
        public async static Task<bool> DeleteAsync(StorageFolder folder, string path)
        {
            try
            {
                var file = await folder.GetFileAsync(path).AsTask().ConfigureAwait(false);
                await file.DeleteAsync().AsTask().ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

#if !WINDOWS_PHONE
        #region Save Setting
        /// <summary>
#if WINDOWS_PHONE
        /// Saves an entry with the provided key and value to the application's persisted local settings.
#elif NETFX_CORE
        /// Saves an entry with the provided key and value to the application's persisted roaming settings.
#endif
        /// </summary>
        /// <param name="key">The key for the entry to save.</param>
        /// <param name="value">The object to save.</param>
        /// <returns>True if the entry has been saved.</returns>
        public static bool SaveSetting(string key, object value)
        {
            return SaveSetting(DefaultSettings, key, value);
        }

        /// <summary>
        /// Saves an entry with the provided key and value to the application's persisted specified settings store.
        /// </summary>
        /// <param name="container">The container to save the entry to.</param>
        /// <param name="key">The key for the entry to save.</param>
        /// <param name="value">The object to save.</param>
        /// <returns>True if the entry has been saved.</returns>
        public static bool SaveSetting(ApplicationDataContainer container, string key, object value)
        {
            try
            {
                container.Values[key] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Load Setting
        /// <summary>
#if WINDOWS_PHONE
        /// Gets the value associated with the specified key from the application's persisted local settings.
#elif NETFX_CORE
        /// Gets the value associated with the specified key from the application's persisted roaming settings.
#endif
        /// </summary>
        /// <typeparam name="T">The type of the object at the specified key.</typeparam>
        /// <param name="key">The key for the entry to retrieve.</param>
        /// <returns>The retrieved element or an empty object of type T if no entry was found at the specified key.</returns>
        public static T LoadSetting<T>(string key)
        {
            return LoadSetting<T>(DefaultSettings, key);
        }

        /// <summary>
        /// Gets the value associated with the specified key from the application's persisted specified settings store.
        /// </summary>
        /// <typeparam name="T">The type of the object at the specified key.</typeparam>
        /// <param name="container">The container to retrieve the entry from.</param>
        /// <param name="key">The key for the entry to retrieve.</param>
        /// <returns>The retrieved element or an empty object of type T if no entry was found at the specified key.</returns>
        private static T LoadSetting<T>(ApplicationDataContainer container, string key)
        {
            try
            {
                var value = container.Values[key];
                if (value == null)
                    return default(T);
                return (T)value;
            }
            catch
            {
                return default(T);
            }
        }
        #endregion

        #region Delete Setting
        /// <summary>
#if WINDOWS_PHONE
        /// Removes the entry with the specified key from the application's persisted local settings.
#elif NETFX_CORE
        /// Removes the entry with the specified key from the application's persisted roaming settings.
#endif
        /// </summary>
        /// <param name="key">The key for the entry to remove.</param>
        /// <returns>True if the entry has been removed.</returns>
        public static bool DeleteSetting(string key)
        {
            return DeleteSetting(DefaultSettings, key);
        }

        /// <summary>
        /// Removes the entry with the specified key from the application's persisted specified settings store.
        /// </summary>
        /// <param name="container">The container to remove the entry from.</param>
        /// <param name="key">The key for the entry to remove.</param>
        /// <returns>True if the entry has been removed.</returns>
        private static bool DeleteSetting(ApplicationDataContainer container, string key)
        {
            try
            {
                container.Values.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
#endif
    }
}
