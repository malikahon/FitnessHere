using FitnessHere.DAL.Repositories;
using FitnessHere.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessHere.Controllers
{
    public class MemberFilterController : Controller
    {
        private readonly AdoNetFiltering _filteringRepository;

        public MemberFilterController(AdoNetFiltering filteringRepository)
        {
            _filteringRepository = filteringRepository;
        }

        public IActionResult Index(int page = 1, string sortColumn = "MemberID", bool sortDesc = false,
            string memberFirstName = null, string memberLastName = null, string className = null,
            string trainerName = null, DateTime? registrationDate = null, int pageSize = 10)
        {
            int totalCount;
            var members = _filteringRepository.Filter(out totalCount,
                memberFirstName, memberLastName, className, trainerName, registrationDate,
                sortColumn, sortDesc, page, pageSize);

            var viewModel = new MemberFilterViewModel
            {
                Members = members,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDesc = sortDesc,
                MemberFirstName = memberFirstName,
                MemberLastName = memberLastName,
                ClassName = className,
                TrainerName = trainerName,
                RegistrationDate = registrationDate
            };

            // Calculate total pages
            viewModel.TotalPages = (int)Math.Ceiling((double)totalCount / viewModel.PageSize);

            return View(viewModel);
        }
    }
}
