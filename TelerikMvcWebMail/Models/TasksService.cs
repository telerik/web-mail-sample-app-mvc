using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class TasksService
    {
        private WebMailEntities entities;

        private static bool UpdateDatabase = true;

        public TasksService(WebMailEntities entities)
        {
            this.entities = entities;
        }

        public IList<TaskViewModel> GetAll()
        {
            IList<TaskViewModel> result = HttpContext.Current.Session["Tasks"] as IList<TaskViewModel>;

            if (result == null || UpdateDatabase)
            {
                result = entities.Notes.Select(e => new TaskViewModel
                {
                    Category = e.Category,
                    CreatedOn = e.DateCreated,
                    Id = e.Id,
                    Subject = e.Subject,
                    Content = e.NoteContent
                }).ToList();

                if (!UpdateDatabase)
                {
                    HttpContext.Current.Session["Tasks"] = result;
                }
            }

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

            if (!UpdateDatabase)
            {
                var first = GetAll().OrderByDescending(e => e.Id).FirstOrDefault();
                var id = (first != null) ? first.Id : 0;

                task.Id = id + 1;

                GetAll().Insert(0, task);
            }
            else
            {
                var entity = task.ToEntity();
                entity.DateCreated = DateTime.Now;

                entities.Notes.Add(entity);
                entities.SaveChanges();

                task.Id = entity.Id;
            }
        }

        public void Update(TaskViewModel task)
        {
            if (string.IsNullOrEmpty(task.Subject))
            {
                task.Subject = "Changed task";
            }

            if (!UpdateDatabase)
            {
                var target = One(e => e.Id == task.Id);

                if (target != null)
                {
                    target.Category = task.Category;
                    target.Subject = task.Subject;
                    target.Content = task.Content;
                }
            }
            else
            {
                var entity = task.ToEntity();
                entities.Notes.Attach(entity);
                entities.Entry(entity).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        public void Delete(TaskViewModel task)
        {
            if (!UpdateDatabase)
            {
                var target = One(p => p.Id == task.Id);
                if (target != null)
                {
                    GetAll().Remove(target);
                }
            }
            else
            {
                var entity = task.ToEntity();
                entities.Notes.Attach(entity);
                entities.Notes.Remove(entity);
                entities.SaveChanges();
            }
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