﻿using NhomXingfa.Areas.Quantri.Models.DataModels;
using NhomXingfa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomXingfa.Controllers
{
    public class ProductController : Controller
    {
        XingFaEntities db = new XingFaEntities();
        // GET: Product
        public ActionResult Index(int? id)
        {
            ProductViewModel model = new ProductViewModel();

            model.categories = db.Categories.Where(q => q.IsActive == true && q.TypeCate == 1).ToList();
            model.listduan = db.Products.Where(q => q.IsActive == true && q.ProductCode == "BST").OrderByDescending(o => o.ProductID).Take(5).ToList();

            if(id == null)
            {
                model.isAll = true;
                model.product = db.Products.Where(q => q.IsActive == true && q.IsProduct == true).OrderByDescending(p=>p.ProductID).ToList();
                model.SEOTitle = "Sản phẩm nội thất chung cư Bình Dương";
                model.SEOKeywords = "Nội thất chung cư Bình Dương, Nội thất giá rẻ tại Bình Dương";
                model.SEOMetadescription = "Dịch vụ thi công nội thất Tân thời cung cấp các sản phẩm nội thất giá rẻ chất lượng, sản phẩm nội thất chung cư tại Bình Dương";
            }
            //else if (model.category.Parent == 0)
            //{
            //    model.isAll = false;
            //    model.product = db.Products.Where(q => q.IsActive == true && q.IsProduct == true && q.CategoryIDParent == id).ToList();
            //    model.category = db.Categories.Find(id);
            //}
            else
            {
                model.isAll = false;
                model.product = db.Products.Where(q => q.IsActive == true && q.IsProduct == true && q.CategoryID == id).OrderByDescending(p => p.ProductID).ToList();
                model.category = db.Categories.Find(id);

                model.SEOTitle = model.category.SEOTitle;
                model.SEOKeywords = model.category.SEOKeywords;
                model.SEOMetadescription = model.category.SEOMetadescription;
            }
            return View(model);
        }

        public ActionResult Detail(int? id)
        {
            var model = new DetailProductViewModel();

            List<ImageData> listimage = new List<ImageData>();

            model.product = db.Products.Find(id);
            model.category = db.Categories.Find(model.product.CategoryID);
            var list = db.ProductImages.Where(q => q.ProductID == id).ToList();

            foreach(var q in list)
            {
                ImageData i = new ImageData();
                i.urlimg = q.URLImage;
                i.urlthumb = q.ImagesThumb;
                i.title = q.Title;
                listimage.Add(i);
            }

            ImageData k = new ImageData();
            k.urlimg = model.product.Images;
            k.urlthumb = model.product.ImagesThumb;
            k.title = model.product.SEOTitle;
            listimage.Add(k);
            model.listimage = listimage;
            model.lstProductSameCate = db.Products.Where(p => p.IsProduct == true && p.IsActive == true && p.CategoryID == model.product.CategoryID).ToList();

            return View(model);
        }
    }
}