using System;
using Volo.Abp.Application.Dtos;
using WorldTravel.Enums;

namespace WorldTravel.Dtos.Forms
{
    public class GetFormRequestDto : PagedAndSortedResultRequestDto
    {
        public string FullNameFilter { get; set; }
        public string EmailFilter { get; set; }
        public string PhoneFilter { get; set; }
        public BooleanType IsContactedFilter { get; set; }
        public Guid Id { get; set; }
    }
}
