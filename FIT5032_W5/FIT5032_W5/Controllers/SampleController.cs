using FIT5032_W5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT5032_W5.Controllers
{
    public class SampleController : Controller
    {
        // GET: SampleController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SampleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult Create()
        {
            return View();
        }

        // POST: SampleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormOneViewModel model)
        {
            try
            {
                String FirstName = model.FirstName;
                String LastName = model.LastName;
                ViewBag.FullName = FirstName + " " + LastName;
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: SampleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SampleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SampleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SampleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
