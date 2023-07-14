using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Data.Repositories;
using MyTicket.Models;
using MyTicket.Static;

namespace MyTicket.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CinemaController : Controller
    {

        private readonly ICinemaRepo cinemaRepo;

        public CinemaController(ICinemaRepo cinemaRepo)
        {
            this.cinemaRepo = cinemaRepo;

        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var cinemas = await cinemaRepo.GetAllAsync();
            return View(cinemas);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Cinema cinema)
        {
            if (cinema.Name == null || cinema.Logo == null || cinema.Description == null)
            {
                return View(cinema);
            }

            await cinemaRepo.AddAsync(cinema);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var cinemaDetails = await cinemaRepo.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");

            return View(cinemaDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cinema = await cinemaRepo.GetByIdAsync(id);
            if (cinema == null) return View("NotFound");

            return View(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Cinema cinema)
        {
            if(cinema.Logo == null || cinema.Name == null || cinema.Description == null)
            {
                return View(cinema);
            }
            await cinemaRepo.UpdateAsync(id, cinema);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await cinemaRepo.GetByIdAsync(id);
            if (cinema == null) return View("NotFound");

            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var cinema = await cinemaRepo.GetByIdAsync(id);
            if (cinema == null) return View("NotFound");

            await cinemaRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}









