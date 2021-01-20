using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceDetectionAPI.Models
{
    public class Person
    {
        public string Description { get; set; }
        public string Image { get; set; }

    }
}
