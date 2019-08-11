using KPI.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class Group : Inheritance
    {
        public int? LevelNumber { get; set; }
        public int? Parent { get; set; }
    }
    public class Division : Inheritance
    {
        public int? LevelNumber { get; set; }
        public int? Parent { get; set; }
    }
    public class Factory : Inheritance
    {
        public int? LevelNumber { get; set; }
        public int? Parent { get; set; }
    }
    public class Center : Inheritance
    {
        public int? LevelNumber { get; set; }
        public int? Parent { get; set; }
    }
    public class Department : Inheritance
    {
        public int? LevelNumber { get; set; }
        public int? Parent { get; set; }
    }
    public class Building : Inheritance
    {
        public int? LevelNumber { get; set; }
        public int? Parent { get; set; }
    }
    public class Line : Inheritance
    {
        public int? LevelNumber { get; set; }
        public int? Parent { get; set; }
    }
    public class Team : Inheritance
    {
        public int? LevelNumber { get; set; }
        public int? Parent { get; set; }
    }
    public class LevelVM
    {
        public List<Group> Groups { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Factory> Factorys { get; set; }
        public List<Center> Centers { get; set; }
        public List<Department> Departments { get; set; }
        public List<Building> Buildings { get; set; }
        public List<Line> Lines { get; set; }
        public List<Team> Teams { get; set; }
    }
}
