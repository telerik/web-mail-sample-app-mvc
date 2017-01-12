using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TelerikMvcWebMail.Models
{
    public class NotesService
    {
        //private static bool UpdateDatabase = false;
        private WebMailEntities entities;

        public NotesService(WebMailEntities entities)
        {
            this.entities = entities;
        }

        public IList<NoteViewModel> GetAll()
        {
            IList<NoteViewModel> result = new List<NoteViewModel>();

            result = entities.Notes.Select(e => new NoteViewModel
            {
                Category = e.Category,
                CreatedOn = e.DateCreated,
                Id = e.Id,
                Subject = e.Subject,
                Content = e.NoteContent
            }).ToList();

            return result;
        }

        public IEnumerable<NoteViewModel> Read()
        {
            return GetAll();
        }

        public void Insert(NoteViewModel note)
        {
            if (string.IsNullOrEmpty(note.Subject))
            {
                note.Subject = "New note";
            }

            var entity = note.ToEntity();
            entity.DateCreated = DateTime.Now;

            entities.Notes.Add(entity);
            entities.SaveChanges();

            note.Id = entity.Id;
        }

        public void Update(NoteViewModel note)
        {
            if (string.IsNullOrEmpty(note.Subject))
            {
                note.Subject = "Changed note";
            }

            var entity = note.ToEntity();
            entities.Notes.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Delete(NoteViewModel note)
        {
            var entity = new Note();
            entity.Id = note.Id;
            entities.Notes.Attach(entity);
            entities.Notes.Remove(entity);
            entities.SaveChanges();
        }

        public NoteViewModel One(Func<NoteViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}