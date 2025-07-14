using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation.Business.DTOs;

namespace Vacation.Business.Services.RequestServices
{
    public interface IRequestService
    {
        Task<RequestDto> CreateRequestAsync(CreateRequestDto createDto);
        Task<bool> DeleteRequestAsync(int id);
        Task<IEnumerable<RequestDto>> GetAllRequestsAsync();
        Task<RequestDto?> GetRequestAsync(int id);
        Task<RequestDto> UpdateRequestAsync(int id, string status);
        
    }
}
