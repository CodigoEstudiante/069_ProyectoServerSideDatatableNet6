using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBPRUEBASContext _dbcontext;

        public HomeController(DBPRUEBASContext context)
        {
            _dbcontext = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult ObtenerEmpleado() {


            //Representa el número de veces que se ha realizado una petición
            int NroPeticion = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0");


            //cuantos registros va a devolver
            int CantidadRegistros = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

            //cuantos registros va a omitir
            int OmitirRegistros = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            //el texto de busqueda
            string ValorBuscado = Request.Form["search[value]"].FirstOrDefault() ?? "";

            //========================= PARA OBTENER DATOS =============================//
            List<Empleado> lista = new List<Empleado>();


            IQueryable<Empleado> queryEmpleado = _dbcontext.Empleados;//select * from empleado

            // Total de registros antes de filtrar.
            int TotalRegistros = queryEmpleado.Count();

            queryEmpleado = queryEmpleado
                .Where(e => string.Concat(e.Nombre, e.Correo, e.Direccion, e.Telefono).Contains(ValorBuscado));

            // Total de registros ya filtrados.
            int TotalRegistrosFiltrados = queryEmpleado.Count();


            lista = queryEmpleado.Skip(OmitirRegistros).Take(CantidadRegistros).ToList();


            return Json(new {
                
                draw = NroPeticion,
                recordsTotal = TotalRegistros,
                recordsFiltered = TotalRegistrosFiltrados,
                data = lista,
            });
        
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}