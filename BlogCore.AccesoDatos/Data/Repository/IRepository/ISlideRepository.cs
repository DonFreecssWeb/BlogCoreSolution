using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface ISlideRepository : IRepository<Slider>
    {        
        void Update(Slider slider);
    }
}
