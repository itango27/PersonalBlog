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
    public class ProjectTodosController : Controller
    {
        private readonly PersonalBlogContext _context;

        public ProjectTodosController(PersonalBlogContext context)
        {
            _context = context;
        }

        // GET: ProjectTodos
        public async Task<IActionResult> Index()
        {
            var personalBlogContext = _context.ProjectTodos.Include(p => p.Project);
            return View(await personalBlogContext.ToListAsync());
        }

        // GET: ProjectTodos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTodo = await _context.ProjectTodos
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectTodo == null)
            {
                return NotFound();
            }

            return View(projectTodo);
        }

        // GET: ProjectTodos/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        // POST: ProjectTodos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,Title,IsCompleted")] ProjectTodo projectTodo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectTodo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", projectTodo.ProjectId);
            return View(projectTodo);
        }

        // GET: ProjectTodos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTodo = await _context.ProjectTodos.FindAsync(id);
            if (projectTodo == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", projectTodo.ProjectId);
            return View(projectTodo);
        }

        // POST: ProjectTodos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectId,Title,IsCompleted")] ProjectTodo projectTodo)
        {
            if (id != projectTodo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTodo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTodoExists(projectTodo.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", projectTodo.ProjectId);
            return View(projectTodo);
        }

        // GET: ProjectTodos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTodo = await _context.ProjectTodos
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectTodo == null)
            {
                return NotFound();
            }

            return View(projectTodo);
        }

        // POST: ProjectTodos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectTodo = await _context.ProjectTodos.FindAsync(id);
            if (projectTodo != null)
            {
                _context.ProjectTodos.Remove(projectTodo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTodoExists(int id)
        {
            return _context.ProjectTodos.Any(e => e.Id == id);
        }
    }
}
