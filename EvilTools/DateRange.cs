using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvilTools {
    /// <summary>
    /// This is a tool used to help sort objects within a date range. Provided
    /// a DateTime object, it will check whether it is within the range set
    /// by the constructor arguments or not.
    /// Furthermore, it can also be used to tell whether a particular date
    /// matches. 
    /// Calling functions then can decide what to do depending on whether
    /// the DateTime object matched the criteria or not.
    /// It does not compare time, only dates
    /// </summary>
    public class DateRangeSort {

        public enum sortType {
            range,
            day
        }

        /// <summary>
        /// From which date the date will be considered within range
        /// </summary>
        private DateTime rangeStart;
        /// <summary>
        /// Up to which date the date will be considered within range
        /// </summary>
        private DateTime rangeEnd;
        /// <summary>
        /// If trying to match a date with another date
        /// </summary>
        private DateTime onDay;
        /// <summary>
        /// The date to evaluate
        /// </summary>
        private DateTime dateEv;
        /// <summary>
        /// What type of sorting the object has been set to 
        /// </summary>
        private sortType action;

        /// <summary>
        /// Sets or gets the action for the current object. It is a
        /// sortType enum that can be either range (check if a date is
        /// within the range set by RangeStart and RangeEnd) or day (it 
        ///  compares two dates and returns true if they are the same)
        /// </summary>
        public sortType Action {
            get {
                return action;
            }
            set {
                action = value;
            }
        }

        /// <summary>
        /// Set the initial date for the range. It will actually create a new
        /// object taking the year, month and day, so time is ignored
        /// </summary>
        public DateTime RangeStart {
            get {
                return rangeStart;
            }
            set {
                this.rangeStart = 
                    new DateTime(value.Year, value.Month, value.Day);
            }
        }
        /// <summary>
        /// Set the final date for the range. It will actually create a new
        /// object taking the year, month and day, so time is ignored
        /// </summary>
        public DateTime RangeEnd {
            get {
                return rangeEnd;
            }
            set {
                this.rangeEnd =
                    new DateTime(value.Year, value.Month, value.Day);
            }
        }
        /// <summary>
        /// Set the date for single date comparisons. It will actually create a new
        /// object taking the year, month and day, so time is ignored
        /// </summary>
        public DateTime OnDay {
            get {
                return onDay;
            }
            set {
                this.onDay =
                    new DateTime(value.Year, value.Month, value.Day);
            }
        }

        /// <summary>
        /// Create object to match against a date range. The range limits are
        /// inclusive: a date that matches the start or end date will return
        /// true
        /// </summary>
        /// <param name="start">From which date we're matching it</param>
        /// <param name="end">Up to which date we're matching it</param>
        public DateRangeSort(DateTime start, DateTime end) {
            RangeStart = start;
            RangeEnd = end;
            action = sortType.range;
        }
        /// <summary>
        /// Create object to match a particular date
        /// </summary>
        /// <param name="day"></param>
        public DateRangeSort(DateTime day) {
            OnDay = day;
            action = sortType.day;
        }

        /// <summary>
        /// Evaluates the date passed based on the arguments and type of
        /// comparison (date range or particular date) set when constructing
        /// the object
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool EvaluateDate(DateTime date) {
            this.dateEv = new DateTime(date.Year, date.Month, date.Day);
            if (action == sortType.day) {
                return MatchDay();
            }
            return MatchRange();
        }

        /// <summary>
        /// Evaluate whether the date being evaluated and onDay are the same
        /// date
        /// </summary>
        /// <returns></returns>
        private bool MatchDay() {
            if (dateEv == null) {
                return false;
            }
            
            return (dateEv.CompareTo(onDay) == 0);
        }

        /// <summary>
        /// Evalutes if dateEv is within the range start - end
        /// </summary>
        /// <returns></returns>
        private bool MatchRange() {
            // If range end after range start, always returns false!
            if (rangeEnd.CompareTo(rangeStart) < 0) {
                return false;
            }
            return (dateEv.CompareTo(rangeStart) >= 0 && dateEv.CompareTo(rangeEnd) <= 0);
        }
    }
}