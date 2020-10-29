using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TheMovies.Models;
using System.Net;
using System.IO;
using System.Data.Entity.Migrations;
using PagedList;

namespace TheMovies.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly dbContextData _context = new dbContextData();
        // GET: Productos


        [HttpGet]
        public ActionResult Index(string Sorting_Order, string searchData, string filterValue, int? pageNo)
        {

            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Nombre" : "";
            
            if (searchData != null)
            {
                pageNo = 1;
            }

            else
            {
                searchData = filterValue;
            }

            ViewBag.Filter = searchData;

            var peliculas = from pelis in _context.Peliculas select pelis;

            if (!String.IsNullOrEmpty(searchData))
            {
                peliculas = peliculas.Where(pelis => pelis.Nombre.ToUpper().Contains(searchData.ToUpper())
                     || pelis.Genero.ToUpper().Contains(searchData.ToUpper())
                     || pelis.AñoPublicacion.ToString().Contains(searchData.ToUpper()));

            }


            switch (Sorting_Order)
            {
                case "Nombre":
                    peliculas = peliculas.OrderByDescending(pelis => pelis.AñoPublicacion);
                    break;

                case "Genero":
                    peliculas = peliculas.OrderByDescending(pelis => pelis.Genero);
                    break;

                case "Age":
                    peliculas = peliculas.OrderBy(pelis => pelis.AñoPublicacion);
                    break;

                default:
                    peliculas = peliculas.OrderBy(pelis => pelis.Nombre);
                    break;

            }


            int Size_Of_Page = 4;
            int No_Of_Page = (pageNo ?? 1);
            return View(peliculas.ToPagedList(No_Of_Page, Size_Of_Page));

        }

        [HttpGet]
        public ActionResult CrearPeliculas()
        {
            return View();
        }

        /// <param name="model"></param>

        [Route("CrearPeliculas")]
        [HttpPost]
        public ActionResult CrearPeliculas(Peliculas model)
        {
            if (ModelState.IsValid)
            {
                if (model.AñoPublicacion <= 1990 || model.AñoPublicacion > DateTime.Now.Year)
                {
                    ModelState.AddModelError("AñoPublicacion", "El Año esta Fuera del Rango");
                    return View(model);
                }
                HttpPostedFileBase file = Request.Files["ImageData"];
                PeliculasData service = new PeliculasData();
                int i = service.UploadImageInDataBase(file, model);
                if (i == 1)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(model);
        }





        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from Peliculas in _context.Peliculas where Peliculas.Id_peliculas == Id select Peliculas.Data;
            byte[] cover = q.First();
            return cover;
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            var peliculas = _context.Peliculas.SingleOrDefault(p => p.Id_peliculas == id);
            if (peliculas == null)
            {
                return HttpNotFound();
            }


            return View(peliculas);
        }


        [HttpPost]
        public ActionResult Editar(Peliculas peliculas)
        {
            if (peliculas.AñoPublicacion <= 1990 || peliculas.AñoPublicacion > DateTime.Now.Year)
            {
                ModelState.AddModelError("AñoPublicacion", "La Año esta Fuera del Rango");
                return View(peliculas);
            }

            byte[] imagenActual = null;
            string nombreImagen = null;
            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                imagenActual = _context.Peliculas.SingleOrDefault(p => p.Id_peliculas == peliculas.Id_peliculas).Data;
                peliculas.Data = imagenActual;

                nombreImagen = _context.Peliculas.SingleOrDefault(p => p.Id_peliculas == peliculas.Id_peliculas).Imagen;
                peliculas.Imagen = nombreImagen;


            }
            else
            {

                PeliculasData service = new PeliculasData();
                int i = service.UploadImageInDataBaseEdit(file, peliculas);

                if (i == 1)
                {
                    return RedirectToAction("Index");
                }



            }
            if (ModelState.IsValid)
            {
                _context.Set<Peliculas>().AddOrUpdate(peliculas);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(peliculas);


        }



        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            var peliculas = _context.Peliculas.SingleOrDefault(p => p.Id_peliculas == id);
            if (peliculas == null)
            {
                return HttpNotFound();
            }


            return View(peliculas);
        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var productos = _context.Peliculas.SingleOrDefault(x => x.Id_peliculas == id);
            _context.Peliculas.Remove(productos ?? throw new InvalidOperationException());
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }


}
