using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeparmentInfos.Models;

namespace DeparmentInfos.Controllers
{
    public class DepartmentInfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

    
        public  IActionResult Index()

        {
            DeparmentViewModel deparmentView = new DeparmentViewModel();
            deparmentView.departmentInfoVM = _context.DepartmentInfos.AsEnumerable();
            

            return View(deparmentView.departmentInfoVM);
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentInfo = await _context.DepartmentInfos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (departmentInfo == null)
            {
                return NotFound();
            }

            return View(departmentInfo);
        }

     
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
       
        public async Task<IActionResult> Create( DepartmentInfo departmentInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentInfo);
        }

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentInfo = await _context.DepartmentInfos.FindAsync(id);
            if (departmentInfo == null)
            {
                return NotFound();
            }
            return View(departmentInfo);
        }

        
        [HttpPost]
        
        public async Task<IActionResult> Edit(int id, DepartmentInfo departmentInfo)
        {
            if (id != departmentInfo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentInfoExists(departmentInfo.ID))
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
            return View(departmentInfo);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentInfo = await _context.DepartmentInfos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (departmentInfo == null)
            {
                return NotFound();
            }

            return View(departmentInfo);
        }

       
        [HttpPost, ActionName("Delete")]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentInfo = await _context.DepartmentInfos.FindAsync(id);
            _context.DepartmentInfos.Remove(departmentInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentInfoExists(int id)
        {
            return _context.DepartmentInfos.Any(e => e.ID == id);
        }
    }
}
