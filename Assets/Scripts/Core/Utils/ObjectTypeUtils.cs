using System;

namespace Alkawa.Core
{
    public class ObjectTypeUtils
    {

        public static bool IsSameOrSubclass(Type typeBase, Type typeDescendant)
        {
            return typeDescendant.IsSubclassOf(typeBase) || typeDescendant == typeBase;
        }

        public static bool ConvertToType<T>(object value,out T result)
        {
            result = default(T);
            if (typeof(T) == typeof(bool))
            {
                bool boolResult;
                if (bool.TryParse(value.ToString(), out boolResult))
                {
                    result = (T)(object)boolResult;
                    return true;
                }

                return false;
            }
            else
            if (typeof(T) == typeof(int))
            {
                int intResult;
                if (int.TryParse(value.ToString(), out intResult))
                {
                    result = (T)(object)intResult;
                    return true;
                }

                return false;
            }
            else
            if (typeof(T) == typeof(UInt32))
            {
                UInt32 uint32Result;
                if (UInt32.TryParse(value.ToString(), out uint32Result))
                {
                    result = (T)(object)uint32Result;
                    return true;
                }

                return false;
            }
            else
            if (typeof(T) == typeof(float))
            {
                float floatResult;
                if (float.TryParse(value.ToString(), out floatResult))
                {
                    result = (T)(object)floatResult;
                    return true;
                }

                return false;
            }
          
            result = (T)(object)(value);
            return true;
        }
        
        public static string GetFullDelegateName(Delegate _delegate)
            => $"{_delegate.Method.DeclaringType}.{_delegate.Method.Name}";
        
    }
}