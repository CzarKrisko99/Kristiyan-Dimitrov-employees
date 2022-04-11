using Sirma_Solutions_Assigment.BussinessLogic.Contracts;
using Sirma_Solutions_Assigment.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sirma_Solutions_Assigment.BussinessLogic.Services
{
    public class CSVService : ICSVService
    {
        private string[] dateTimeFormats = new string[8] { "dd-mmm-yyyy", "yyyy-dd-mmm", "yyyy-mmm-dd", "mmm-yyyy-dd", "MM/dd/yyyy", "dd/MM/yyyy", "yyyy/MM/dd", "yyyy/dd/MM" };

        public CSVService()
        {

        }

        public async Task<SharedWork> GetHardestWorkingPair(Stream stream)
        {
            List<Employee> employees = await this.ParseCsvToDataObjects(stream);

            List<SharedWork> sharedProjects = this.CalculateSharedProjects(employees);

            return sharedProjects.OrderByDescending(p => p.TotalWorkedDays).First();                                   
        }

        private async Task<List<Employee>> ParseCsvToDataObjects(Stream stream)
        {
            int lines = 0;
            List<string[]> stringLines = new List<string[]>();
            List<Employee> employees = new List<Employee>();

            using (StreamReader str = new StreamReader(stream))
            {
                while (!str.EndOfStream)
                {
                    string? line = await str.ReadLineAsync();

                    var values = line?.Split(',');

                    if(values?.Length == 4)
                    {
                        stringLines.Add(values);
                        lines++;
                    }
                }
            }

            for (int i = 0; i < lines; i++)
            {
                int employeeId = Convert.ToInt32(stringLines[i][0]);
                int projectId = Convert.ToInt32(stringLines[i][1]);
                DateTime startFrom = this.ParseDate(stringLines[i][2]);
                DateTime endTo = (stringLines[i][3].ToUpper() != "NULL") ? this.ParseDate(stringLines[i][3]) : DateTime.Now;

                if (!employees.Any(e => e.EmployeeID == employeeId))
                {
                    var employee = new Employee();
                    employee.EmployeeID = employeeId;
                    employee.Projects.Add(new Project()
                    {
                        ProjectId = projectId,
                        StartedWork = startFrom,
                        EndedWork = endTo
                    });
                    employees.Add(employee);
                }
                else
                {
                    employees.First(e => e.EmployeeID == employeeId).Projects.Add(new Project()
                    {
                        ProjectId = projectId,
                        StartedWork = startFrom,
                        EndedWork = endTo
                    });
                }
            }
            return employees;
        }

        private DateTime ParseDate(string date)
        {     
            DateTime result = DateTime.Now;
            bool result2 = false;
            var counter = 0;
            while (!result2)
            {
                result2 = DateTime.TryParseExact(date, this.dateTimeFormats[counter], CultureInfo.InvariantCulture, DateTimeStyles.None, out result);

                counter++;
                if (counter == this.dateTimeFormats.Length)
                {
                    throw new Exception("Cannot parse date value!");                    
                }
            }
            return result;
        }

        private List<SharedWork> CalculateSharedProjects(List<Employee> employees)
        {
            int employeeCount =  employees.Count;
            List<SharedWork> sharedWorkList = new List<SharedWork>();

            for (int i = 0; i < employeeCount; i++)
            {
                var firstEmployee = employees[i];
                //iterate other employees to compare them with the previous one
                for (int u = i + 1; u < employeeCount; u++)
                {
                    var secondEmployee = employees[u];
                    //iterate all projects from the emplyoee and check if they have common projects
                    foreach (var secondProject in secondEmployee.Projects)
                    {
                        var firstProject = firstEmployee.Projects.FirstOrDefault(p => p.ProjectId == secondProject.ProjectId);
                        if(firstProject != null)
                        {
                            int projectId = firstProject.ProjectId = secondProject.ProjectId;
                            int worktime = 0;
                            
                            //check whether one's work time intercepts with other's worktime => they worked on the project together
                            if((firstProject.StartedWork >= secondProject.StartedWork && firstProject.StartedWork <= secondProject.EndedWork)
                                || (firstProject.EndedWork >= secondProject.StartedWork && firstProject.EndedWork <= secondProject.EndedWork)
                                || (secondProject.StartedWork >= firstProject.StartedWork && secondProject.StartedWork <= firstProject.EndedWork)
                                || (secondProject.EndedWork >= firstProject.StartedWork && secondProject.EndedWork <= secondProject.EndedWork))
                            {
                                DateTime sharingStart = firstProject.StartedWork >= secondProject.StartedWork ? firstProject.StartedWork : secondProject.StartedWork;
                                DateTime sharingEnd = firstProject.EndedWork <= secondProject.EndedWork ? firstProject.EndedWork : secondProject.EndedWork;

                                worktime = this.CalculateSharedWorkTime(sharingStart, sharingEnd);
                            }
                            
                            if(worktime > 0)
                            {
                                var sharedWork = sharedWorkList.FirstOrDefault(r => (r.FirstEmployeeId == firstEmployee.EmployeeID && r.SecondEmployeeId == secondEmployee.EmployeeID));

                                if(sharedWork != null)
                                {
                                    if(!sharedWork.ProjectIdsAndWorkTime.Keys.Contains(projectId))
                                        sharedWork.ProjectIdsAndWorkTime.Add(projectId, worktime);
                                    else
                                        sharedWork.ProjectIdsAndWorkTime[projectId] += worktime;

                                    continue;
                                }

                                sharedWorkList.Add(new SharedWork()
                                {
                                    FirstEmployeeId = firstEmployee.EmployeeID,
                                    SecondEmployeeId = secondEmployee.EmployeeID,
                                    ProjectIdsAndWorkTime = new Dictionary<int, int>() { { projectId, worktime } }
                                });
                            }
                        }
                    }
                }
            }

            return sharedWorkList;            
        }

        private int CalculateSharedWorkTime(DateTime startDate, DateTime endDate)
        {
            var time = endDate - startDate;

            return time.Days;
        }
    }
}
