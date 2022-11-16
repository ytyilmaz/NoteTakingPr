using NoteTaking.Services.Models;
namespace NoteTaking.Services.Note;

public interface INoteService
{
    Task<bool> AddNoteAsync(NoteVM obj);
    Task<NoteVM> FindNoteAsync(int? id);
    Task<IEnumerable<NoteVM>> GetNotesAsync(string userId);
    Task<bool> RemoveNoteAsync(NoteVM tempNote);
    Task<bool> UpdateNoteAsync(NoteVM obj);
}
