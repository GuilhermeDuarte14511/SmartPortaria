using System.Collections.Generic;
using System.Linq;

namespace SmartPortaria.Application.DTOs
{
    public class ReconhecimentoFacialRequest
    {
        public IEnumerable<float> VetorFacial { get; set; } = Enumerable.Empty<float>();
    }
}
