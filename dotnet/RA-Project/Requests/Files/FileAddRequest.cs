using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Files
{
    public class FileAddRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public int FileTypeId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public int CreatedBy { get; set; }

    }
}
