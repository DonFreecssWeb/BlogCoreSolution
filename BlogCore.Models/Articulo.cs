using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Articulo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre es obligatorio")]
        [Display(Name = "Nombre de artículo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]        
        public string Descripcion { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Imagen")]
        [DataType(DataType.ImageUrl)]
        public string UrlImagen { get; set; }

        [Required(ErrorMessage = "La categoria es necesaria")]
        
        public int CategoriaID { get; set; }

        [ForeignKey("CategoriaID")]
        public Categoria Categoria { get; set; }
    }
}
