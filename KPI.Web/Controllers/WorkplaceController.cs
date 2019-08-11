using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml.Table;
using OfficeOpenXml.Style;
using KPI.Web.Models;
using System.Drawing;
using MvcBreadCrumbs;
using KPI.Model.ViewModel;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class WorkplaceController : BaseController
    {
        // GET: Workplace
        [BreadCrumb(Clear =true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("Workplace");
            return View();
        }
        [HttpPost]
        public ActionResult Submit(FormCollection formCollection)
        {

            HttpPostedFileBase file = Request.Files["UploadedFile"];
            var datasList = new List<UploadDataVM>();
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                string fileName = file.FileName;
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;

                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var item = new UploadDataVM();
                        item.KPILevelCode = workSheet.Cells[rowIterator, 1].Value.ToSafetyString().ToUpper();
                        item.Value = workSheet.Cells[rowIterator, 2].Value.ToInt();
                        item.PeriodValue = workSheet.Cells[rowIterator, 3].Value.ToInt();
                        item.Year = workSheet.Cells[rowIterator, 4].Value.ToInt();
                        item.CreateTime = DateTime.Now;
                        datasList.Add(item);
                    }
                }

                return Json(new UploadDAO().Add(datasList), JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ExcelExport()
        {
            try
            {

                DataTable Dt = new DataTable();
                Dt.Columns.Add("KPILevel Code", typeof(string));
                Dt.Columns.Add("Value", typeof(string));
                Dt.Columns.Add("Period Value", typeof(string));
                Dt.Columns.Add("Year", typeof(string));
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;

                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(2).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public ActionResult Download()
        {


            if (Session["DownloadExcel_FileManager"] != null)
            {
                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                return File(data, "application/octet-stream", "DataUpload.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }


    }
}