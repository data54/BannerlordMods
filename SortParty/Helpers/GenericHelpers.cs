using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SortParty
{
    public static class GenericHelpers
    {
        public static MethodInfo GetPrivateMethod<T>(string methodName, T reflectionObject)
        {
            return GetMethod("RefreshPartyInformation", reflectionObject, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static MethodInfo GetMethod<T>(string methodName, T reflectionObject, BindingFlags bindingFlags)
        {
            return reflectionObject.GetType().GetMethod("RefreshPartyInformation", bindingFlags);
        }
    }
}
