using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util
{
    /*
     * https://www.cnblogs.com/runningsmallguo/p/10962414.html
     *  //EnumTricks.IsVolumeHigh((Volume)27);

            //var tmp = Enum.IsDefined(typeof(Volume), 3);
            //var str = EnumTricks.EnumToString((Volume)27);
            //var str2 = EnumTricks.EnumToString((Volume)3);


            //Console.WriteLine($"Volume 27:{str}");
            //Console.WriteLine($"Volume 3:{str2}");

            Console.WriteLine("------------------------------------------------------------");
            
            Console.WriteLine(Volume.High.Value);
            Console.WriteLine(Volume.High.DisplayName);

            var volume = Volume.From(2);
            var volume2 = Volume.FromName("high");
            var none = Volume.From(27);

     */
    public class Enumeration : IComparable
    {
        private readonly int _value;
        private readonly string _displayName;

        protected Enumeration()
        {
        }

        protected Enumeration(int value, string displayName)
        {
            _value = value;
            _displayName = displayName;
        }

        public int Value
        {
            get { return _value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration, new()
        {
            var matchingItem = parse<T, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        public int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration)other).Value);
        }
    }
    public class Volume : Enumeration
    {
        private Volume() { throw new Exception(""); }
        private Volume(int value, string displayName) : base(value, displayName) { }


        public static readonly Volume Low = new Volume(1, nameof(Low).ToLowerInvariant());
        public static readonly Volume Medium = new Volume(2, nameof(Medium).ToLowerInvariant());
        public static readonly Volume High = new Volume(3, nameof(High).ToLowerInvariant());


        public static IEnumerable<Volume> List() =>
            new[] { Low, Medium, High };

        public static Volume From(int value)
        {
            var state = List().SingleOrDefault(s => s.Value == value);

            if (state == null)
            {
                throw new Exception($"Possible values for Volume: {String.Join(",", List().Select(s => s.Value))}");
            }

            return state;
        }

        public static Volume FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.DisplayName, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for Volume: {String.Join(",", List().Select(s => s.DisplayName))}");
            }

            return state;
        }


    }
}
