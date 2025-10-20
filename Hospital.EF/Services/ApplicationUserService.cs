using Hospital.Core.Consts;
using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Hospital.EF.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ApplicationUserService(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public PagedResult<ListApplicationUserViewModel> GetAll(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            int excludedRecords = (pageNumber * pageSize) - pageSize;

            var Admins = _userManager.GetUsersInRoleAsync(WebSiteRoles.Admin).GetAwaiter().GetResult().Cast<ApplicationUser>();

            var users = _unitOfWork.ApplicationUsers.GetAll(orderBy: u => u.Name).Except(Admins);
            var totalRecords = users.Count();

            var result = users.Skip(excludedRecords).Take(pageSize).ToList();

            var viewModel = result.Select(u => new ListApplicationUserViewModel
            {
                Id = u.Id,
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

        public async Task<GenericResponse<ApplicationUser>> GetByIdAsync(string id, string includeProperties = "")
        {
            var user = new ApplicationUser();

            if (string.IsNullOrEmpty(includeProperties))
                user = await _unitOfWork.ApplicationUsers.GetByIdAsync(id);
            else
                user = await _unitOfWork.ApplicationUsers.GetByIdAsync(u => u.Id == id, includeProperties);

            if (user is null)
                return new GenericResponse<ApplicationUser> { Message = $"User with Id {id} not found." };

            return new GenericResponse<ApplicationUser>
            {
                Succeeded = true,
                Result = user
            };
        }

        public async Task<GenericResponse<ApplicationUser>> UpdateAsync(EditApplicationUserViewModel viewModel)
        {
            var user = await _unitOfWork.ApplicationUsers.GetByIdAsync(viewModel.Id);

            if (user is null)
                return new GenericResponse<ApplicationUser> { Message = $"User with Id {viewModel.Id} not found." };

            var hasNewImage = viewModel.PictureUrl is not null;
            var oldImage = user.PictureUrl;

            user.Name = viewModel.Name;
            user.Gender = viewModel.Gender;
            user.Nationality = viewModel.Nationality;
            user.Address = viewModel.Address;
            user.DOB = viewModel.DOB;
            user.Specialist = viewModel.Specialist!;
            user.Email = viewModel.Email;

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                var errors = string.Join(",\n", changePasswordResult.Errors.Select(e => e.Description));

                return new GenericResponse<ApplicationUser> { Message = errors };
            }

            var path = user.IsDoctor ? ImagePaths.Doctor : ImagePaths.Patient;

            if (hasNewImage)
            {
                ImageOperation imagePath = new ImageOperation(_webHostEnvironment, path);
                user.PictureUrl = await imagePath.SaveImage(viewModel.PictureUrl!);
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (hasNewImage)
                {
                    ImageOperation imageOperation = new ImageOperation(_webHostEnvironment, path);
                    imageOperation.DeleteImage(oldImage);
                }

                return new GenericResponse<ApplicationUser>
                {
                    Succeeded = true,
                    Result = user
                };
            }
            else
            {
                if (hasNewImage)
                {
                    ImageOperation imageOperation = new ImageOperation(_webHostEnvironment, path);
                    imageOperation.DeleteImage(user.PictureUrl);
                }

                var errors = string.Join(",\n", result.Errors.Select(e => e.Description));

                return new GenericResponse<ApplicationUser> { Message = errors };
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var isDeleted = false;

            var user = await _unitOfWork.ApplicationUsers.GetByIdAsync(id);

            if (user is null)
                return isDeleted;

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                isDeleted = true;

                if (!await _userManager.IsInRoleAsync(user, WebSiteRoles.Admin))
                {
                    var path = user.IsDoctor ? ImagePaths.Doctor : ImagePaths.Patient;

                    ImageOperation imageOperation = new ImageOperation(_webHostEnvironment, path);
                    imageOperation.DeleteImage(user.PictureUrl);
                }
            }

            return isDeleted;
        }
    }
}
