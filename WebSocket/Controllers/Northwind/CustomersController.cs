using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSocket.Models.Northwind;
using PagedList;

namespace WebSocket.Controllers.Northwind
{
    public class CustomersController : Controller
    {
        private Context db = new Context();

        // GET: Customers
        public async Task<ActionResult> Index(string ordenarPor, string filtroActual, string busqueda, int? numeroDePagina)
        {
            // datos originales en la base de datos.
            var data = await db.Customers.ToListAsync();
            // Guardar el orden actual.
            ViewBag.CurrentSort = ordenarPor;
            // Establecer los ordenes de cada columna....
            ViewBag.CompanySortParm = String.IsNullOrEmpty(ordenarPor) ? "company_desc" : "";
            ViewBag.ContactSortParm = ordenarPor == "contact" ? "contact_desc" : "contact";
            ViewBag.Contact2SortParm = ordenarPor == "contact2" ? "contact2_desc" : "contact2";
            ViewBag.AddressSortParm = ordenarPor == "address" ? "address_desc" : "address";
            ViewBag.CitySortParm = ordenarPor == "city" ? "city_desc" : "city";
            ViewBag.RegionSortParm = ordenarPor == "region" ? "region_desc" : "region";
            ViewBag.PostalSortParm = ordenarPor == "postal" ? "postal_desc" : "postal";
            ViewBag.CountrySortParm = ordenarPor == "country" ? "country_desc" : "country";
            ViewBag.PhoneSortParm = ordenarPor == "phone" ? "phone_desc" : "phone";
            ViewBag.FaxSortParm = ordenarPor == "fax" ? "fax_desc" : "fax";
            // Comprobar la cadena de busqueda de texto.
            if(busqueda != null) // si hay una busqueda mostrar los resultados desde la primer pagina.
            {
                numeroDePagina = 1;
            }
            else
            {
                /// en el caso de que vengamos de una busqueda conservar el valor
                /// para navegar en las siguientes paginas de resultado.
                busqueda = filtroActual;
            }
            ViewBag.CurrentFilter = busqueda;
            // Hacer la busqueda
            if (!string.IsNullOrEmpty(busqueda))
            {
                data = data.Where
                   (
                       s => s.CompanyName.Contains(busqueda) ||
                       s.ContactName.Contains(busqueda) ||
                       s.ContactTitle.Contains(busqueda) ||
                       s.Address.Contains(busqueda) ||
                       s.City.Contains(busqueda) ||
                       // s.Region.Contains(cadenaBusqueda) || contiene campos nullos :( 
                       s.Country.Contains(busqueda) ||
                       s.Phone.Contains(busqueda) //||
                       // s.Fax.Contains(cadenaBusqueda) Contiene campos nulos
                   ).ToList();

            }
            // ordenar de forma ascendente o descendente.
            switch (ordenarPor)
            {
                case "company_desc": data = data.OrderByDescending(x => x.CompanyName).ToList();  break;

                case "contact": data = data.OrderBy(x => x.ContactName).ToList(); break;
                case "contact_desc": data = data.OrderByDescending(x => x.ContactName).ToList(); break;

                case "contact2": data = data.OrderBy(x => x.ContactTitle).ToList(); break;
                case "contact2_desc": data = data.OrderByDescending(x => x.CompanyName).ToList(); break;

                case "address": data = data.OrderBy(x => x.Address).ToList(); break;
                case "address_desc": data = data.OrderByDescending(x => x.Address).ToList(); break;

                case "city": data = data.OrderBy(x => x.City).ToList(); break;
                case "city_desc": data = data.OrderByDescending(x => x.City).ToList(); break;

                case "region": data = data.OrderBy(x => x.Region).ToList(); break;
                case "region_desc": data = data.OrderByDescending(x => x.Region).ToList(); break;

                case "postal": data = data.OrderBy(x => x.PostalCode).ToList(); break;
                case "postal_desc": data = data.OrderByDescending(x => x.PostalCode).ToList(); break;

                case "country": data = data.OrderBy(x => x.Country ).ToList(); break;
                case "country_desc": data = data.OrderByDescending(x => x.Country).ToList(); break;

                case "phone": data = data.OrderBy(x => x.Phone).ToList(); break;
                case "phone_desc": data = data.OrderByDescending(x => x.Phone).ToList(); break;

                case "fax": data = data.OrderBy(x => x.Fax).ToList(); break;
                case "fax_desc": data = data.OrderByDescending(x => x.Fax).ToList(); break;
                default: // Company ascendendte.
                    data = data.OrderBy(x => x.CompanyName).ToList();
                    break;
            }

            
            int numeroPagina = (numeroDePagina ?? 1);
            int filasPorPagina = 10;
            return View(data.ToPagedList(numeroPagina, filasPorPagina));
        }

        // GET: Customers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
