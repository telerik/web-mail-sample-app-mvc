using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;

namespace TelerikMvcWebMail.Models
{
    public class EventViewModel : Kendo.Mvc.UI.ISchedulerEvent
    {
        public int TaskID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        private DateTime start;
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

        public string StartTimezone { get; set; }

        private DateTime end;
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }

        public string EndTimezone { get; set; }

        public string RecurrenceRule { get; set; }

        public int? RecurrenceID { get; set; }

        public string RecurrenceException { get; set; }

        public bool IsAllDay { get; set; }

        public int? Category { get; set; }

        private bool isAllDay;
        bool ISchedulerEvent.IsAllDay
        {
            get
            {
                return this.isAllDay;
            }

            set
            {
                this.isAllDay = value;
            }
        }

        public Event ToEntity()
        {
            return new Event
            {
                EventID = TaskID,
                Title = Title,
                Start = Start,
                End = End,
                Description = Description,
                RecurrenceRule = RecurrenceRule,
                RecurrenceException = RecurrenceException,
                StartTimezone = StartTimezone,
                EndTimezone = EndTimezone,
                RecurrenceID = RecurrenceID,
                IsAllDay = IsAllDay,
                Category = Category
            };
        }
    }
}