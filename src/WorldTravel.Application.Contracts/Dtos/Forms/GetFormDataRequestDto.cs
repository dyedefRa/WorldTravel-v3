using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace WorldTravel.Dtos.Forms
{
    public class GetFormDataRequestDto : PagedAndSortedResultRequestDto
    {
        public int Id { get; set; }

    }
}
