using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TelerikMvcWebMail.Models
{
    public class EventsService : ISchedulerEventService<EventViewModel>
    {
        //private static bool UpdateDatabase = false;
        private WebMailEntities1 entities;

        public EventsService(WebMailEntities1 entities)
        {
            this.entities = entities;
        }

        public IList<EventViewModel> GetAll()
        {
            IList<EventViewModel> result = new List<EventViewModel>();

            result = entities.Events.Select(e => new EventViewModel
            {
                TaskID = e.TaskID,
                Title = e.Title,
                Start = e.Start,
                End = e.End,
                RecurrenceRule = e.RecurrenceRule,
                RecurrenceID = e.RecurrenceID,
                RecurrenceException = e.RecurrenceException,
                StartTimezone = e.StartTimezone,
                EndTimezone = e.EndTimezone,
                OwnerID = e.OwnerID,
                Description = e.Description,
                IsAllDay = e.IsAllDay
            }).ToList();

            return result;
        }

        public IEnumerable<EventViewModel> Read()
        {
            return GetAll();
        }

        public void Insert(EventViewModel appointment, ModelStateDictionary modelState)
        {
            if (ValidateModel(appointment, modelState))
            {

                if (string.IsNullOrEmpty(appointment.Title))
                {
                    appointment.Title = "";
                }

                var entity = appointment.ToEntity();

                entities.Events.Add(entity);
                entities.SaveChanges();

                appointment.TaskID = entity.TaskID;

            }
        }

        public void Update(EventViewModel appointment, ModelStateDictionary modelState)
        {
            if (ValidateModel(appointment, modelState))
            {
                if (string.IsNullOrEmpty(appointment.Title))
                {
                    appointment.Title = "";
                }

                var entity = appointment.ToEntity();
                entities.Events.Attach(entity);
                entities.Entry(entity).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        public void Delete(EventViewModel appointment, ModelStateDictionary modelState)
        {
            var entity = appointment.ToEntity();
            entities.Events.Attach(entity);

            // Change id to take int value!!!!!
            //var recurrenceExceptions = entities.Events.Where(t => t.RecurrenceID == appointment.TaskID);

            //foreach (var recurrenceException in recurrenceExceptions)
            //{
            //    db.Tasks.Remove(recurrenceException);
            //}

            entities.Events.Remove(entity);
            entities.SaveChanges();
        }

        private bool ValidateModel(EventViewModel appointment, ModelStateDictionary modelState)
        {
            if (appointment.Start > appointment.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to Start date.");
                return false;
            }

            return true;
        }

        public EventViewModel One(Func<EventViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}