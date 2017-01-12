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
        private static bool UpdateDatabase = true;
        
        private WebMailEntities entities;

        public EventsService(WebMailEntities entities)
        {
            this.entities = entities;
        }

        public IList<EventViewModel> GetAll()
        {
            IList<EventViewModel> result = HttpContext.Current.Session["Events"] as IList<EventViewModel>;

            if (result == null || UpdateDatabase)
            {
                result = entities.Events.ToList().Select(task => new EventViewModel
                {
                    TaskID = task.TaskID,
                    Title = task.Title,
                    Start = DateTime.SpecifyKind(task.Start, DateTimeKind.Utc),
                    End = DateTime.SpecifyKind(task.End, DateTimeKind.Utc),
                    StartTimezone = task.StartTimezone,
                    EndTimezone = task.EndTimezone,
                    Description = task.Description,
                    IsAllDay = task.IsAllDay,
                    RecurrenceRule = task.RecurrenceRule,
                    RecurrenceException = task.RecurrenceException,
                    RecurrenceID = task.RecurrenceID,
                    OwnerID = task.OwnerID
                }).ToList();

                if (!UpdateDatabase)
                {
                    HttpContext.Current.Session["Events"] = result;
                }
            }

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

                if (!UpdateDatabase)
                {
                    var first = GetAll().OrderByDescending(e => e.TaskID).FirstOrDefault();
                    var id = (first != null) ? first.TaskID : 0;

                    appointment.TaskID = id + 1;

                    GetAll().Insert(0, appointment);
                }
                else
                {
                    var entity = appointment.ToEntity();

                    entities.Events.Add(entity);
                    entities.SaveChanges();

                    appointment.TaskID = entity.TaskID;
                }
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

                if (!UpdateDatabase)
                {
                    var target = One(e => e.TaskID == appointment.TaskID);

                    if (target != null)
                    {
                        target.Title = appointment.Title;
                        target.Start = appointment.Start;
                        target.End = appointment.End;
                        target.StartTimezone = appointment.StartTimezone;
                        target.EndTimezone = appointment.EndTimezone;
                        target.Description = appointment.Description;
                        target.IsAllDay = appointment.IsAllDay;
                        target.RecurrenceRule = appointment.RecurrenceRule;
                        target.RecurrenceException = appointment.RecurrenceException;
                        target.RecurrenceID = appointment.RecurrenceID;
                        target.OwnerID = appointment.OwnerID;
                    }
                }
                else
                {
                    var entity = appointment.ToEntity();
                    entities.Events.Attach(entity);
                    entities.Entry(entity).State = EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

        public void Delete(EventViewModel appointment, ModelStateDictionary modelState)
        {
            if (!UpdateDatabase)
            {
                var target = One(p => p.TaskID == appointment.TaskID);
                if (target != null)
                {
                    GetAll().Remove(target);

                    var recurrenceExceptions = GetAll().Where(m => m.RecurrenceID == appointment.TaskID).ToList();

                    foreach (var recurrenceException in recurrenceExceptions)
                    {
                        GetAll().Remove(recurrenceException);
                    }
                }
            }
            else
            {
                var entity = appointment.ToEntity();
                entities.Events.Attach(entity);

                var recurrenceExceptions = entities.Events.Where(t => t.RecurrenceID == appointment.TaskID);

                foreach (var recurrenceException in recurrenceExceptions)
                {
                    entities.Events.Remove(recurrenceException);
                }

                entities.Events.Remove(entity);
                entities.SaveChanges();
            }
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