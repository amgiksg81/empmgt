using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModels;
using DomainModels.Entities;
using BAL.Abstraction;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {        
        public CategoryController(IUnitOfWork _uof): base(_uof)
        {
        }

        public ActionResult Index()
        {
            return View(uof.CategoryRepo.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category model)
        {
            try
            {
                uof.CategoryRepo.Add(model);
                uof.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View(uof.CategoryRepo.GetById(id));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(Category model)
        {
            try
            {
                model.UpdatedDate = DateTime.Now;
                uof.CategoryRepo.Modify(model);
                uof.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            uof.CategoryRepo.DeleteById(id);
            uof.SaveChanges();
            return RedirectToAction("index");
        }
    }
}