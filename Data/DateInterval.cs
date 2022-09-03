using System.Globalization;
using SmallNetUtils.DateTimeUtils;

namespace SmallNetUtils.Data
{
    /// <summary>
    /// DateTime interval
    /// </summary>
    [Serializable]
    public struct DateInterval
    {
        /// <summary>
        /// Empty value
        /// </summary>
        public static readonly DateInterval Empty = new(DateTime.MaxValue, DateTime.MinValue);

        /// <summary>
        /// Start of an interval
        /// </summary>
        private readonly DateTime _begin;

        /// <summary>
        /// Finish of an interval
        /// </summary>
        private readonly DateTime _end;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="begin"> Begin </param>
        /// <param name="end"> End </param>
        public DateInterval(DateTime begin, DateTime end)
        {
            _begin = begin;
            _end = end;
        }

        /// <summary>
        /// Interval begin
        /// </summary>
        public DateTime Begin => _begin;

        /// <summary>
        /// Interval end
        /// </summary>
        public DateTime End => _end;

        /// <summary>
        /// Span between begin and end
        /// </summary>
        public TimeSpan Span => _begin < _end ? _end - _begin : TimeSpan.Zero;

        /// <summary>
        /// Check if interval is Empty
        /// </summary>
        public bool IsEmpty => this == Empty;

        /// <summary>
        /// Overload ==
        /// </summary>
        /// <param name="item1"> First interval </param>
        /// <param name="item2"> Second interval </param>
        /// <returns> Flag if intervals are equal </returns>
        public static bool operator ==(DateInterval item1, DateInterval item2)
        {
            return item1.Begin == item2.Begin && item1.End == item2.End;
        }

        /// <summary>
        /// Overload !=
        /// </summary>
        /// <param name="item1"> First interval </param>
        /// <param name="item2"> Second interval </param>
        /// <returns> Flag if intervals are note equal </returns>
        public static bool operator !=(DateInterval item1, DateInterval item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// Overload And
        /// </summary>
        /// <param name="item1"> First interval </param>
        /// <param name="item2"> Second interval </param>
        /// <returns> Merged interval </returns>
        public static DateInterval operator &(DateInterval item1, DateInterval item2)
        {
            return new DateInterval(
                item1.Begin > item2.Begin ? item1.Begin : item2.Begin,
                item1.End < item2.End ? item1.End : item2.End);
        }

        /// <summary>
        /// Union two intervals (OR)
        /// </summary>
        /// <param name="item1"> First interval </param>
        /// <param name="item2"> Second interval </param>
        /// <returns> Merged interval </returns>
        public static DateInterval operator |(DateInterval item1, DateInterval item2)
        {
            return item1.IsEmpty switch
            {
                true when !item2.IsEmpty => item2,
                false when item2.IsEmpty => item1,
                _ => new DateInterval(
                    new DateTime(Math.Min(item1.Begin.Ticks, item2.Begin.Ticks)),
                    new DateTime(Math.Max(item1.End.Ticks, item2.End.Ticks)))
            };
        }

        /// <summary>
        /// Union an interval and a date (OR)
        /// </summary>
        /// <param name="interval"> Interval </param>
        /// <param name="date"> Date </param>
        /// <returns> Merged interval </returns>
        public static DateInterval operator |(DateInterval interval, DateTime date)
        {
            return new DateInterval(
                new DateTime(Math.Min(interval.Begin.Ticks, date.Ticks)),
                new DateTime(Math.Max(interval.End.Ticks, date.Ticks)));
        }

        /// <summary>
        /// Check for equals
        /// </summary>
        /// <param name="obj"> Second interval </param>
        /// <returns> Flag if equals </returns>
        public override bool Equals(object? obj)
        {
            var item2 = obj as DateInterval? ?? Empty;

            return this == item2;
        }

        /// <summary>
        /// Get hashcode
        /// </summary>
        /// <returns> Hashcode </returns>
        public override int GetHashCode()
        {
            return (_begin.ToString(CultureInfo.InvariantCulture) + _end).GetHashCode();
        }

        /// <summary>
        /// Get string representation
        /// </summary>
        /// <returns> String representation </returns>
        public override string ToString()
        {
            return ToShortDateString();
        }

        /// <summary>
        /// Get short date string representation
        /// </summary>
        /// <returns> String representation </returns>
        public string ToShortDateString()
        {
            return $"{_begin.ToShortDateString()} / {_end.ToShortDateString()}";
        }

        /// <summary>
        /// Check if interval contains another interval
        /// </summary>
        /// <param name="interval2"> Second interval </param>
        /// <param name="ignoreTime"> Flag if check only by date, without time </param>
        /// <returns> Flag if an interval contains a second interval </returns>
        public bool Contains(DateInterval interval2, bool ignoreTime)
        {
            if (ignoreTime)
            {
                return _begin.Date <= interval2.Begin.Date && interval2.End.Date <= _end.Date;
            }

            return _begin <= interval2.Begin && interval2.End <= _end;
        }

        /// <summary>
        /// Check if interval contains DateTime
        /// </summary>
        /// <param name="value"> DateTime value </param>
        /// <param name="ignoreTime"> Flag if check only by date, without time </param>
        /// <returns> Flag if an interval contains a DateTime </returns>
        public bool Contains(DateTime value, bool ignoreTime = false)
        {
            if (ignoreTime)
            {
                return _begin.Date <= value.Date && value.Date <= _end.Date;
            }

            return _begin <= value && value <= _end;
        }

        /// <summary>
        /// Check if two intervals has intersection
        /// </summary>
        /// <param name="interval2"> Second interval </param>
        /// <param name="ignoreTime"> Flag if check only by date, without time </param>
        /// <returns> Flag if two intervals have intersection </returns>
        public bool HasIntersection(DateInterval interval2, bool ignoreTime = false)
        {
            if (ignoreTime)
            {
                return Begin.Date < interval2.End.Date && End.Date > interval2.Begin.Date;
            }

            return Begin < interval2.End && End > interval2.Begin;
        }

        /// <summary>
        /// Get intervals intersection
        /// </summary>
        /// <param name="interval2"> Second interval </param>
        /// <returns> Intersection of two intervals </returns>
        public DateInterval GetIntersection(DateInterval interval2)
        {
            if (HasIntersection(interval2))
            {
                return new DateInterval(
                    new DateTime(Math.Max(Begin.Ticks, interval2.Begin.Ticks)),
                    new DateTime(Math.Min(End.Ticks, interval2.End.Ticks)));
            }

            return Empty;
        }

        /// <summary>
        /// Split interval for intervals based on a IntervalType
        /// </summary>
        /// <param name="size"> Size of intervals </param>
        /// <returns> Splitted DateIntervals </returns>
        public IEnumerable<DateInterval> Split(Microsoft.VisualBasic.DateInterval size)
        {
            var result = new List<DateInterval>();
            var dateBegin = Begin;
            var dateEnd = Begin.AddInterval(size);

            while (dateEnd <= End)
            {
                result.Add(new DateInterval(dateBegin, dateEnd));

                dateBegin = dateEnd;
                dateEnd = dateEnd.AddInterval(size);
            }

            return result;
        }
    }
}