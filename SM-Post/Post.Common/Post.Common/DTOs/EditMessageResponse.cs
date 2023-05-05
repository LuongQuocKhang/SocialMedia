using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Post.Common.DTOs
{
    public class EditMessageResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}