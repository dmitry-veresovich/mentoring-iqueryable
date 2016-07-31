using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample03.E3SClient.Entities;
using Sample03.E3SClient;
using System.Configuration;
using System.Linq;

namespace Sample03
{
	[TestClass]
	public class E3SProviderTests
	{
		[TestMethod]
		public void WithoutProvider()
		{
			var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"] , ConfigurationManager.AppSettings["password"]);
			var res = client.SearchFTS<EmployeeEntity>("workstation:(EPRUIZHW0249)", 0, 1);

			foreach (var emp in res)
			{
                PrintEmployee(emp);
            }
		}

		[TestMethod]
		public void WithoutProviderNonGeneric()
		{
			var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
			var res = client.SearchFTS(typeof(EmployeeEntity), "workstation:(EPRUIZHW0249)", 0, 10);

			foreach (var emp in res.OfType<EmployeeEntity>())
			{
                PrintEmployee(emp);
            }
		}


		[TestMethod]
		public void WithProviderOrder1()
		{
			var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

			foreach (var emp in employees.Where(e => e.workstation == "EPRUIZHW0249"))
			{
                PrintEmployee(emp);
            }
        }

        [TestMethod]
        public void WithProviderOrder2()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            foreach (var emp in employees.Where(e => "EPRUIZHW0249" == e.workstation))
            {
                PrintEmployee(emp);
            }
        }

        [TestMethod]
        public void WithProviderStartsWith()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            foreach (var emp in employees.Where(e => e.workstation.StartsWith("EPRUIZHW024")))
            {
                PrintEmployee(emp);
            }
        }

        [TestMethod]
        public void WithProviderEndsWith()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            foreach (var emp in employees.Where(e => e.workstation.EndsWith("0249")))
            {
                PrintEmployee(emp);
            }
        }

        [TestMethod]
        public void WithProviderContains()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            foreach (var emp in employees.Where(e => e.workstation.Contains("UIZHW02")))
            {
                PrintEmployee(emp);
            }
        }

        [TestMethod]
        public void WithoutProviderAnd()
        {
            var client = new E3SQueryClient(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var res = client.SearchFTS<EmployeeEntity>("workstation:(*4881*) workstation:(EPBYMINW*)", 0, 1);

            foreach (var emp in res)
            {
                PrintEmployee(emp);
            }
        }

        [TestMethod]
        public void WithProviderAndOrder1()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            foreach (var emp in employees.Where(e => e.workstation.Contains("4881") && e.workstation.StartsWith("EPBYMINW")))
            {
                PrintEmployee(emp);
            }
        }

        [TestMethod]
        public void WithProviderAndOrder2()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);

            foreach (var emp in employees.Where(e => e.workstation.StartsWith("EPBYMINW") && e.workstation.Contains("4881")))
            {
                PrintEmployee(emp);
            }
        }

	    private static void PrintEmployee(EmployeeEntity emp)
	    {
	        Console.WriteLine($"{emp.nativename} {emp.startworkdate} {emp.workstation}");
	    }
	}
}
