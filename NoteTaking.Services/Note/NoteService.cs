using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteTaking.DataAccess;
using NoteTaking.Services.Models;
using System.Reflection.Metadata.Ecma335;

namespace NoteTaking.Services.Note
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="mapper"></param>
        public NoteService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        /// <summary>
        /// Add a new note
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<bool> AddNoteAsync(NoteVM obj)
        {
            var note = mapper.Map<NoteVM, DataAccess.Note>(obj);
            db.Notes.Add(note);
            await db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Find the selected note
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<NoteVM> FindNoteAsync(int? id)
        {
            if (id != null)
            {
                var tempNote = await db.Notes.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
                if (tempNote != null)
                    return mapper.Map<DataAccess.Note, NoteVM>(tempNote);
            }
            throw new ArgumentNullException(nameof(id));
        }

        /// <summary>
        /// Get all notes for the selected user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IEnumerable<NoteVM>> GetNotesAsync(string userId)
        {
            if (userId != null)
            {
                var noteList = await db.Notes.Where(x => x.UserId == userId).ToListAsync();
                return noteList.Select((a) =>
                {
                    return mapper.Map<DataAccess.Note, NoteVM>(a);
                });
            }
            throw new ArgumentNullException(nameof(userId));
        }

        /// <summary>
        /// Remove the selected note
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<bool> RemoveNoteAsync(NoteVM obj)
        {
            var note = mapper.Map<NoteVM, DataAccess.Note>(obj);
            db.Notes.Remove(note);
            await db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Update the selected note
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<bool> UpdateNoteAsync(NoteVM obj)
        {
            var note = mapper.Map<NoteVM, DataAccess.Note>(obj);
            db.Notes.Update(note);
            await db.SaveChangesAsync();
            return true;
        }
    }
}