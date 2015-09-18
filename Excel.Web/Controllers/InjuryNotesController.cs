using System.Linq;
using System.Web.Mvc;
using Excel.Web.DataContexts;

namespace Excel.Web.Controllers
{
    public class InjuryNotesController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: InjuryNotes
        public ActionResult Index()
        {
            var notes = db.InjuryNotes.ToList();
            var allNotes = from n in notes
                           select n;
            return View(allNotes.OrderByDescending(n => n.NoteDate));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
