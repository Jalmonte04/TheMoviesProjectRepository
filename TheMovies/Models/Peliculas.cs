using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheMovies.Models
{
    public class Peliculas
    {
        [Key]
        public int Id_peliculas { get; set; }

        [Required(ErrorMessage = "Introduce el Nombre")]
        [Display(Name = "Pelicula")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Selecciona El Genero")]
        [Display(Name = "Seleciona El Genero")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "Introduce El Año")]
        [Display(Name = "Año")]
        public int AñoPublicacion { get; set; }

      

        [AllowHtml]
        public string Imagen { get; set; }
        public byte [] Data { get; set; }

        [Required(ErrorMessage = "Introduce el Director")]
        [Display(Name = "Director")]
        public string Director {get; set;}       
    }

}