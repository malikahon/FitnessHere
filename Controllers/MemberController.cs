using AutoMapper;
using FitnessHere.DAL.DTOs;
using FitnessHere.DAL.Repositories;
using FitnessHere.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Web;

namespace FitnessHere.Controllers
{
    public class MemberController : Controller
    {
        private readonly IRepository<MemberDTO> _memberRepository;
        private readonly IMapper _mapper;


        public MemberController(IRepository<MemberDTO> memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }


        // GET: MemberController
        public ActionResult Index()
        {
            try{
            var enteties = _memberRepository.GetAll();
            var models = enteties.Select(_mapper.Map<MemberViewModel>);
                return View(models);
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
                var entity = _memberRepository.GetById(id);

                var model = _mapper.Map<MemberViewModel>(entity);

                return View(model);
            }
            catch(Exception ex)
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
        public ActionResult Create(MemberViewModel member, IFormFile profilePicture)
        {
            try
            {
                
                    if (profilePicture != null && profilePicture.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            profilePicture.CopyTo(memoryStream);
                            member.ProfilePicture = memoryStream.ToArray(); // Convert to byte[]
                        }
                    }

                    var memeber = _mapper.Map<MemberDTO>(member);

                    _memberRepository.Create(memeber);

                    return RedirectToAction(nameof(Index));
                
            }
            catch(Exception ex)
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
                var entity = _memberRepository.GetById(id);

                var model = _mapper.Map<MemberViewModel>(entity);

                return View(model);
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
        public ActionResult Edit(MemberViewModel member, IFormFile profilePicture)
        {
            try
            {

                byte[]? profilePic = null;

                // If no new image is uploaded, keep the existing profile picture
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        profilePicture.CopyTo(memoryStream);
                        profilePic = memoryStream.ToArray(); // Convert to byte[]
                    }
                }
                member.ProfilePicture = profilePic;
                    // Map and update member
                    var updatedMember = _mapper.Map<MemberDTO>(member);


                _memberRepository.Update(updatedMember);

                    return RedirectToAction(nameof(Index));
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(member);
            }
        }


        // GET: MemberController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _memberRepository.GetById(id);

                var model = _mapper.Map<MemberViewModel>(entity);

                return View(model);
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
            try{
            
                _memberRepository.Delete(id);

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
            _memberRepository.Dispose();
        }
    }
}
