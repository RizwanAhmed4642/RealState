using EStateDevelopment.Areas.Admin;
using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;


namespace EStateDevelopment.Areas.PropertyGuru.Controllers
{

    [Authorize(Roles = "Property Guru")]
    public class ProductController : Controller
    {
        QIGIEntities _db = new QIGIEntities();
        AdminActivity_Logs log = new AdminActivity_Logs();

        // GET: Property_Guru/Product
        public ActionResult AddProduct()
        {
            if (Session["UserId"] != null)
            {
                var areaofstock = _db.AreaOfStocks.ToList();
                ViewBag.AreaStock = new SelectList(areaofstock.ToList(), "Id", "Name");
                var data = _db.ProductTypes.ToList();
                ViewBag.ProductType = new SelectList(data.ToList(), "ProductTypeID", "TypeName");
                return View();
            }
            else
            {
                TempData["response"] = "Session Expired";
                return RedirectToAction("Login", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        public ActionResult AddProduct(ProductionModel model)
        {
            var areaofstock = _db.AreaOfStocks.ToList();
            ViewBag.AreaStock = new SelectList(areaofstock.ToList(), "Id", "Name");

            var data = _db.ProductTypes.ToList();
            ViewBag.ProductType = new SelectList(data.ToList(), "ProductTypeID", "TypeName");

            if (Session["UserId"] != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var proType = _db.ProductTypes.Find(model.ProductTypeID);
                        var astock = _db.AreaOfStocks.Find(Convert.ToInt32(model.AreaOfStock));

                        var userid = Session["UserId"].ToString();
                        var currentuser = _db.AspNetUsers.Find(Convert.ToInt32(userid));

                        // Image Get, path save and Check Code for View Model
                        string path = "";
                        var fileName = "";
                        byte[] imagebytes = null;
                        if (model.ImagePath != null)
                        {
                            var extentions = Path.GetExtension(model.ImagePath.FileName);

                            if (extentions == ".jpg" || extentions == ".jpeg" || extentions == ".png" || extentions == ".jfif")
                            {

                                if (model.ImagePath != null)
                                {
                                    var input = model.ImagePath.InputStream;
                                    byte[] byteData = null, buffer = new byte[input.Length];
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        int read;
                                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                                        {
                                            ms.Write(buffer, 0, read);
                                        }
                                        byteData = ms.ToArray();
                                    }
                                    imagebytes = byteData;
                                }
                                fileName = Path.GetFileName(model.ImagePath.FileName);

                                // Save File in a Folder using the following Path 
                                //path = Path.Combine(Server.MapPath("~/Areas/PropertyGuru/Doc/"), fileName);

                                var namef = Guid.NewGuid().ToString();
                                var Imagename = namef + ".jpg";

                                path = Server.MapPath("/Images/products/");
                                string p = path.Replace("/", @"\");
                                string imgPath = Path.Combine(p, Imagename);

                                byte[] bytes = imagebytes;
                                System.IO.File.WriteAllBytes(imgPath, bytes);
                                string sp = path.Replace(@"\", "/");
                                path = "http://localhost:52317/Images/products/" + Imagename;
                                // model.ImagePath.SaveAs(path);

                                //var filename = Path.GetFileName(model.ImagePath.FileName);
                                //path = Path.Combine(Server.MapPath("~/Areas/PropertyGuru/Doc/"), filename);
                                //model.ImagePath.SaveAs(path);
                            }
                            else
                            {
                                TempData["response"] = "Only Jpg, Jpeg and Png Allow";
                                return View();
                            }
                        }

                        var products = new Product()
                        {
                            Name = model.ProductName,
                            StartDate = model.ProductStartDate,
                            EndDate = model.ProductEndDate,
                            RateofReturn = model.RateofReturn,
                            Description = model.ProductDescription,
                            MaximumPeriod = model.MaximumPeriod,
                            MinimumPeriod = model.MinimumPeriod,
                            ProductTypeID = model.ProductTypeID,
                            ProductType = proType.TypeName,
                            AreaOfStock = astock.Name,
                            UserEmail = currentuser.Email,
                            UserID = currentuser.Id,
                            ApprovalStatus = false,
                            RejectedStatus = false,
                            Status = false,
                            CreatedDate = DateTime.Now,
                        };

                        var _Products = _db.Products.Add(products);
                        _db.SaveChanges();

                        if (model.ImagePath != null && path != "")
                        {
                            var Image = new ProductImage()
                            {
                                ProductID = _Products.ProductID,
                                ImagePath = path
                            };
                            _db.ProductImages.Add(Image);
                            _db.SaveChanges();
                        }

                        var collateraltype = new CollateralType()
                        {
                            Name = model.ColleteralDetail,
                            StartDate = model.ColletrallStartDate,
                            EndDate = model.ColletrallEndDate,
                            ColleteralDetail = model.ColleteralDetail,
                            CreatedBy = currentuser.Email,
                            CreatedDate = DateTime.Now,
                            ProductID = _Products.ProductID,
                            ProductName = _Products.Name,
                        };
                        _db.CollateralTypes.Add(collateraltype);
                        _db.SaveChanges();

                        var chargesSlab = new ChargesSlab()
                        {
                            SlabName = model.SlabName,
                            MinAmount = model.MinAmount,
                            MaxAmount = model.MaxAmount,
                            ChargesAmount = model.ChargesAmount,
                        };
                        _db.ChargesSlabs.Add(chargesSlab);
                        _db.SaveChanges();

                        var prochargestype = new ProductChargesType()
                        {
                            ChargesName = model.ChargesName,
                            StartDate = model.ChargesStartDate,
                            EndDate = model.ChargesEndDate,
                            ChargesSlabID = chargesSlab.ChargesSlabID,
                        };
                        _db.ProductChargesTypes.Add(prochargestype);
                        _db.SaveChanges();

                        var procharges = new ProductCharge()
                        {
                            ProductID = _Products.ProductID,
                            ProductName = _Products.Name,
                            PChargesTypeID = prochargestype.PChargesTypeID,
                        };
                        _db.ProductCharges.Add(procharges);
                        _db.SaveChanges();


                        var prointerestslab = new ProductInteresRateSlab()
                        {
                            ProductID = _Products.ProductID,
                            ProductName = _Products.Name,
                            PChargesID = procharges.PChargesID,
                            IntrestRate = model.IntrestRate,
                            Tenor = model.Tenor,
                            CreatedDate = DateTime.Now,
                            CreateBy = currentuser.Email,
                        };
                        _db.ProductInteresRateSlabs.Add(prointerestslab);
                        _db.SaveChanges();

                        log.AddLog("Add", "Add Product in Application");
                        TempData["response"] = "Product Add Successfully.";


                        /////Sending Email to user
                        //var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p> Your request for product " + model.Name + " approval has send to administrator, You will be notified shortly.</p></div>";
                        //EmailSend _emaiSendForAdmin = new EmailSend();
                        ////_emailsend.Message = "You have successfully Applied for Booking";
                        //_emaiSendForAdmin.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                        //var _emailAdmin = ConfigurationManager.AppSettings["MENAFATF_Email"];
                        //var _password = "qigi@123"; //ConfigurationManager.AppSettings["From_Email_Password"];
                        //var _disputeName = "QIGI Administration";
                        //MailMessage mymessageAdmin = new MailMessage();
                        //mymessageAdmin.To.Add(currentuser.Email);
                        //mymessageAdmin.From = new MailAddress(_emailAdmin, _disputeName);
                        //mymessageAdmin.Subject = "Product Request Submitted";
                        //mymessageAdmin.Body = _msgForAdmin;
                        //mymessageAdmin.IsBodyHtml = true;
                        //using (SmtpClient smtp = new SmtpClient())
                        //{
                        //    smtp.EnableSsl = true;
                        //    smtp.Host = ConfigurationManager.AppSettings["SMTP"];
                        //    smtp.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTP_Port"]);
                        //    smtp.UseDefaultCredentials = false;
                        //    smtp.Credentials = new System.Net.NetworkCredential(_emailAdmin, _password);

                        ////    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        ////    smtp.Send(mymessageAdmin);

                        //}
                        return RedirectToAction("Dashboard", "Product", new { area = "PropertyGuru" });

                    }
                    catch (Exception ex)
                    {
                        TempData["response"] = "Oops Error " + ex.Message;
                        return RedirectToAction("Home", "Dashboard", new { area = "PropertyGuru" });
                    }
                }
                else
                {
                    TempData["response"] = "Oops Error " + ModelState.Values;
                    return RedirectToAction("Home", "Dashboard", new { area = "PropertyGuru" });
                }
            }
            else
            {
                TempData["response"] = "Session Expired";
                return RedirectToAction("Login", "Home", new { area = "Public" });
            }
        }



        public ActionResult Dashboard()
        {
            if (Session["UserId"] != null)
            {
                var userid = Session["UserId"].ToString();
                var currentuser = _db.AspNetUsers.Find(Convert.ToInt32(userid));
                List<ApplicationModel> applications = new List<ApplicationModel>();

                var users = _db.AspNetUsers.ToList();
                var charges = _db.ChargesSlabs.ToList();
                var chargesType = _db.ProductChargesTypes.ToList();
                var Pcharges = _db.ProductCharges.ToList();
                var products = _db.Products.ToList().Where(x => x.UserID == currentuser.Id).ToList();

                for (int i = 0; i < chargesType.Count(); i++)
                {
                    foreach (var item in charges.ToList().Where(x => x.ChargesSlabID == chargesType[i].ChargesSlabID).ToList())
                    {
                        chargesType[i].ChargesSlabs = new List<ChargesSlab>();
                        chargesType[i].ChargesSlabs.Add(item);
                    }
                }

                for (int i = 0; i < Pcharges.Count(); i++)
                {
                    foreach (var items in chargesType.ToList().Where(c => c.PChargesTypeID == Pcharges[i].PChargesID).ToList())
                    {
                        Pcharges[i].ProductChargesType = items;
                    }
                }

                for (int i = 0; i < products.Count(); i++)
                {
                    foreach (var items in Pcharges.ToList().Where(p => p.ProductID == products[i].ProductID).ToList())
                    {
                        products[i].ProductCharges = new List<ProductCharge>();
                        products[i].ProductCharges.Add(items);
                    }

                    foreach (var items in users.ToList().Where(u => u.Id == products[i].UserID).ToList())
                    {
                        products[i].AspNetUsers = new List<AspNetUser>();
                        products[i].AspNetUsers.Add(items);
                    }
                }

                var borrowerapp = _db.BorrowerApplications.ToList();
                var investorapp = _db.InvestorApplications.ToList();
                for (int i = 0; i < borrowerapp.Count(); i++)
                {
                    var product = _db.Products.Find(borrowerapp[i].ProductID);
                    if (product != null)
                    {
                        ApplicationModel obj = new ApplicationModel();
                        obj.Name = borrowerapp[i].FullName;
                        obj.ApplicationID = borrowerapp[i].BorrowerApplicationID;
                        obj.ProductName = product.Name;
                        obj.RateofReturn = product.RateofReturn;
                        obj.Phone = borrowerapp[i].Phone;
                        obj.Email = borrowerapp[i].Email;
                        obj.ProductID = (int)borrowerapp[i].ProductID;
                        obj.ApplicationNumber = borrowerapp[i].ApplicationNumber;
                        obj.personType = "Borrower";
                        applications.Add(obj);
                    }
                }

                //for(int i=0; i< investorapp.Count(); i++)
                //{
                //    var product = _db.Products.Find(borrowerapp[i].ProductID);
                //    ApplicationModel obj = new ApplicationModel();

                //    obj.Name = investorapp[i].FullName;
                //    obj.ApplicationID = investorapp[i].ApplicationNumber;
                //    obj.ProductName = product.Name;
                //    obj.RateofReturn = product.RateofReturn;
                //    obj.Phone = investorapp[i].Phone;
                //    obj.Email = borrowerapp[i].Email;
                //    obj.ProductID = (int)investorapp[i].ProductID;
                //    obj.personType = "Investor";
                //    applications.Add(obj);
                //}

                foreach (InvestorApplication i in investorapp)
                {
                    var product = _db.Products.Find(i.ProductID);
                    if (product != null)
                    {
                        applications.Add(new ApplicationModel()
                        {
                            Name = i.FullName,
                            ApplicationID = i.InvestorApplicationID,
                            ProductName = product.Name,
                            RateofReturn = product.RateofReturn,
                            Phone = i.Phone,
                            Email = i.Email,
                            ProductID = (int)i.ProductID,
                            ApplicationNumber = i.ApplicationNumber,
                            personType = "Investor",
                        });
                    }
                }

                foreach (Product p in products)
                {
                    p.ApplicationModels = new List<ApplicationModel>();
                    foreach (var items in applications.Where(a => a.ProductID == p.ProductID))
                    {

                        p.ApplicationModels.Add(items);
                    }
                }

                return View(products);
            }
            else
            {
                TempData["response"] = "Session Expired";
                return RedirectToAction("Login", "Home", new { area = "Public" });
            }
        }
        public ActionResult Details(int id)
        {
            try
            {
                var users = _db.AspNetUsers.ToList();
                var charges = _db.ChargesSlabs.ToList();
                var chargesType = _db.ProductChargesTypes.ToList();
                var Pcharges = _db.ProductCharges.ToList();

                var images = _db.ProductImages.ToList().Where(i => i.ProductID == id);

                var colletralTypes = _db.CollateralTypes.ToList();

                var products = _db.Products.Find(id);

                for (int i = 0; i < chargesType.Count(); i++)
                {
                    foreach (var item in charges.ToList().Where(x => x.ChargesSlabID == chargesType[i].ChargesSlabID).ToList())
                    {
                        chargesType[i].ChargesSlabs = new List<ChargesSlab>();
                        chargesType[i].ChargesSlabs.Add(item);
                    }
                }

                for (int i = 0; i < Pcharges.Count(); i++)
                {
                    foreach (var items in chargesType.ToList().Where(c => c.PChargesTypeID == Pcharges[i].PChargesID).ToList())
                    {
                        Pcharges[i].ProductChargesType = items;
                    }
                }

                foreach (var items in Pcharges.ToList().Where(p => p.ProductID == products.ProductID).ToList())
                {
                    products.ProductCharges = new List<ProductCharge>();
                    products.ProductCharges.Add(items);
                }

                foreach (var items in users.ToList().Where(u => u.Id == products.UserID).ToList())
                {
                    products.AspNetUsers = new List<AspNetUser>();
                    products.AspNetUsers.Add(items);
                }

                foreach (var item in colletralTypes.ToList().Where(c => c.ProductID == products.ProductID).ToList())
                {
                    products.CollateralTypes = new List<CollateralType>();
                    products.CollateralTypes.Add(item);
                }

                foreach (var item in images.ToList().Where(i => i.ProductID == products.ProductID).ToList())
                {
                    products.ProductImages = new List<ProductImage>();
                    products.ProductImages.Add(item);

                }

                return View(products);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Home", "Dashboard", new { area = "PropertyGuru" });
            }
        }

        public ActionResult ApplicationDetails(int? id, int? appID, string appnumber)
        {
            if (id == null || appID == null || appnumber == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                try
                {
                    List<ApplicationModel> applications = new List<ApplicationModel>();

                    var users = _db.AspNetUsers.ToList();
                    var charges = _db.ChargesSlabs.ToList();
                    var chargesType = _db.ProductChargesTypes.ToList();
                    var Pcharges = _db.ProductCharges.ToList();

                    var collateraltype = _db.CollateralTypes.ToList();

                    var products = _db.Products.Find(id);

                    for (int i = 0; i < chargesType.Count(); i++)
                    {
                        foreach (var item in charges.ToList().Where(x => x.ChargesSlabID == chargesType[i].ChargesSlabID).ToList())
                        {
                            chargesType[i].ChargesSlabs = new List<ChargesSlab>();
                            chargesType[i].ChargesSlabs.Add(item);
                        }
                    }

                    for (int i = 0; i < Pcharges.Count(); i++)
                    {
                        foreach (var items in chargesType.ToList().Where(c => c.PChargesTypeID == Pcharges[i].PChargesID).ToList())
                        {
                            Pcharges[i].ProductChargesType = items;
                        }
                    }

                    foreach (var items in Pcharges.ToList().Where(p => p.ProductID == products.ProductID).ToList())
                    {
                        products.ProductCharges = new List<ProductCharge>();
                        products.ProductCharges.Add(items);
                    }

                    foreach (var items in users.ToList().Where(u => u.Id == products.UserID).ToList())
                    {
                        products.AspNetUsers = new List<AspNetUser>();
                        products.AspNetUsers.Add(items);
                    }

                    foreach (var item in collateraltype.ToList().Where(c => c.ProductID == products.ProductID).ToList())
                    {
                        products.CollateralTypes = new List<CollateralType>();
                        products.CollateralTypes.Add(item);
                    }

                    var borrowerapp = _db.BorrowerApplications.ToList().Where(x => x.ProductID == id && x.BorrowerApplicationID == appID && x.ApplicationNumber == Convert.ToString(appnumber)).ToList();
                    var investorapp = _db.InvestorApplications.ToList().Where(x => x.ProductID == id && x.InvestorApplicationID == appID && x.ApplicationNumber == Convert.ToString(appnumber)).ToList();
                    for (int i = 0; i < borrowerapp.Count(); i++)
                    {
                        var product = _db.Products.Find(products.ProductID);
                        ApplicationModel obj = new ApplicationModel();

                        obj.Name = borrowerapp[i].FullName;
                        obj.ApplicationID = borrowerapp[i].BorrowerApplicationID;
                        obj.ApplicationNumber = borrowerapp[i].ApplicationNumber;
                        obj.Phone = borrowerapp[i].Phone;
                        obj.Email = borrowerapp[i].Email;
                        obj.ProductID = (int)borrowerapp[i].ProductID;
                        obj.personType = "Borrower";
                        obj.AppDescription = borrowerapp[i].Descripation;
                        obj.AppAdress = borrowerapp[i].CompleteAddress;
                        obj.AppApprovalStatus = borrowerapp[i].ApprovalStatus;
                        obj.AppRejectedStatus = borrowerapp[i].RejectedStatus;
                        obj.city = borrowerapp[i].City;
                        obj.state = borrowerapp[i].State;



                        obj.ProductName = product.Name;
                        obj.ChargesAmount = products.ProductCharges.ElementAt(0).ProductChargesType.ChargesSlabs.ElementAt(0).ChargesAmount;
                        obj.RateofReturn = product.RateofReturn;
                        obj.Description = product.Description;
                        obj.CreatedDate = product.CreatedDate;
                        obj.ProductType = product.ProductType;
                        obj.StartDate = product.StartDate;
                        obj.EndDate = product.EndDate;
                        obj.PublishedDate = product.PublishedDate;
                        obj.ApprovalStatus = product.ApprovalStatus;
                        obj.ProductStatus = product.ProductStatus;
                        obj.MaximumPeriod = product.MaximumPeriod;
                        obj.MinimumPeriod = product.MinimumPeriod;
                        applications.Add(obj);
                    }

                    foreach (InvestorApplication i in investorapp)
                    {
                        var product = _db.Products.Find(i.ProductID);

                        applications.Add(new ApplicationModel()
                        {
                            Name = i.FullName,
                            ApplicationID = i.InvestorApplicationID,
                            ApplicationNumber = i.ApplicationNumber,
                            Phone = i.Phone,
                            Email = i.Email,
                            ProductID = (int)i.ProductID,
                            personType = "Investor",
                            AppDescription = i.Descripation,
                            AppAdress = i.CompleteAddress,
                            AppApprovalStatus = i.ApprovalStatus,
                            AppRejectedStatus = i.RejectedStatus,
                            city = i.City,
                            state = i.State,

                            ProductName = product.Name,
                            ChargesAmount = products.ProductCharges.ElementAt(0).ProductChargesType.ChargesSlabs.ElementAt(0).ChargesAmount,
                            RateofReturn = product.RateofReturn,
                            Description = product.Description,
                            CreatedDate = product.CreatedDate,
                            ProductType = product.ProductType,
                            StartDate = product.StartDate,
                            EndDate = product.EndDate,
                            PublishedDate = product.PublishedDate,
                            ApprovalStatus = product.ApprovalStatus,
                            ProductStatus = product.ProductStatus,
                            MaximumPeriod = product.MaximumPeriod,
                            MinimumPeriod = product.MinimumPeriod,
                        });
                    }
                    products.ApplicationModels = new List<ApplicationModel>();
                    foreach (var items in applications.Where(a => a.ProductID == products.ProductID && a.ApplicationID == appID))
                    {

                        products.ApplicationModels.Add(items);
                    }

                    return View(products);
                }
                catch (Exception ex)
                {
                    TempData["response"] = "Oops Error " + ex.Message;
                    return RedirectToAction("Home", "Dashboard", new { area = "PropertyGuru" });
                }
            }

        }

        public ActionResult PendingAssesment()
        {
            return View();
        }

        public ActionResult LiveProducts()
        {
            return View();
        }

        public ActionResult ProductApplicationStagewise()
        {
            return View();
        }

        public ActionResult AttachDocument()
        {
            var data = _db.ProductDocumentTypes.ToList();
            ViewBag.ProductDocumentTypes = new SelectList(data.ToList(), "ProductDocumentTypeID", "DocumentType");

            var charges = _db.ChargesSlabs.ToList();
            var chargesType = _db.ProductChargesTypes.ToList();
            var Pcharges = _db.ProductCharges.ToList();


            var products = _db.Products.ToList();

            for (int i = 0; i < chargesType.Count(); i++)
            {
                foreach (var item in charges.ToList().Where(c => c.ChargesSlabID == chargesType[i].ChargesSlabID).ToList())
                {
                    chargesType[i].ChargesSlabs = new List<ChargesSlab>();
                    chargesType[i].ChargesSlabs.Add(item);
                }
            }

            for (int i = 0; i < Pcharges.Count(); i++)
            {
                foreach (var item in chargesType.ToList().Where(pc => pc.PChargesTypeID == Pcharges[i].PChargesTypeID).ToList())
                {
                    Pcharges[i].ProductChargesType = item;
                }
            }

            for (int i = 0; i < products.Count(); i++)
            {
                foreach (var item in Pcharges.ToList().Where(p => p.ProductID == products[i].ProductID).ToList())
                {
                    products[i].ProductCharges = new List<ProductCharge>();
                    products[i].ProductCharges.Add(item);
                }
            }

            return View(products);
        }


        [HttpPost]
        public ActionResult uploadDocument(FormCollection formCollection)
        {

            var document = new ProductDocument
            {
                ProductID = int.Parse(formCollection["productid"]),
                ProductDocumentTypeID = int.Parse(formCollection["prodocid"]),
                UploadedDocument = formCollection["docName"],
            };

            var files = Request.Files;

            foreach (string str in files)
            {
                HttpPostedFileBase file = Request.Files[str] as HttpPostedFileBase;



                var data = _db.ProductDocumentTypes.ToList();
                ViewBag.ProductDocumentTypes = new SelectList(data.ToList(), "ProductDocumentTypeID", "DocumentType");



                //if (Request.Files.Count > 0)
                //{
                //    HttpPostedFileBase file = Request.Files[0];

                var fileName = Path.GetFileName(file.FileName);
                document.UploadedDocumentPath = Path.Combine(Server.MapPath("~/Areas/PropertyGuru/Doc/"), fileName);
                file.SaveAs(document.UploadedDocumentPath);

                _db.ProductDocuments.Add(document);
                _db.SaveChanges();
            }

            return RedirectToAction("AttachDocument");
        }

    }
}