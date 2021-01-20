using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FaceDetectionAPI.Models;
using IronPython.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Emgu.CV;
using System.Drawing;
using Emgu.CV.Structure;
using static System.Net.Mime.MediaTypeNames;

namespace FaceDetectionAPI.Controllers
{
    [Route("/")]
    [ApiController]

    public class ImageObjectController : ControllerBase
    {
        public static IWebHostEnvironment _environment;

        public ImageObjectController(IWebHostEnvironment en)
        {
            _environment = en;

        }

        [HttpPost]
        public async Task<string> UploadImage([FromForm] FileUpload imgObj)
        {
            try
            {
                if (imgObj.File.Length > 0)
                {
                    string path = _environment.WebRootPath + "\\Upload\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using FileStream fileStream = System.IO.File.Create(path + imgObj.File.FileName);
                    imgObj.File.CopyTo(fileStream);
                    fileStream.Flush();


                    return imgObj.File.FileName.ToString();
                }
                else
                {
                    return "Failed to upload image";
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetImage(string imgName)
        {
            string path = _environment.WebRootPath + "\\Upload\\";
            var filePath = path + imgName + ".png";
            if (System.IO.File.Exists(filePath))
            {
                var image = System.IO.File.OpenRead(filePath);
                return File(image, "image/jpeg");
            }
            return null;

        }

        [Route("[action]")]
        [HttpPost]

        public async Task<IActionResult> FaceDetection([FromForm] FileUpload imgObj)
        {
            if (imgObj.File.Length > 0)
            {
                string path = _environment.WebRootPath + "\\Upload\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using FileStream fileStream = System.IO.File.Create(path + imgObj.File.FileName);
                imgObj.File.CopyTo(fileStream);
                fileStream.Flush();
                fileStream.Close();
                Mat mat = CvInvoke.Imread(path + imgObj.File.FileName, Emgu.CV.CvEnum.ImreadModes.AnyColor);
                System.IO.File.Delete(path + imgObj.File.FileName);
                CascadeClassifier faceDetector = new CascadeClassifier(_environment.ContentRootPath + "\\config\\haarcascade_frontalface_default.xml");
                Image<Bgr, byte> img = mat.ToImage<Bgr, byte>();
                //int width = 200;
                //int height = 200;
                var count=0;
                //CvInvoke.Resize(img, img, new Size(width, height));
                using (img)
                {
                    if (img != null)
                    {
                        //var grayframe = img.Convert<Gray, byte>();
                        var faces = faceDetector.DetectMultiScale(mat, 1.1, 4);
                        
                        foreach (var face in faces)
                        {
                            img.Draw(face, new Bgr(Color.Red), 1);
                            count+=1;
                        }
                    }
                    CvInvoke.Imwrite(path + "detected.jpg", img);
                    var image = System.IO.File.OpenRead(path + "detected.jpg");
                    //System.IO.File.Delete(path + "detected.jpg");
                    return File(image, "image/jpeg");

                }
                //CvInvoke.Imwrite(path + "detected.jpg", img);
                //img.Save(path+"detected.jpg");
                //var image = System.IO.File.OpenRead(path + "detected.jpg");
                
                //var image = System.IO.File.OpenRead(path +"1"+ imgObj.File.FileName);
                //System.IO.File.Delete(path + "detected.jpg"); 
                //return mat2;
            }
            else
            {
                return null;
            }
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<Person> FaceDetectionWithName([FromForm] FileUpload imgObj)
        {
            if (imgObj.File.Length > 0)
            {
                string path = _environment.WebRootPath + "\\Upload\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using FileStream fileStream = System.IO.File.Create(path + imgObj.File.FileName);
                imgObj.File.CopyTo(fileStream);
                fileStream.Flush();
                fileStream.Close();
                Mat mat = CvInvoke.Imread(path + imgObj.File.FileName, Emgu.CV.CvEnum.ImreadModes.AnyColor);
                System.IO.File.Delete(path + imgObj.File.FileName);
                CascadeClassifier faceDetector = new CascadeClassifier(_environment.ContentRootPath + "\\config\\haarcascade_frontalface_default.xml");
                Image<Bgr, byte> img = mat.ToImage<Bgr, byte>();
                var count = 0;
                using (img)
                {
                    if (img != null)
                    {
                        //var grayframe = img.Convert<Gray, byte>();
                        var faces = faceDetector.DetectMultiScale(mat, 1.1, 4);

                        foreach (var face in faces)
                        {
                            img.Draw(face, new Bgr(Color.Red), 1);
                            count += 1;
                        }
                    }
                    CvInvoke.Imwrite(path + "detected.jpg", img);
                    //var image = System.IO.File.OpenRead(path + "detected.jpg");
                    byte[] imageArray = System.IO.File.ReadAllBytes(path + "detected.jpg");
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                    Person person = new Person();
                    person.Image = base64ImageRepresentation;
                    person.Description = "Maleeha";
                    return person;

                }
            }
            else
            {
                return null;
            }
        }
    }
}
