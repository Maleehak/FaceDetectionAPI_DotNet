using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceDetectionAPI.Models
{
    public class FileUpload
    {
        public IFormFile File { get; set; }

    }
}
