using AutoMapper;
using FitnessHere.DAL.DTOs;
using FitnessHere.DAL.Entities;
using FitnessHere.DAL.Repositories;
using FitnessHere.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessHere.Controllers
{
    public class TrainerController : Controller
    {
        private readonly IRepository<Trainer> _trainerRepository;


        public TrainerController(IRepository<Trainer> trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }



        // GET: TrainerController
        public ActionResult Index()
        {
            try
            {
                var enteties = _trainerRepository.GetAll();
                return View(enteties);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: MemberController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var entity = _trainerRepository.GetById(id);

                return View(entity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: MemberController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Trainer trainer, IFormFile ProfilePictureFile)
        {
            try
            {

                if (ProfilePictureFile != null && ProfilePictureFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        ProfilePictureFile.CopyTo(memoryStream);
                        trainer.ProfilePicture = memoryStream.ToArray(); // Convert to byte[]
                    }
                }


                _trainerRepository.Create(trainer);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: MemberController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var entity = _trainerRepository.GetById(id);

                return View(entity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // POST: MemberController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Trainer trainer, IFormFile ProfilePictureFile)
        {
            try
            {

                byte[]? profilePic = null;

                // If no new image is uploaded, keep the existing profile picture
                if (ProfilePictureFile != null && ProfilePictureFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        ProfilePictureFile.CopyTo(memoryStream);
                        profilePic = memoryStream.ToArray(); // Convert to byte[]
                    }
                }
                trainer.ProfilePicture = profilePic;



                _trainerRepository.Update(trainer);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(trainer);
            }
        }


        // GET: MemberController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _trainerRepository.GetById(id);

                return View(entity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // POST: MemberController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                _trainerRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _trainerRepository.Dispose();
        }
    }
}
