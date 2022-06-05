using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationManagment.Data;
using VacationManagment.Models;

namespace VacationManagment.Controllers
{
    [Authorize(Policy = "Admin Policy")]
    public class VacationTypeController : Controller
    {
        private readonly VacationDbContext _context;

        public VacationTypeController(VacationDbContext context)
        {
            _context = context;
        }
        public IActionResult VacationType()
        {
            return View(_context.VacationTypes.OrderBy(x=> x.VacationName).ToList());
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VacationType model)
        {
            if (ModelState.IsValid)
            {                                                                                // Trim => for remove spaces
                var result = _context.VacationTypes.FirstOrDefault(x=> x.VacationName.Contains(model.VacationName.Trim()));
                if (result == null)
                {
                    _context.VacationTypes.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(VacationType));
                }
                ViewBag.ErrorMsg = false;
            }
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var chick = _context.VacationTypes.FirstOrDefault(x => x.Id == id);
            return View(chick);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VacationType model)
        {
            if (ModelState.IsValid)
            {
                _context.VacationTypes.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(VacationType));
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var Chick = _context.VacationTypes.FirstOrDefault(x => x.Id == id);
            if (Chick == null)
            {
                return NotFound();
            }
            _context.VacationTypes.Remove(Chick);
            _context.SaveChanges();
            return RedirectToAction(nameof(VacationType));
        }










        //[HttpPost("[Controller]/Delete/{id}")]
        //public IActionResult postDelete(int id)
        //{
        //    var cick = _context.VacationTypes.FirstOrDefault(x => x.Id == id);
        //    if (cick == null)
        //    {
        //        return NotFound();
        //    }
        //    _context.VacationTypes.Remove(cick);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
