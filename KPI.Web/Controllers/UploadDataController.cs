using ExcelDataReader;
using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Web.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace KPI.Web.Controllers
{
    public class UploadDataController : BaseController
    {
        // GET: UploadData
        public ActionResult Index()
        {
            return View();
        }
      
    }
}