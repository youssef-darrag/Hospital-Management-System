using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace Hospital.EF.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicationUserService(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public PagedResult<ListApplicationUserViewModel> GetAll(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            int excludedRecords = (pageNumber * pageSize) - pageSize;

            var users = _unitOfWork.ApplicationUsers.GetAll();
            var totalRecords = users.Count();

            var result = users.Skip(excludedRecords).Take(pageSize).ToList();

            var viewModel = result.Select(u => new ListApplicationUserViewModel
            {
                Name = u.Name,
                Gender = u.Gender,
                Email = u.Email!,
                IsDoctor = u.IsDoctor

            }).ToList();

            return new PagedResult<ListApplicationUserViewModel>
            {
                Data = viewModel,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<GenericResponse<DetailsApplicationUserViewModel>> GetByIdAsync(string id, string includeProperties = "")
        {
            var user = new ApplicationUser();

            if (string.IsNullOrEmpty(includeProperties))
                user = await _unitOfWork.ApplicationUsers.GetByIdAsync(id);
            else
                user = await _unitOfWork.ApplicationUsers.GetByIdAsync(u => u.Id == id, includeProperties);

            if (user is null)
                return new GenericResponse<DetailsApplicationUserViewModel> { Message = $"User with Id {id} not found." };

            var viewModel = new DetailsApplicationUserViewModel
            {
                Name = user.Name,
                Gender = user.Gender,
                Nationality = user.Nationality,
                Email = user.Email!,
                IsDoctor = user.IsDoctor,
                Specialist = user.Specialist,
                PictureUrl = user.PictureUrl
            };

            return new GenericResponse<DetailsApplicationUserViewModel>
            {
                Succeeded = true,
                Result = viewModel
            };
        }

        public async Task<GenericResponse<ApplicationUser>> UpdateAsync(EditApplicationUserViewModel viewModel)
        {
            var user = await _unitOfWork.ApplicationUsers.GetByIdAsync(viewModel.Id);

            if (user is null)
                return new GenericResponse<ApplicationUser> { Message = $"User with Id {viewModel.Id} not found." };

            user.Name = viewModel.Name;
            user.Gender = viewModel.Gender;
            user.Nationality = viewModel.Nationality;
            user.Address = viewModel.Address;
            user.DOB = viewModel.DOB;
            user.Specialist = viewModel.Specialist!;
            user.Email = viewModel.Email;
            user.PictureUrl = viewModel.PictureUrl;
            user.IsDoctor = viewModel.IsDoctor;

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, user.PasswordHash!, viewModel.Password);
            if (!changePasswordResult.Succeeded)
            {
                var errors = string.Join(",\n", changePasswordResult.Errors.Select(e => e.Description));

                return new GenericResponse<ApplicationUser> { Message = errors };
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(",\n", result.Errors.Select(e => e.Description));

                return new GenericResponse<ApplicationUser> { Message = errors };
            }

            if (user.IsDoctor && !await _userManager.IsInRoleAsync(user, WebSiteRoles.Doctor))
                await _userManager.AddToRoleAsync(user, WebSiteRoles.Doctor);

            if (!user.IsDoctor && !await _userManager.IsInRoleAsync(user, WebSiteRoles.Patient))
                await _userManager.AddToRoleAsync(user, WebSiteRoles.Patient);

            return new GenericResponse<ApplicationUser>
            {
                Succeeded = true,
                Result = user
            };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var isDeleted = false;

            var user = await _unitOfWork.ApplicationUsers.GetByIdAsync(id);

            if (user is null)
                return isDeleted;

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                isDeleted = true;

            return isDeleted;
        }
    }
}
