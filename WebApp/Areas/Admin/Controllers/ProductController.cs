using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using DomainModels.Entities;
using BAL.Abstraction;
using DomainModels.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork _uof) : base(_uof)
        {
        }

        void BindCategory()
        {
            ViewBag.CategoryList = uof.CategoryRepo.GetAll();
        }
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(uof.ProductRepo.GetAll());
        }

        public ActionResult Create()
        {
            BindCategory();
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(ProductModel model)
        {
            try
            {
                string folderPath = "~/Uploads/"; // your code goes here
                bool exists = Directory.Exists(Server.MapPath(folderPath));
                if (!exists)
                    Directory.CreateDirectory(Server.MapPath(folderPath));

                //saving file
                var fileName = Path.GetFileName(model.file.FileName);
                var path = Path.Combine(Server.MapPath(folderPath), fileName);
                model.file.SaveAs(path);

                model.ImageName = fileName;
                model.ImagePath = "/Uploads/" + fileName;

                Product data = new Product();
                data.ProductId = model.ProductId;
                data.Name = model.Name;
                data.UnitPrice = model.UnitPrice;
                data.CategoryId = model.CategoryId;
                data.Description = model.Description;
                data.ImagePath = model.ImagePath;
                data.ImageName = model.ImageName;

                uof.ProductRepo.Add(data);
                uof.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            BindCategory();
            return View();
        }

        public ActionResult Edit(int id)
        {
            BindCategory();
            Product data = uof.ProductRepo.GetById(id);
            ProductModel model = new ProductModel();
            if (data != null)
            {
                model.ProductId = data.ProductId;
                model.Name = data.Name;
                model.UnitPrice = data.UnitPrice;
                model.CategoryId = data.CategoryId;
                model.Description = data.Description;
                model.ImagePath = data.ImagePath;
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(ProductModel model)
        {
            try
            {
                if (model.file != null)
                {
                    //deleting previous one
                    var filePath = Server.MapPath(model.ImagePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    //uploading new one
                    var fileName = Path.GetFileName(model.file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                    model.file.SaveAs(path);

                    model.ImageName = fileName;
                    model.ImagePath = "/Uploads/" + fileName;
                }
                Product data = new Product();
                data.ProductId = model.ProductId;
                data.Name = model.Name;
                data.UnitPrice = model.UnitPrice;
                data.CategoryId = model.CategoryId;
                data.Description = model.Description;
                data.ImagePath = model.ImagePath;
                if (model.ImageName != null)
                    data.ImageName = model.ImageName;

                uof.ProductRepo.Modify(data);
                uof.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            BindCategory();
            return View();
        }

        public ActionResult Delete(int id, string file)
        {
            uof.ProductRepo.DeleteById(id);
            var filePath = Server.MapPath(file);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            uof.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}