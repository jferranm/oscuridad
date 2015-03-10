
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public static class DataProviderHelper
    {

        public static T[] InitProviders<T>(string[] activeProviders)
        {
            
            if (activeProviders != null)
            {
                List<T> result = new List<T>();
                Type interfaceType = typeof(T);

                foreach (string providerName in activeProviders)
                {
                    Type providerType = Type.GetType("Frankfort.VBug.Internal." + providerName);
                    if (providerType == null)
                        providerType = Type.GetType(providerName);

                    if (providerType != null){
                        if (interfaceType.IsAssignableFrom(providerType)) {
                            try {
                                T newProvider = (T)Activator.CreateInstance(providerType);
                                result.Add(newProvider);
                            } catch (Exception e) {
                                if (vBug.settings.general.debugMode)
                                    Debug.LogError(e.Message + e.StackTrace);
                            }
                        } else {
                            if (vBug.settings.general.debugMode)
                                Debug.LogError("vBug - InitProviders ERROR: Provider " + providerName + " does not implement " + providerType.FullName + "!");
                        }
                    } else {
                        if (vBug.settings.general.debugMode)
                            Debug.LogError("vBug - InitProviders ERROR: Provider " + providerName + " could not be found!");
                    }
                }

                return result.ToArray();
            }
            return default(T[]);
        }


    }
}
