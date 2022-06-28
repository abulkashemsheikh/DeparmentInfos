using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeparmentInfos.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DeparmentInfos.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment _webHostEnvironment;
        private IWebHostEnvironment webHostEnvironment;

        public EmployeesController(ApplicationDbContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

   
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employees.Include(e => e.DeparmentInfo);
            return View(await applicationDbContext.ToListAsync());
        }

       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.DeparmentInfo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

       
        public IActionResult Create()
        {
            ViewData["DepartmentInfoID"] = new SelectList(_context.DepartmentInfos, "ID", "ID");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Employee employee)
        {
            string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (employee.Image.Length > 0)
            {
                string filePath = Path.Combine(uploads, employee.Image.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await employee.Image.CopyToAsync(fileStream);
                    employee.ImagePath = employee.Image.FileName;
                }

                if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentInfoID"] = new SelectList(_context.DepartmentInfos, "ID", "ID", employee.DepartmentInfoID);
            return View(employee);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentInfoID"] = new SelectList(_context.DepartmentInfos, "ID", "ID", employee.DepartmentInfoID);
            return View(employee);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Employee employee)
        {
            if (id != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
              
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
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
            ViewData["DepartmentInfoID"] = new SelectList(_context.DepartmentInfos, "ID", "ID", employee.DepartmentInfoID);
            return View(employee);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.DeparmentInfo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
