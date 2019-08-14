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
        [BreadCrumb(Clear = true)]
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
                        item.KPIName = workSheet.Cells[rowIterator, 2].Value.ToSafetyString().ToUpper();
                        item.Value = workSheet.Cells[rowIterator, 3].Value.ToInt();
                        item.PeriodValue = workSheet.Cells[rowIterator, 4].Value.ToInt();
                        item.Year = workSheet.Cells[rowIterator, 5].Value.ToInt();
                        item.Area = workSheet.Cells[rowIterator, 6].Value.ToSafetyString();
                        item.UpdateTime = workSheet.Cells[rowIterator, 7].Value.ToSafetyString().Trim();
                        item.Remark = workSheet.Cells[rowIterator, 8].Value.ToSafetyString();

                        item.CreateTime = DateTime.Now;
                        datasList.Add(item);
                    }
                }

                return Json(new UploadDAO().Add(datasList), JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ExcelExport(int userid)
        {
            var model = new UploadDAO().DataExport(userid);
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();
            try
            {
                DataTable Dt = new DataTable();
                Dt.Columns.Add("KPILevel Code", typeof(string));
                Dt.Columns.Add("KPI Name", typeof(string));
                Dt.Columns.Add("Value", typeof(int));
                Dt.Columns.Add("Period Value", typeof(string));
                Dt.Columns.Add("Year", typeof(int));
                Dt.Columns.Add("Area", typeof(string));
                Dt.Columns.Add("Update Time", typeof(object));
                Dt.Columns.Add("Remark", typeof(string));
                foreach (var item in model)
                {
                    if (item.PeriodValueW <= currentWeek && item.StateW)
                    {
                        for (int i = item.PeriodValueW.Value; i <= currentWeek; i++)
                        {
                            Dt.Rows.Add(item.KPILevelCode + "W", item.KPIName, item.Value, item.PeriodValueW ++, item.Year, item.Area, item.UploadTimeW.ToSafetyString(), item.Remark);
                        }
                    }
                   
                    if (item.PeriodValueM <= currentMonth && item.StateM)
                    {
                        for (int i = item.PeriodValueM.Value; i <= currentMonth; i++)
                        {
                            Dt.Rows.Add(item.KPILevelCode + "M", item.KPIName, item.Value, item.PeriodValueM++, item.Year, item.Area, item.UploadTimeM.Value.ToSafetyString().Split(' ')[0], item.Remark);
                        }
                    }
                   
                    if (item.PeriodValueQ <= currentQuarter && item.StateQ)
                    {
                        for (int i = item.PeriodValueQ.Value; i <= currentQuarter; i++)
                        {
                            Dt.Rows.Add(item.KPILevelCode + "Q", item.KPIName, item.Value, item.PeriodValueQ++, item.Year, item.Area, item.UploadTimeQ.ToSafetyString().Split(' ')[0], item.Remark);
                        }
                    }
                    
                    if (item.PeriodValueY <= currentYear && item.StateY)
                    {
                        for (int i = item.PeriodValueY.Value; i <= currentYear; i++)
                        {
                            Dt.Rows.Add(item.KPILevelCode + "Y", item.KPIName, item.Value, item.PeriodValueY, item.Year++, item.Area, item.UploadTimeY.ToSafetyString().Split(' ')[0], item.Remark);
                        }
                    }
                }
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
                var message = ex.Message;
                return Json("", JsonRequestBehavior.AllowGet);
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

        public JsonResult UpLoadKPILevel(int userid, int page, int pageSize)
        {
            return Json(new UploadDAO().UpLoadKPILevel(userid, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpLoadKPILevelTrack(int userid, int page, int pageSize)
        {
            return Json(new UploadDAO().UpLoadKPILevelTrack(userid, page, pageSize), JsonRequestBehavior.AllowGet);
        }
    }
}