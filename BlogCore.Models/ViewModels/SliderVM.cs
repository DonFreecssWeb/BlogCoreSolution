using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
    public class SliderVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre Slider")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Elija un estado")]
        public bool Estado { get; set; }
        [Display(Name = "Imagen")]
        [DataType(DataType.ImageUrl)]
        public string? UrlImagen { get; set; }
    }
}
