using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace TheMovies.Models
{
    public class PeliculasData
    {
        private readonly dbContextData _context = new dbContextData();

        //insert imagen to database 
        public int UploadImageInDataBase(HttpPostedFileBase file, Peliculas pelis)
        {
            pelis.Data = ConvertToBytes(file);
            var peliculas = new Peliculas
            {


                Nombre = pelis.Nombre,
                Genero = pelis.Genero,
                AñoPublicacion = pelis.AñoPublicacion,
                Imagen = file.FileName,
                Data = pelis.Data,
                Director = pelis.Director


            };



            _context.Peliculas.Add(peliculas);
            int i = _context.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }





        }

        //update when it's does has imagen
        public int UploadImageInDataBaseEdit(HttpPostedFileBase file, Peliculas pelis)
        {


            pelis.Data = ConvertToBytes(file);
            var peliculas = new Peliculas
            {
                Id_peliculas = pelis.Id_peliculas,
                Nombre = pelis.Nombre,
                Genero = pelis.Genero,
                AñoPublicacion = pelis.AñoPublicacion,
                Imagen = file.FileName,
                Data = pelis.Data,
                Director = pelis.Director
                


            };
            _context.Entry(peliculas).State = EntityState.Modified;
            int i = _context.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }





        //Convert imagen to byte
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }



    }
}