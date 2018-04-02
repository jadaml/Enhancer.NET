using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Collections;

namespace Enhancer
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Multiplies the size of the current rectangle's x and y size by the specified scalar.
        /// </summary>
        /// <param name="rect">The current rectangle.</param>
        /// <param name="scalar">The scalar to scale the x and y sizes by.</param>
        /// <returns>The scaled rectangle rectangle.</returns>
        public static Rect Scale(this Rect rect, double scalar)
        {
            rect.Scale(scalar, scalar);
            return rect;
        }

        private static Action EmptyDelegate = delegate() { ; };

        public static void Refresh(this UIElement uiElement)
        {
            Refresh(uiElement, EmptyDelegate);
        }

        public static void Refresh(this UIElement uiElement, Action method, params object[] args)
        {
            uiElement.Dispatcher.Invoke(method, DispatcherPriority.Render, args);
        }

        /// <summary>
        /// Adds many amount of the same function to the collection.
        /// </summary>
        /// <typeparam name="TSource">The base type of the collection</typeparam>
        /// <param name="collection">The collection to add to
        /// (You can also call this function as if would be defined in this object.)</param>
        /// <param name="function">The object to add to the collection</param>
        /// <param name="amount">The amount of times to add the object to the collection</param>
        public static void Add<TSource>(this ICollection<TSource> collection, TSource value, int amount)
        {
            Add(collection, delegate() { return value; }, amount);
        }

        public static void Add<TSource>(this ICollection<TSource> collection, Func<TSource> function, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                collection.Add(function.Invoke());
            }
        }

        public static void Add(this IList list, object value, int amount)
        {
            Add(list, delegate() { return value; }, amount);
        }

        public static void AddMany(this IList list, Type type, int amount)
        {
            AddMany(list, type, new Type[] { }, amount);
        }

        public static void AddMany(this IList list, Type type, Type[] signature, int amount, params object[] args)
        {
            Add(list, delegate() { return type.GetConstructor(signature).Invoke(args); }, amount);
        }

        public static void Add(this IList list, Func<object> function, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                list.Add(function.Invoke());
            }
        }

        /// <summary>
        /// Determines if a nullable boolean is set and it is true.
        /// </summary>
        /// <param name="l">The nullable boolean</param>
        /// <returns><c>true</c> if it is set and it is true, <c>false</c> otherwise</returns>
        public static bool IsTrue(this bool? l)
        {
            return l.HasValue && l.Value;
        }

        /// <summary>
        /// Determines if a nullable boolean is set and it is false.
        /// </summary>
        /// <param name="l">The nullable boolean</param>
        /// <returns><c>true</c> if it is set and it is false, <c>false</c> otherwise</returns>
        public static bool IsFalse(this bool? l)
        {
            return l.HasValue && !l.Value;
        }

        public enum ResizeRule {
            None,
            MinSize,
            MaxSize,
            FixedSize,
        }

        /// <summary>
        /// Resizes a WPF Window to fit it's childrens size.
        /// </summary>
        /// <param name="window">The window that should be resized</param>
        public static void ResizeToContent(this Window window)
        {
            ResizeToContent(window, ResizeRule.None);
        }

        public static void ResizeToContent(this Window window, ResizeRule rule)
        {
            SizeToContent __preSet = window.SizeToContent;
            WindowState __preState = window.WindowState;
            window.WindowState = WindowState.Normal;
            window.SizeToContent = SizeToContent.WidthAndHeight;
            switch (rule)
            {
                case ResizeRule.MinSize:
                    window.MinWidth = window.Width;
                    window.MinHeight = window.Height;
                    break;
                case ResizeRule.MaxSize:
                    window.MaxWidth = window.Width;
                    window.MaxHeight = window.Height;
                    break;
                case ResizeRule.FixedSize:
                    window.MinWidth = window.MaxWidth = window.Width;
                    window.MinHeight = window.MaxHeight = window.Height;
                    break;
            }
            window.SizeToContent = __preSet;
            window.WindowState = __preState;
        }

        /// <summary>
        /// Converts the value of the current <see cref="TimeSpan"/> object to its equivalent string representation
        /// by using the specified format, with some of its format specified being optional.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> object to convert.</param>
        /// <param name="format">The format specifier.</param>
        /// <returns>The string representation of the <see cref="TimeSpan"/> object.</returns>
        /// <remarks>
        /// <para>
        /// With this method, you can specifies some specifier being optional, if the specified field otherwise 0.
        /// </para>
        /// <code>
        /// string format = "[h][h]':'mm':'ss";
        /// Console.WriteLine((new TimeSpan(0, 32, 16)).FormatToString(format));
        /// // Outputs: :32:16
        /// Console.WriteLine((new TimeSpan(5, 42, 27)).FormatToString(format));
        /// // Outputs: 05:42:27
        /// </code>
        /// </remarks>
        public static string FormatToString(this TimeSpan timeSpan, string format)
        {
            StringBuilder _format = new StringBuilder();
            int start = -1, end = -1, last = 0;
            while ((start = format.IndexOf('[', start + 1)) >= 0 && (end = format.IndexOf(']', end + 1)) >= 0)
            {
                _format.Append(format, last, start - last);
                bool append;

                switch (format.Substring(start + 1, end - start - 1).LastSpecifierChar(c => "dhmsf".Contains(c)))
                {
                    case '-':
                        append = timeSpan.TotalDays < 0;
                        break;
                    case 'd':
                        append = Math.Abs(timeSpan.TotalDays) >= 1;
                        break;
                    case 'h':
                        append = Math.Abs(timeSpan.TotalHours) >= 1;
                        break;
                    case 'm':
                        append = Math.Abs(timeSpan.TotalMinutes) >= 1;
                        break;
                    case 's':
                        append = Math.Abs(timeSpan.TotalSeconds) >= 1;
                        break;
                    case 'f':
                        append = Math.Abs(timeSpan.Milliseconds) >= 1;
                        break;
                    default:
                        append = false;
                        break;
                }

                if (append)
                    _format.Append(format, start + 1, end - start - 1);
                last = end + 1;
            }
            _format.Append(format, last, format.Length - last);
            return timeSpan.ToString(_format.ToString());
        }

        // TODO
        internal static char FirstSpecifierChar(this string str, char c, char d = '\0')
        {
            return FirstSpecifierChar(str, _c => _c == c, d);
        }

        // TODO
        internal static char FirstSpecifierChar(this string str, Predicate<char> sel, char d = '\0')
        {
            bool inStr = false, esc = false;
            foreach (char c in str)
            {
                switch (c)
                {
                    case '\'':
                        inStr = !inStr;
                        continue;

                    case '\\':
                        esc = !esc;
                        if (esc)
                            continue;
                        else
                            break;
                }

                // TODO: Can it be enhanced further?
                if (inStr) continue;
                if (!esc && sel(c)) return c;
                if (esc) esc = false;
            }
            return d;
        }

        // TODO
        internal static char LastSpecifierChar(this string str, char c, char d = '\0')
        {
            return LastSpecifierChar(str, _c => _c == c, d);
        }

        // TODO
        internal static char LastSpecifierChar(this string str, Predicate<char> sel, char d = '\0')
        {
            bool inStr = false, esc = false;
            char retval = d;
            foreach (char c in str)
            {
                switch (c)
                {
                    case '\'':
                        inStr = !inStr;
                        continue;

                    case '\\':
                        esc = !esc;
                        if (esc)
                            continue;
                        else
                            break;
                }

                // TODO: Can it be enhanced further?
                if (inStr) continue;
                if (!esc && sel(c)) retval = c;
                if (esc) esc = false;
            }
            return retval;
        }
    }
}
