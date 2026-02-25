using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSessionEmpleados.Extensions;
using MvcCoreSessionEmpleados.Models;
using MvcCoreSessionEmpleados.Repositories;

namespace MvcCoreSessionEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if (salario != null)
            {
                // Queremos almacenar la suma total de salarios
                int sumaTotal = 0;
                if (HttpContext.Session.GetString("sumasalarial") != null)
                {
                    // Si ya tenemos datos almacenados, los recuperamos
                    sumaTotal = HttpContext.Session.GetObject<int>("sumasalarial");
                }
                // Sumamos el nuevo salario a la suma total.
                sumaTotal += salario.Value;
                HttpContext.Session.SetObject("sumasalarial", sumaTotal);
                ViewData["mensaje"] = "Salario almacenado: " + salario;
            }

            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult SumaSalarial()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleados(int? idempleado)
        {
            if (idempleado != null)
            {
                Empleado emp = await this.repo.FindEmpleadoAsync(idempleado.Value);
                // En Session tendremos almacenados un conjunto de Empleados
                List<Empleado> empleados;

                // Se busca si ya existe la lista en Session
                if (HttpContext.Session.GetObject<List<Empleado>>("empleados") != null)
                {
                    // Si existe, se recupera la lista de Session.
                    empleados = HttpContext.Session.GetObject<List<Empleado>>("empleados");
                }
                else
                {
                    // Si no, se inicializa la lista.
                    empleados = new List<Empleado>();
                }

                empleados.Add(emp);

                // Se vuelve a almacenar la lista en Session
                HttpContext.Session.SetObject("empleados", empleados);
            }

            List<Empleado> empleadosList = await this.repo.GetEmpleadosAsync();

            return View(empleadosList);
        }

        public IActionResult ListaEmpleados()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleadosById(int? idempleado)
        {
            if (idempleado != null)
            {
                // Esta vez se almacena lo minimo
                List<int> empleadosIds;
                if (HttpContext.Session.GetObject<List<int>>("idsempleados") != null)
                {
                    // Si existe, se recupera la coleccion
                    empleadosIds = HttpContext.Session.GetObject<List<int>>("idsempleados");
                }
                else
                {
                    // Si no, se inicializa
                    empleadosIds = new List<int>();
                }
                empleadosIds.Add(idempleado.Value);
                HttpContext.Session.SetObject("idsempleados", empleadosIds);
            }

            List<Empleado> empleadosList = await this.repo.GetEmpleadosAsync();
            return View(empleadosList);
        }

        public async Task<IActionResult> ListaEmpleadosById()
        {
            List<int> empleadosIds = HttpContext.Session.GetObject<List<int>>("idsempleados");
            List<Empleado> empleados = new List<Empleado>();
            if (empleadosIds != null)
            {
                empleados = await this.repo.GetEmpleadosSessionAsync(empleadosIds);
            }
            return View(empleados);
        }

        public async Task<IActionResult> SessionEmpleadosV4(int? idempleado)
        {
            List<int> empleadosIds;

            if (HttpContext.Session.GetObject<List<int>>("idsempleadosv4") != null)
            {
                empleadosIds = HttpContext.Session.GetObject<List<int>>("idsempleadosv4");
            }
            else
            {
                empleadosIds = new List<int>();
            }

            if (idempleado != null)
            {
                empleadosIds.Add(idempleado.Value);
                HttpContext.Session.SetObject("idsempleadosv4", empleadosIds);
            }

            List<Empleado> empleadosList = await this.repo.GetEmpleadosNotSessionAsync(
                empleadosIds
            );
            return View(empleadosList);
        }

        public async Task<IActionResult> ListaEmpleadosV4()
        {
            List<int> empleadosIds = HttpContext.Session.GetObject<List<int>>("idsempleadosv4");
            List<Empleado> empleados = new List<Empleado>();
            if (empleadosIds != null)
            {
                empleados = await this.repo.GetEmpleadosSessionAsync(empleadosIds);
            }
            return View(empleados);
        }
    }
}
