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
    public class IndexModel : PageModel
    {
        private readonly DAL.GameSaveDbContext _context;

        public IndexModel(DAL.GameSaveDbContext context)
        {
            _context = context;
        }

        public IList<Domain.GameSave> GameSave { get;set; }

        public async Task OnGetAsync()
        {
            GameSave = await _context.GameSave.ToListAsync();
        }
    }
}
