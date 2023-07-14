using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Data.Repositories;
using MyTicket.Models;
using MyTicket.Static;

namespace MyTicket.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ProducerController : Controller
    {
        private readonly IProducerRepo producerRepo;

        public ProducerController(IProducerRepo producerRepo)
        {
            this.producerRepo = producerRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var producers = await producerRepo.GetAllAsync();
            return View(producers);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Producer producer)
        {
            if (producer.ProfilePictureUrl == null || producer.FullName == null || producer.Bio == null)
            {
                return View(producer);
            }
            await producerRepo.AddAsync(producer);

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await producerRepo.GetByIdAsync(id);

            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Producer = await producerRepo.GetByIdAsync(id);
            if (Producer == null) return View("NotFound");

            return View(Producer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Producer producer)
        {
            if (producer.ProfilePictureUrl == null || producer.FullName == null || producer.Bio == null) return View(producer);
            await producerRepo.UpdateAsync(id, producer);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var producer = await producerRepo.GetByIdAsync(id);
            if (producer == null) return View("NotFound");
            return View(producer);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var actor = await producerRepo.GetByIdAsync(id);
            if (actor == null) return View("NotFound");

            await producerRepo.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

















