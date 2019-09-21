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
using System.Configuration;
using System.Net.Mail;

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
            //var datasList2 = new List<UploadDataVM2>();
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
                        //item.KPIName = workSheet.Cells[rowIterator, 2].Value.ToSafetyString().ToUpper();
                        item.Value = workSheet.Cells[rowIterator, 3].Value.ToSafetyString();
                        item.TargetValue = workSheet.Cells[rowIterator, 4].Value.ToDouble();
                        item.PeriodValue = workSheet.Cells[rowIterator, 5].Value.ToInt();
                        //item.Year = workSheet.Cells[rowIterator, 6].Value.ToInt();
                        //item.Area = workSheet.Cells[rowIterator, 7].Value.ToSafetyString();
                        //item.UpdateTime = workSheet.Cells[rowIterator, 8].Value.ToSafetyString().Trim();
                        //item.Remark = workSheet.Cells[rowIterator, 8].Value.ToSafetyString();

                        item.CreateTime = DateTime.Now;
                        datasList.Add(item);
                    }
                }
                var model = new UploadDAO().UploadData(datasList);
                if (model.ListUploadKPIVMs.Count > 0)
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Templates/newEmail.html"));
                    var html = string.Empty;
                    foreach (var item in model.ListUploadKPIVMs)
                    {
                        html += @"<tr>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{area}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{kpiname}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{week}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{month}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{quarter}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{year}}</td>
                         </tr>"
                         .Replace("{{area}}", item.Area)
                         .Replace("{{kpiname}}", item.KPIName)
                         .Replace("{{week}}", item.Week.ToSafetyString())
                         .Replace("{{month}}", item.Month.ToSafetyString())
                         .Replace("{{quarter}}", item.Quarter.ToSafetyString())
                         .Replace("{{year}}", item.Year.ToSafetyString());

                    }
                    content = content.Replace("{{{html-template}}}", html);
                    var sessionUser = Session["UserProfile"] as UserProfileVM;
                    string from = ConfigurationManager.AppSettings["FromEmailAddress"].ToSafetyString();
                    string password = ConfigurationManager.AppSettings["FromEmailPassword"].ToSafetyString();
                    string to = sessionUser.User.Email;
                    if (sessionUser.User.Email.IsEmailFormat())
                    {
                        to = sessionUser.User.Email.ToSafetyString();
                    }

                    string subject = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToSafetyString();

                    MailMessage mail = new MailMessage();
                    mail.To.Add(to.ToString());
                    mail.From = new MailAddress(from, "KPI.App");
                    mail.Subject = subject;
                    mail.Body = content;
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.Priority = MailPriority.High;

                    try
                    {
                        using (var smtp = new SmtpClient())
                        {
                            smtp.Send(mail);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    return Json(model.Status, JsonRequestBehavior.AllowGet);
                }
                return Json(model.Status, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ExcelExport1(int userid)
        {
            var model = new UploadDAO().DataExport(userid);
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();

            var now = DateTime.Now;
            var end = now.GetEndOfQuarter();
            var tt = end.Subtract(now).Days;
            //var targetValue = "";

            DataTable Dt = new DataTable();
            Dt.Columns.Add("KPILevel Code", typeof(string));
            Dt.Columns.Add("KPI Name", typeof(string));
            Dt.Columns.Add("Actual Value", typeof(string));
            Dt.Columns.Add("Target Value", typeof(object));
            Dt.Columns.Add("Period Value", typeof(string));
            Dt.Columns.Add("Year", typeof(int));
            Dt.Columns.Add("Area", typeof(string));
            Dt.Columns.Add("Update Time", typeof(object));
            Dt.Columns.Add("Start Date", typeof(string));
            Dt.Columns.Add("End Date", typeof(string));
            foreach (var item in model)
            {
                ///Logic export tuần
                //Nếu tuần MAX trong bảng DATA mà bằng 0 thì export từ tuần đầu cho đến tuần hiện tại
                if (item.PeriodValueW == 0 && item.StateW == true)
                {

                    for (int i = 1; i <= currentWeek; i++)
                    {
                        var startDayOfWeek = CodeUtility.ToGetMondayOfWeek(currentYear, i).ToString("MM/dd/yyyy");
                        var endDayOfWeek = CodeUtility.ToGetSaturdayOfWeek(currentYear, i).ToString("MM/dd/yyyy");

                        var updateTimeW = item.UploadTimeW.ConvertNumberDayOfWeekToString() + ", Week " + i;
                        Dt.Rows.Add(item.KPILevelCode + "W", item.KPIName, item.Value, item.TargetValueW, i, currentYear, item.Area, updateTimeW, startDayOfWeek, endDayOfWeek);
                    }
                }
                // nếu tuần hiện tại trừ tuần MAX >= 1 thì export từ tuần kế tiếp tuần MAX cho đến tuần hiện tại
                else if (item.PeriodValueW > 0 && item.StateW == true)
                {
                    if (currentWeek - item.PeriodValueW >= 1)
                    {

                        for (int i = item.PeriodValueW + 1; i <= currentWeek; i++)
                        {
                            var startDayOfWeek = CodeUtility.ToGetMondayOfWeek(currentYear, i).ToString("MM/dd/yyyy");
                            var endDayOfWeek = CodeUtility.ToGetSaturdayOfWeek(currentYear, i).ToString("MM/dd/yyyy");
                            var updateTimeW = item.UploadTimeW.ConvertNumberDayOfWeekToString() + ", Week " + i;
                            Dt.Rows.Add(item.KPILevelCode + "W", item.KPIName, item.Value, item.TargetValueW, i, currentYear, item.Area, updateTimeW, startDayOfWeek, endDayOfWeek);
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= currentWeek; i++)
                        {
                            var startDayOfWeek = CodeUtility.ToGetMondayOfWeek(currentYear, i).ToString("MM/dd/yyyy");
                            var endDayOfWeek = CodeUtility.ToGetSaturdayOfWeek(currentYear, i).ToString("MM/dd/yyyy");

                            var updateTimeW = item.UploadTimeW.ConvertNumberDayOfWeekToString() + ", Week " + i;
                            var value = new UploadDAO().GetValueData(item.KPILevelCode, "W", i);
                            Dt.Rows.Add(item.KPILevelCode + "W", item.KPIName, value, item.TargetValueW, i, currentYear, item.Area, updateTimeW, startDayOfWeek, endDayOfWeek);
                        }
                    }
                }


                ///Logic export tháng
                //Nếu tháng MAX trong bảng DATA mà bằng 0 thì export từ tháng đầu cho đến tháng hiện tại

                if (item.PeriodValueM == 0 && item.StateM == true)
                {
                    for (int i = 1; i <= currentMonth; i++)
                    {
                        Dt.Rows.Add(item.KPILevelCode + "M", item.KPIName, item.Value, item.TargetValueW, i, currentYear, item.Area, item.UploadTimeM.ToSafetyString().Split(' ')[0].ToSafetyString());
                    }
                }
                // nếu tháng hiện tại trừ tháng MAX >= 1 thì export từ tháng kế tiếp tháng MAX cho đến tháng hiện tại
                if (item.PeriodValueM > 0 && item.StateM == true)
                {
                    if (currentMonth - item.PeriodValueM > 1)
                    {

                        for (int i = item.PeriodValueM + 1; i <= currentMonth; i++)
                        {

                            Dt.Rows.Add(item.KPILevelCode + "M", item.KPIName, item.Value, item.TargetValueW, i, currentYear, item.Area, item.UploadTimeM.ToSafetyString().Split(' ')[0].ToSafetyString());
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= currentMonth; i++)
                        {
                            var value = new UploadDAO().GetValueData(item.KPILevelCode, "M", i);
                            Dt.Rows.Add(item.KPILevelCode + "M", item.KPIName, value, item.TargetValueW, i, currentYear, item.Area, item.UploadTimeM.ToSafetyString().Split(' ')[0].ToSafetyString());
                        }
                    }

                }
                ///Logic export quý
                //Nếu quý MAX trong bảng DATA mà bằng 0 thì export từ tháng đầu cho đến quý hiện tại
                if (item.PeriodValueQ == 0 && item.StateQ == true)
                {
                    for (int i = 1; i < currentQuarter; i++)
                    {
                        Dt.Rows.Add(item.KPILevelCode + "Q", item.KPIName, item.Value, item.TargetValueW, i, currentYear, item.Area, item.UploadTimeQ.ToSafetyString().Split(' ')[0].ToSafetyString());
                    }
                    if (tt <= 30)
                    {
                        Dt.Rows.Add(item.KPILevelCode + "Q", item.KPIName, item.Value, item.TargetValueW, currentQuarter, currentYear, item.Area, item.UploadTimeQ.ToSafetyString().Split(' ')[0].ToSafetyString());
                    }
                }
                if (item.PeriodValueQ > 0 && item.StateQ == true)
                {

                    //, Nếu quý hiện tại trừ quý MAX trong bảng DATA lớn hơn 1 thì 
                    //export từ quý 1 cho đến quý hiện tại
                    if (currentQuarter - item.PeriodValueQ >= 1)
                    {

                        for (int i = item.PeriodValueQ; i <= currentQuarter; i++)
                        {
                            Dt.Rows.Add(item.KPILevelCode + "Q", item.KPIName, item.Value, item.TargetValueW, i, currentYear, item.Area, item.UploadTimeQ.ToSafetyString().Split(' ')[0].ToSafetyString());
                        }

                    }
                    else
                    {
                        for (int i = 1; i <= currentQuarter; i++)
                        {
                            var value = new UploadDAO().GetValueData(item.KPILevelCode, "Q", i);
                            Dt.Rows.Add(item.KPILevelCode + "Q", item.KPIName, value, item.TargetValueW, i, currentYear, item.Area, item.UploadTimeQ.ToSafetyString().Split(' ')[0].ToSafetyString());
                        }
                    }

                }

                ///Logic export năm
                //Nếu năm MAX trong bảng DATA == 0 thì export năm hiện tại
                if (item.PeriodValueY == 0 && item.StateY == true)
                {
                    for (int i = currentYear - 10; i <= currentYear; i++)
                    {
                        Dt.Rows.Add(item.KPILevelCode + "Y", item.KPIName, item.Value, item.TargetValueW, i, currentYear, item.Area, item.UploadTimeY.ToSafetyString().Split(' ')[0].ToSafetyString());
                    }

                }
                if (item.PeriodValueY > 0 && item.StateY == true)
                {
                    // nếu năm hiện tại - năm max trong bảng DATA > 1 thì export năm kế tiếp đến năm hiện tại
                    if (currentYear - item.PeriodValueY >= 1)
                    {
                        for (int i = item.PeriodValueY + 1; i <= currentYear; i++)
                        {
                            Dt.Rows.Add(item.KPILevelCode + "Y", item.KPIName, item.Value, item.TargetValueW, i, currentYear, item.Area, item.UploadTimeY.ToSafetyString().Split(' ')[0].ToSafetyString());
                        }
                    }
                    else
                    {
                        for (int i = currentYear - 10; i <= currentYear; i++)
                        {
                            var value = new UploadDAO().GetValueData(item.KPILevelCode, "Y", i);
                            Dt.Rows.Add(item.KPILevelCode + "Y", item.KPIName, value, item.TargetValueW, i, currentYear, item.Area, item.UploadTimeY.ToSafetyString().Split(' ')[0].ToSafetyString());
                        }
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
        public ActionResult ExcelExport(int userid)
        {
            var model = new UploadDAO().DataExport(userid);
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();

            var now = DateTime.Now;
            var end = now.GetEndOfQuarter();
            var tt = end.Subtract(now).Days;
            //var targetValue = "";

            DataTable Dt = new DataTable();
            Dt.Columns.Add("KPILevel Code", typeof(string));
            Dt.Columns.Add("KPI Name", typeof(string));
            Dt.Columns.Add("Actual Value", typeof(string));
            Dt.Columns.Add("Target Value", typeof(object));
            Dt.Columns.Add("Period Value", typeof(string));
            Dt.Columns.Add("Year", typeof(int));
            Dt.Columns.Add("Area", typeof(string));
            Dt.Columns.Add("Update Time", typeof(object));
            Dt.Columns.Add("Start Date", typeof(string));
            Dt.Columns.Add("End Date", typeof(string));
            foreach (var item in model)
            {
                ///Logic export tuần
                //if (item.StateW == true)
                //{
                
                //    for (int i = 1; i <= currentWeek; i++)
                //    {
                //        var startDayOfWeek = CodeUtility.ToGetMondayOfWeek(currentYear, i).ToString("MM/dd/yyyy");
                //        var endDayOfWeek = CodeUtility.ToGetSaturdayOfWeek(currentYear, i).ToString("MM/dd/yyyy");
                //        var updateTimeW = item.UploadTimeW.ConvertNumberDayOfWeekToString() + ", Week " + i;

                //        var value = new UploadDAO().GetValueData(item.KPILevelCode, "W", i);
                //        Dt.Rows.Add(item.KPILevelCode + "W", item.KPIName, value, item.TargetValueW, i, currentYear, item.Area, updateTimeW, startDayOfWeek, endDayOfWeek);
                //    }
                //}

                /////Logic export tháng

                //if (item.StateM == true)
                //{
                 
                //    var updateTimeM = item.UploadTimeM.ToStringDateTime("MM/dd/yyyy");
                //    for (int i = 1; i <= currentMonth; i++)
                //    {
                //        var startDayOfMonth = CodeUtility.ToGetStartDateOfMonth(currentYear, i).ToString("MM/dd/yyyy");
                //        var endDayOfMonth = CodeUtility.ToGetEndDateOfMonth(currentYear, i).ToString("MM/dd/yyyy");
                //        var value = new UploadDAO().GetValueData(item.KPILevelCode, "M", i);
                //        Dt.Rows.Add(item.KPILevelCode + "M", item.KPIName, value, item.TargetValueW, i, currentYear, item.Area, updateTimeM, startDayOfMonth, endDayOfMonth);
                //    }

                //}
                /////Logic export quý
                //if (item.StateQ == true)
                //{
                  
                //    var updateTimeQ = item.UploadTimeQ.ToStringDateTime("MM/dd/yyyy");
                //    for (int i = 1; i <= currentQuarter; i++)
                //    {
                //        var seq = CodeUtility.ToGetStartAndEndDateOfQuarter(currentYear, i);
                //        var value = new UploadDAO().GetValueData(item.KPILevelCode, "Q", i);
                //        Dt.Rows.Add(item.KPILevelCode + "Q", item.KPIName, value, item.TargetValueW, i, currentYear, item.Area, updateTimeQ, seq.start.ToString("MM/dd/yyyy"), seq.end.ToString("MM/dd/yyyy"));
                //    }

                //}

                ///Logic export năm
                if (item.StateY == true)
                {
                    
                    var updateTimeY = item.UploadTimeY.ToStringDateTime("MM/dd/yyyy");
                    var sey = CodeUtility.ToGetStartAndEndDateOfYear(currentYear);
                    for (int i = currentYear - 9; i <= currentYear; i++)
                    {
                        var value = new UploadDAO().GetValueData(item.KPILevelCode, "Y", currentYear);
                        Dt.Rows.Add(item.KPILevelCode + "Y", item.KPIName, value, item.TargetValueW, i, currentYear, item.Area, updateTimeY, sey.start.ToString("MM/dd/yyyy"), sey.end.ToString("MM/dd/yyyy"));

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

                worksheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.DefaultColWidth = 20;
                worksheet.Column(2).AutoFit();

                Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
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
        public JsonResult KPIRelated(int levelid, int page, int pageSize)
        {
            return Json(new UploadDAO().KPIRelated(levelid, page, pageSize), JsonRequestBehavior.AllowGet);
        }
    }
}