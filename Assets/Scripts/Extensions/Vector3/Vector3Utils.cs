using UnityEngine;
using System.Globalization;

namespace Alkawa
{
    public static class Vector3Utils
    {
        internal static bool TryFloat(string input, out float f)
        {
            if (!float.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out f))
            {
                if (!float.TryParse(input, out f))
                    return false;
            }

            return true;
        }

        public static bool TryParse(string input, out Vector3 vector3)
        {
            vector3 = default;

            if (string.IsNullOrEmpty(input))
                return false;

            input = input.Trim(new char[] { '(', ')' });

            string[] floatArray = input.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (floatArray.Length != 3)
                return false;

            if (!TryFloat(floatArray[0], out float x))
                return false;
            if (!TryFloat(floatArray[1], out float y))
                return false;
            if (!TryFloat(floatArray[2], out float z))
                return false;

            vector3 = new Vector3(x, y, z);
            return true;

        }


        //output (x,y,z)
        public static string ToStringNoSpaces(this Vector3 target, CultureInfo cultureInfo)
        {
            return $"({target.x.ToString(cultureInfo)},{target.y.ToString(cultureInfo)},{target.z.ToString(cultureInfo)})";
        }

        public static string ToStringNoSpaces(this Vector3 target, CultureInfo cultureInfo, string format)
        {
            return $"({target.x.ToString(format, cultureInfo)},{target.y.ToString(format, cultureInfo)},{target.z.ToString(format, cultureInfo)})";
        }

        public static Vector3 Abs(Vector3 _vector)
        {
            Vector3 result;
            result.x = Mathf.Abs(_vector.x);
            result.y = Mathf.Abs(_vector.y);
            result.z = Mathf.Abs(_vector.z);
            return result;
        }

        public static readonly Vector3 s_2DOne = new Vector3(1,1,0);

}

}