﻿using KPI.Model.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
  public  class CenterAdminDAO
    {
        KPIDbContext _dbContext = null;

        public CenterAdminDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public int Add(EF.Center entity)
        {
            var code = entity.Code.ToUpper();
            List<EF.KPILevel> kpiLevelList = new List<EF.KPILevel>();

            if (_dbContext.Centers.FirstOrDefault(x => x.Code == code) != null)
            {
                return 2;
            }
            if (_dbContext.KPILevels.FirstOrDefault(x=>x.KPICode==entity.Code) != null)
            {
                return 2;
            }
            //get all kpi
            var kpis = new KPIAdminDAO().GetAll();
            foreach (var kpi in kpis)
            {
                var kpilevel = new EF.KPILevel();
                kpilevel.TableID =code;
                kpilevel.KPICode = kpi.Code;
                kpilevel.Name = kpi.Name;
                kpilevel.Checked = false;
                kpiLevelList.Add(kpilevel);
            }
            try
            {
                entity.Code = entity.Code.ToUpper();
                _dbContext.Centers.Add(entity);
                _dbContext.KPILevels.AddRange(kpiLevelList);
                _dbContext.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public bool Update(EF.Center entity)
        {
            var code = entity.Code.ToUpper();
            var item = _dbContext.Centers.FirstOrDefault(x => x.ID == entity.ID);
            var kpiLevels = _dbContext.KPILevels.Where(f => f.TableID == item.Code).ToList();
            kpiLevels.ForEach(a =>
            {
                a.TableID = code;

            });


            try
            {
                var SOP = _dbContext.Centers.Find(entity.ID);
                SOP.Name = entity.Name;
                SOP.Code = code;
                SOP.LevelID = entity.LevelID;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }
        public bool Delete(int ID)
        {
            var findUser = _dbContext.Centers.FirstOrDefault(x => x.ID == ID);
            var kpiLevel = _dbContext.KPILevels.Where(x => x.TableID == findUser.Code).ToList();
            try
            {
                _dbContext.KPILevels.RemoveRange(kpiLevel);
                _dbContext.Centers.Remove(findUser);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Exception error = ex;
                return false;
            }

        }
        public IEnumerable<EF.Center> GetAll()
        {
            return _dbContext.Centers.ToList();
        }
        public EF.Center GetbyID(int ID)
        {
            return _dbContext.Centers.FirstOrDefault(x=>x.ID==ID);
        }
        //public MuitipleDataViewModel MultipleData()
        //{
        //    var cCategory = _dbContext.ComponantCategorys.ToList();
        //    var cDetail = _dbContext.ComponantDetails.ToList();
        //    var operation = _dbContext.Operations.ToList();
        //    var modelSOP = _dbContext.SOPModels.ToList();
        //    var machine = _dbContext.Machines.ToList();
        //    var treatment = _dbContext.Treatments.ToList();
        //    var sop = _dbContext.SOPs.ToList();
        //    MuitipleDataViewModel listMulti = new MuitipleDataViewModel();
        //    listMulti.ComponentCategories = cCategory;
        //    listMulti.ComponentDetails = cDetail;
        //    listMulti.Operations = operation;
        //    listMulti.SOPModels = modelSOP;
        //    listMulti.Machines = machine;
        //    listMulti.Treatments = treatment;
        //    listMulti.SOPs = sop;
        //    return listMulti;

        //}

        //public bool SearchName(string name)
        //{

        //    if (!String.IsNullOrEmpty(name))
        //    {
        //        var SearchName = _dbContext.Operations.Where(s => s.Name == name);
        //        if (SearchName == null)
        //            return false;
        //    }
        //    return true;
        //}
        public PagedData<EF.Center> GetPaggedData(int pageNumber = 1, int pageSize = 5)
        {
            List<EF.Center> listData = _dbContext.Centers.ToList();
            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            return pagedData;
        }
    }
}
