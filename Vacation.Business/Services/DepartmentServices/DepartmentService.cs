using Vacation.Business.DTOs;
using Vacation.Data.Models;
using Vacation.Data.UnitOfWork;
using VacationRequestSystem.Business.Utilities;

namespace Vacation.Business.Services.DepartmentServices
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      
        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAll(
                d => d.IsActive);
            return departments.Select(d => new DepartmentDto { Id = d.Id, Name = d.Name,IsActive=d.IsActive });

        }

        public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto createDto)
        {
            if (createDto == null)
            {
                throw new ArgumentNullException(nameof(createDto), "Department DTO cannot be null");
            }
            var department = new Department
            {
                Name = createDto.Name,
                IsActive = true
            };
            await _unitOfWork.DepartmentRepository.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(department);

        }

        public async Task<DepartmentDto> GetDepartmenAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAsync(d => d.Id == id && d.IsActive);
            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found");
            }
            return MapToDto(department);
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(int id, string name)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAsync(d => d.Id == id && d.IsActive);
            if (department == null)
            {
                throw new ArgumentException($"Department with ID {id} not found");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Department name cannot be null or empty", nameof(name));
            }
            department.Name = name;
            _unitOfWork.DepartmentRepository.Update(department);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(department);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAsync(d => d.Id == id && d.IsActive==true);
            if (department == null)
            {
                throw new ArgumentException($"Department with ID {id} not found");
            }
            department.IsActive = false; // Soft delete
            _unitOfWork.DepartmentRepository.Update(department);
            int changes=  await _unitOfWork.SaveChangesAsync();
            return changes > 0;
        }

        private DepartmentDto MapToDto(Department request)
        {
            return new DepartmentDto
            {
                Id = request.Id,
                Name = request.Name,
                IsActive = request.IsActive
            };
        }
    }
}