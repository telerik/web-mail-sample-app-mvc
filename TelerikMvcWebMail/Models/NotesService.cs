using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class TasksService
    {
        //private static bool UpdateDatabase = false;
        private WebMailEntities entities;

        public TasksService(WebMailEntities entities)
        {
            this.entities = entities;
        }

        public IList<TaskViewModel> GetAll()
        {
            IList<TaskViewModel> result = new List<TaskViewModel>();

            result = entities.Notes.Select(e => new TaskViewModel
            {
                Category = e.Category,
                CreatedOn = e.DateCreated,
                Id = e.Id,
                Subject = e.Subject,
                Content = e.NoteContent
            }).ToList();

            return result;
        }

        public IEnumerable<TaskViewModel> Read()
        {
            return GetAll();
        }

        public void Insert(TaskViewModel task)
        {
            if (string.IsNullOrEmpty(task.Subject))
            {
                task.Subject = "New task";
            }

            var entity = task.ToEntity();
            entity.DateCreated = DateTime.Now;

            entities.Notes.Add(entity);
            entities.SaveChanges();

            task.Id = entity.Id;
        }

        public void Update(TaskViewModel task)
        {
            if (string.IsNullOrEmpty(task.Subject))
            {
                task.Subject = "Changed task";
            }

            var entity = task.ToEntity();
            entities.Notes.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Delete(TaskViewModel task)
        {
            var entity = new Note();
            entity.Id = task.Id;
            entities.Notes.Attach(entity);
            entities.Notes.Remove(entity);
            entities.SaveChanges();
        }

        public TaskViewModel One(Func<TaskViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}