using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTaking.DataAccess;
using NoteTaking.Services.Models;
using NoteTaking.Services.Note;
using System.Security.Claims;
namespace NoteTakingWeb.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly INoteService noteService;

        /// <summary>
        /// construtor of the note controller
        /// </summary>
        /// <param name="noteService"></param>
        public NoteController(ApplicationDbContext db, INoteService noteService)
        {
            this.noteService = noteService;
        }

        /// <summary>
        /// GET - Presentation of the notes for the current user
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET - Insert the new record via new button
        /// or edit the selected record via edit button for the selected note record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Upsert(int? id)
        {
            NoteVM noteObj = new();

            //Update : else new insert
            if (id != null && id != 0)
            {
                NoteVM tempNote = await noteService.FindNoteAsync(id);

                if (tempNote != null)
                    noteObj = tempNote;
            }
            return View(noteObj);
        }

        /// <summary>
        /// POST - Insert the new record via new button
        /// or edit the selected record via edit button for the selected note record.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Upsert(NoteVM obj)
        {
            string? userId = GetUserId();
            if (ModelState.IsValid && userId != null)
            {
                obj.UserId = userId;
                if (obj.Id == 0)
                {
                    var result = await noteService.AddNoteAsync(obj);
                    TempData["success"] = "Note created successfully";
                }
                else
                {
                    obj.UpdatedDateTime = DateTime.Now;
                    var result = await noteService.UpdateNoteAsync(obj);
                    TempData["success"] = "Note updated successfully";
                }
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS

        /// <summary>
        /// Retrieve all the records of the current user to the table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string? userId = GetUserId();
            var noteList = await noteService.GetNotesAsync(userId);
            return Json(new { data = noteList });
        }

        private string? GetUserId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        /// <summary>
        /// POST - Delete the selected note record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            NoteVM tempNote = await noteService.FindNoteAsync(id);
            if (tempNote == null)
            {
                return Json(new { success = false, message = "Error while deleting" }); ;
            }
            var removeSuccessful = await noteService.RemoveNoteAsync(tempNote);
            return Json(new { success = removeSuccessful, message = removeSuccessful ? "Deleted successfully." : "Error while deleting" }); ;
        }
        #endregion

    }
}
