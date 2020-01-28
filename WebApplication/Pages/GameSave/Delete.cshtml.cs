using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApplication.Pages.GameSave
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.GameSaveDbContext _context;

        public DeleteModel(DAL.GameSaveDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Domain.GameSave GameSave { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GameSave = await _context.GameSave.FirstOrDefaultAsync(m => m.GameSaveId == id);

            if (GameSave == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GameSave = await _context.GameSave.FindAsync(id);

            if (GameSave != null)
            {
                _context.GameSave.Remove(GameSave);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
