﻿using System;
using DapperDoodle;
using Microsoft.AspNetCore.Mvc;

namespace TestRunner.Controllers
{
    [Route("")]
    [Controller]
    public class HomeController : Controller
    {
        public ICommandExecutor CommandExecutor { get; }
        public IQueryExecutor QueryExecutor { get; }

        public HomeController(
            ICommandExecutor commandExecutor, 
            IQueryExecutor queryExecutor)
        {
            CommandExecutor = commandExecutor;
            QueryExecutor = queryExecutor;
        }
        
        [Route("")]
        public ActionResult Index()
        {
            CommandExecutor.Execute(new PersistSomething());
            return Content("Saved Successfully");
        }
    }

    public class PersistSomething : Command
    {
        public override void Execute()
        {
            BuildInsert<Person>();
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}