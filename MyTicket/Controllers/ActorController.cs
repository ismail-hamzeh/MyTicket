using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Data.Repositories;
using MyTicket.Models;
using MyTicket.Static;

namespace MyTicket.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ActorController : Controller
    {

        private readonly IActorRepo actorRepo;

        public ActorController(IActorRepo actorRepo)
        {
            this.actorRepo = actorRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var actors = await actorRepo.GetAllAsync();
            return View(actors);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Actor actor)
        {
            if (actor.ProfilePictureUrl == null || actor.FullName == null || actor.Bio == null)
            {
                return View(actor);
            }

            await actorRepo.AddAsync(actor);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await actorRepo.GetByIdAsync(id);

            if (actorDetails == null) return View("NotFound");

            return View(actorDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var actor = await actorRepo.GetByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Actor actor)
        {
            if (actor.ProfilePictureUrl == null || actor.FullName == null || actor.Bio == null)
            {
                return View(actor);
            }
            await actorRepo.UpdateAsync(id, actor);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var actor = await actorRepo.GetByIdAsync(id);
            if (actor == null) return View("NotFound");

            return View(actor);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await actorRepo.GetByIdAsync(id);
            if (actor == null) return View("NotFound");

            await actorRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
