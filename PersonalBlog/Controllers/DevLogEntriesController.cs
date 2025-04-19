using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;

namespace PersonalBlog.Controllers
{
    public class DevLogEntriesController : Controller
    {
        private readonly PersonalBlogContext _context;

        public DevLogEntriesController(PersonalBlogContext context)
        {
            _context = context;
        }

        // GET: DevLogEntries
        public async Task<IActionResult> Index()
        {
            var personalBlogContext = _context.DevLogEntries.Include(d => d.Project);
            return View(await personalBlogContext.ToListAsync());
        }

        // GET: DevLogEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devLogEntry = await _context.DevLogEntries
                .Include(d => d.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devLogEntry == null)
            {
                return NotFound();
            }

            return View(devLogEntry);
        }

        // GET: DevLogEntries/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        // POST: DevLogEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Title,Summary,Content,ProjectId")] DevLogEntry devLogEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(devLogEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", devLogEntry.ProjectId);
            return View(devLogEntry);
        }

        // GET: DevLogEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devLogEntry = await _context.DevLogEntries.FindAsync(id);
            if (devLogEntry == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", devLogEntry.ProjectId);
            return View(devLogEntry);
        }

        // POST: DevLogEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Title,Summary,Content,ProjectId")] DevLogEntry devLogEntry)
        {
            if (id != devLogEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devLogEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevLogEntryExists(devLogEntry.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", devLogEntry.ProjectId);
            return View(devLogEntry);
        }

        // GET: DevLogEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devLogEntry = await _context.DevLogEntries
                .Include(d => d.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devLogEntry == null)
            {
                return NotFound();
            }

            return View(devLogEntry);
        }

        // POST: DevLogEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var devLogEntry = await _context.DevLogEntries.FindAsync(id);
            if (devLogEntry != null)
            {
                _context.DevLogEntries.Remove(devLogEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevLogEntryExists(int id)
        {
            return _context.DevLogEntries.Any(e => e.Id == id);
        }
    }
}
