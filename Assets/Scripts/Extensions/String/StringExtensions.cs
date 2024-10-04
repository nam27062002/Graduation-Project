using System;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace Alkawa
{
    public enum EBufferPosition
    {
        Begin,
        Middle,
        End
    }
    public static class StringExtensions
    {
        public static string Truncate(this string target, int len)
        {
            if (string.IsNullOrEmpty(target))
                return target;
            return target.Length <= len ? target : target.Substring(0, len);
        }

        public static string RemoveStart(this string target, string _stringToRemove)
        {
            int indexOfPrefix = target.IndexOf(_stringToRemove, StringComparison.InvariantCulture);
            if (indexOfPrefix != 0)
                return target;
            return target.Remove(indexOfPrefix, _stringToRemove.Length);
        }
        
        public static string RemoveUntil(this string target, string _stringToRemove, bool _keepTargetString = false)
        {
            int indexOfPrefix = target.IndexOf(_stringToRemove, StringComparison.InvariantCulture);
            if (indexOfPrefix < 0)
                return target;

            var lengthToRemove = indexOfPrefix;
            if (!_keepTargetString)
                lengthToRemove += _stringToRemove.Length;
            
            return target.Remove(0, lengthToRemove );
        }
        
        public static string RemoveFrom(this string target, string _stringToRemove, bool _keepTargetString = false)
        {
            int indexOfPrefix = target.LastIndexOf(_stringToRemove, StringComparison.InvariantCulture);
            if (indexOfPrefix < 0)
                return target;

            var endIndex = indexOfPrefix;
            if (_keepTargetString)
                endIndex += _stringToRemove.Length;
            
            return target.Substring(0, endIndex);
        }
        
        public static string RemoveEnd(this string target,  string _stringToRemove)
        {
            int indexOfPrefix = target.LastIndexOf(_stringToRemove, StringComparison.InvariantCulture);
            int newLength = target.Length - _stringToRemove.Length;
            if (indexOfPrefix != newLength)
                return target;
            return target.Substring(0, newLength);
        }

        public static string ReplaceFirstOccurence(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        
        public static bool IsNullOrEmpty( this string _this )
        {
            return string.IsNullOrEmpty(_this);
        }

        public static bool HasValue(this string _this)
        {
            return !string.IsNullOrEmpty(_this);
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static string FirstCharToUpper(this string _this)
        {
            switch (_this)
            {
                case null: throw new ArgumentNullException(nameof(_this));
                case "": throw new ArgumentException($"{nameof(_this)} cannot be empty", nameof(_this));
                default: return _this[0].ToString().ToUpper() + _this.Substring(1);
            }
        }
        
        private const string kBufferSeparator  = "...";
        public static string Buffer(this string text, int finalSize, EBufferPosition position)
        {
            if (text.Length <= finalSize)
                return text;
            
            StringBuilder sb = new StringBuilder();
            int toTake = finalSize - kBufferSeparator.Length;
            
            switch (position)
            {
                case  EBufferPosition.Begin:
                    sb.Append(kBufferSeparator);
                    sb.Append(text.Substring(text.Length - toTake, toTake));
                    break;
                
                case  EBufferPosition.Middle:
                    int takeBegin = toTake / 2;
                    int takeEnd = takeBegin;
                    
                    if (toTake % 2 == 1)
                    {
                        takeEnd++;
                    }
                    
                    sb.Append(text.Substring(0, takeBegin));
                    sb.Append(kBufferSeparator);
                    sb.Append(text.Substring(text.Length - takeEnd, takeEnd));
                    break;
                
                case  EBufferPosition.End:
                    sb.Append(text.Substring(0, toTake));
                    sb.Append(kBufferSeparator);
                    break;
            }

            return sb.ToString();
        }
        
        
        /// <summary>
        /// Sets the color of the text according to the parameter value.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="color">Color.</param>
        public static string Colored(this string message, Color color)
        {
            return string.Format("<color={0}>{1}</color>", color.ToHexStringRGBA(), message);
        }

        /// <summary>
        /// Sets the color of the text according to the traditional HTML format parameter value.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="color">Color</param>
        public static string Colored(this string message, string colorCode)
        {
            return string.Format("<color={0}>{1}</color>", colorCode, message);
        }

        /// <summary>
        /// Sets the size of the text according to the parameter value, given in pixels.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="size">Size.</param>
        public static string Sized(this string message, int size)
        {
            return string.Format("<size={0}>{1}</size>", size, message);
        }

        /// <summary>
        /// Renders the text in boldface.
        /// </summary>
        /// <param name="message">Message.</param>
        public static string Bold(this string message)
        {
            return string.Format("<b>{0}</b>", message);
        }

        /// <summary>
        /// Renders the text in italics.
        /// </summary>
        /// <param name="message">Message.</param>
        public static string Italics(this string message)
        {
            return string.Format("<i>{0}</i>", message);
        }

        public static bool TryParseVector3(this string input, out Vector3 vector3)
        {
            return Vector3Utils.TryParse(input, out vector3);
        }

        public static byte[] ToUTF8Bytes_NullTerminated(this string content)
        {
            int byteCount = Encoding.UTF8.GetByteCount(content);
            byte[] bytes = new byte[byteCount + 1];
            Encoding.UTF8.GetBytes(content, 0, byteCount, bytes, 0);
            bytes[byteCount] = 0;
            return bytes;
        }
    }
}
