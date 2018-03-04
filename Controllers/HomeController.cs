using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Ostruvek.Models;
using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Brushes;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace Ostruvek.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly string labelPath;
        public HomeController(IHostingEnvironment environment) {
            this.environment = environment;
            labelPath = System.IO.Path.Combine(environment.ContentRootPath, "labels");
            if (! System.IO.Directory.Exists(labelPath)) System.IO.Directory.CreateDirectory(labelPath);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkin(string code) {
            FontCollection fc = new FontCollection();
            var font = fc.Install(System.IO.Path.Combine(".", "fonts", "arial.ttf")).CreateFont(100);
            using(Image<Rgba32> image = new Image<Rgba32>(696,271)) {
                image.Mutate(c => c
                    .Fill(Rgba32.White)
                    .DrawText(code, font, Rgba32.Black, new PointF(10,10)));
                    
                var path = System.IO.Path.Combine(labelPath, $"{code}.png");
                image.Save(path);
            }
            ViewData["Code"] = code;
            return(View());
        }
        public ActionResult Preview(string code) {
            var path = System.IO.Path.Combine(labelPath, $"{code}.png");
            return(PhysicalFile(path, "image/png"));
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
