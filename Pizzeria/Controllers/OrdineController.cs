using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pizzeria.Models;

namespace Pizzeria.Controllers
{
    public class OrdineController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Ordine
        public ActionResult Index()
        {
            var ordine = db.Ordine.Include(o => o.Pizza).Include(o => o.Utente);
            return View(ordine.ToList());
        }

        // GET: Ordine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordine ordine = db.Ordine.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            return View(ordine);
        }

        // GET: Ordine/Create
        public ActionResult Create()
        {
            ViewBag.IdPizza = new SelectList(db.Pizza, "IDpizza", "NomePizza");
            ViewBag.IdUtente = new SelectList(db.Utente, "IDutente", "Nome");
            return View();
        }

        // POST: Ordine/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDordine,Quantita,Note,IndirizzoSpedizione,OrdineConsegnato,OrdineInConsegnato,DataOrdine,IdPizza,IdUtente")] Ordine ordine)
        {
            if (ModelState.IsValid)
            {
                db.Ordine.Add(ordine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPizza = new SelectList(db.Pizza, "IDpizza", "NomePizza", ordine.IdPizza);
            ViewBag.IdUtente = new SelectList(db.Utente, "IDutente", "Nome", ordine.IdUtente);
            return View(ordine);
        }

        // GET: Ordine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordine ordine = db.Ordine.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPizza = new SelectList(db.Pizza, "IDpizza", "NomePizza", ordine.IdPizza);
            ViewBag.IdUtente = new SelectList(db.Utente, "IDutente", "Nome", ordine.IdUtente);
            return View(ordine);
        }

        // POST: Ordine/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDordine,Quantita,Note,IndirizzoSpedizione,OrdineConsegnato,OrdineInConsegnato,DataOrdine,IdPizza,IdUtente")] Ordine ordine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPizza = new SelectList(db.Pizza, "IDpizza", "NomePizza", ordine.IdPizza);
            ViewBag.IdUtente = new SelectList(db.Utente, "IDutente", "Nome", ordine.IdUtente);
            return View(ordine);
        }

        // GET: Ordine/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ordine ordine = db.Ordine.Find(id);
        //    if (ordine == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ordine);
        //}

        // POST: Ordine/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Ordine ordine = db.Ordine.Find(id);
            db.Ordine.Remove(ordine);
            db.SaveChanges();
            return RedirectToAction("Carrello");
        }

        //------- Partila View Aggiungi al carrello-------------  
        public ActionResult PartialViewAggiungiAlCarrello()
        {
            return PartialView("_PartialViewAggiungiAlCarrello");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialViewAggiungiAlCarrello(Ordine ordine, int id)
        {
            if (ModelState.IsValid && ordine.Quantita>0)// se la quantita e maggiore di 0 vuol dire che ce un ordine
            {
                Utente utente=db.Utente.Where(x=>x.Username == User.Identity.Name).First();
                ordine.IndirizzoSpedizione = " ";
                ordine.IdPizza = id;
                ordine.IdUtente = utente.IDutente;
                db.Ordine.Add(ordine);
                db.SaveChanges();
                return RedirectToAction("Index","Pizza",null);
            }

            return RedirectToAction("Details","Pizza",db.Pizza.Find(id));
        }

        public ActionResult Carrello() {

        Utente utente=db.Utente.Where(x=>x.Username== User.Identity.Name).FirstOrDefault();
        
           List<Ordine> ListaOrdine=db.Ordine.Include(x=>x.Pizza).Where(x=>x.IdUtente==utente.IDutente).ToList();
            if (ListaOrdine.Count<=0) 
            {
                ViewBag.carrelloVuoto = "Carrello Vuoto";
            }
           
            return View(ListaOrdine);
        }

        //=============Conferma dell'ordine=====================
        public ActionResult PartialViewConfermaOrdine() { 

            return PartialView("_PartialViewConfermaOrdine"); 

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult PartialViewConfermaOrdine(Ordine ordine) {

            if (ModelState.IsValid && ordine.IndirizzoSpedizione!=null)
            {
              Utente utente = db.Utente.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
                List<Ordine> Carrello = db.Ordine.Where(x => x.IdUtente == utente.IDutente && x.OrdineInConsegnato == false).ToList();

                foreach (Ordine item in Carrello)
                {
                    item.IndirizzoSpedizione = ordine.IndirizzoSpedizione;
                    item.DataOrdine=DateTime.Now;
                    item.OrdineInConsegnato = true;
                   


                    db.Entry(ordine).State = EntityState.Modified;
                    db.SaveChanges();
                }

                 ViewBag.MessaggioConferma = "Ordine Confermato";
                return RedirectToAction("Index", "Pizza");

            }


            return PartialView("_PartialViewConfermaOrdine");
        }

        public ActionResult PartialViewOrdineInConsegna()
        {
            Utente utente=db.Utente.Where(x=>x.Username== User.Identity.Name).FirstOrDefault();
            List<Ordine>ListaOrdineInCosegna=db.Ordine.Where(u=>u.IdUtente==utente.IDutente && u.OrdineInConsegnato==false).ToList();

            return PartialView("_PartialViewOrdineInConsegna",ListaOrdineInCosegna);
        }

        public JsonResult TotOrdine()
        {
            //mi prendo l username
            Utente utente = db.Utente.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            
            List<Ordine> Carrello = db.Ordine.Where(x => x.IdUtente == utente.IDutente).ToList();
            decimal totOrdine = 0;
            if (utente!=null)
            {

                foreach (var item in Carrello)
                {

                    decimal prezzoItem = (item.Pizza.Prezzo * item.Quantita);
                    totOrdine += prezzoItem;
                }
                

            }
            return Json(totOrdine, JsonRequestBehavior.AllowGet);
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
