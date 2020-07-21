using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Repro.Lib.Data;
using Repro.Lib.Models;
using System;
using System.Linq;

namespace Repro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddScoped(_ => new ReproDbContext(Configuration.GetConnectionString("DefaultConnection")));
            services.AddOData();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Filter().Expand().Count().OrderBy().SkipToken().MaxTop(null);
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
                routeBuilder.EnableDependencyInjection();
            });

            // Seed database
            SeedDatabase(app);
        }

        private static void SeedDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ReproDbContext dbContext = serviceScope.ServiceProvider.GetService<ReproDbContext>();

                if (!dbContext.Companies.Any())
                {
                    #region Companies

                    dbContext.Companies.Add(new Company { Id = "CG", Name = "Contracting & General" });

                    #endregion Companies

                    #region Employees

                    dbContext.Employees.Add(new Employee { Id = 1, Name = "Nancy Davolio" });
                    dbContext.Employees.Add(new Employee { Id = 2, Name = "Andrew Fuller", ManagerId = 1 });
                    dbContext.Employees.Add(new Employee { Id = 3, Name = "Janet Leverling", ManagerId = 1 });
                    dbContext.Employees.Add(new Employee { Id = 4, Name = "Andrew Cencini", ManagerId = 2 });
                    dbContext.Employees.Add(new Employee { Id = 5, Name = "Steven Thorpe", ManagerId = 3 });
                    dbContext.Employees.Add(new Employee { Id = 6, Name = "Mariya Sergienko", ManagerId = 2 });
                    dbContext.Employees.Add(new Employee { Id = 7, Name = "Laura Giussani", ManagerId = 3 });
                    dbContext.Employees.Add(new Employee { Id = 8, Name = "Robert Zare", ManagerId = 2 });
                    dbContext.Employees.Add(new Employee { Id = 9, Name = "Michael Neipper", ManagerId = 7 });

                    #endregion Employees

                    #region Projects

                    dbContext.Projects.AddRange(
                        new[]
                        {
                            new Project { Id = 1, Name = "Electrical Installation", Start = new DateTime(2020, 8, 6), End = new DateTime(2020, 11, 13), ManagerId = 2 },
                            new Project { Id = 2, Name = "Pipeline Installation", Start = new DateTime(2020, 7, 17), End = new DateTime(2020, 8, 11), ManagerId = 3 }
                        });

                    #endregion Projects

                    #region Milestones

                    dbContext.Milestones.AddRange(
                        new[]
                        {
                            new Milestone { Id = 1, Name = "Mobilization", ProjectId = 1, Start = new DateTime(2020, 8, 8), End = new DateTime(2020, 8, 23) },
                            new Milestone { Id = 2, Name = "Construction", ProjectId = 1, Start = new DateTime(2020, 8, 27), End = new DateTime(2020, 10, 17) },
                            new Milestone { Id = 3, Name = "Site Restoration", ProjectId = 1, Start = new DateTime(2020, 9, 18), End = new DateTime(2020, 10, 25) },
                            new Milestone { Id = 4, Name = "Project Closeout", ProjectId = 1, Start = new DateTime(2020, 10, 29), End = new DateTime(2020, 11, 13) },
                            new Milestone { Id = 5, Name = "Excavation", ProjectId = 2, Start = new DateTime(2020, 7, 19), End = new DateTime(2020, 7, 25) },
                            new Milestone { Id = 6, Name = "Installation", ProjectId = 2, Start = new DateTime(2020, 7, 27), End = new DateTime(2020, 8, 3) },
                            new Milestone { Id = 7, Name = "Refill & Paving", ProjectId = 2, Start = new DateTime(2020, 8, 4), End = new DateTime(2020, 8, 11) },
                            new Milestone { Id = 8, Name = "Project Closeout", ProjectId = 2, Start = new DateTime(2020, 8, 11), End = new DateTime(2020, 8, 11) }
                        });

                    #endregion Milestones

                    #region Tasks

                    dbContext.Tasks.AddRange(
                        new[]
                        {
                            new Task { Id = 1, Description = "Notice to Proceed", ProjectId = 1, Start = new DateTime(2020, 8, 6), End = new DateTime(2020, 8, 6) },
                            new Task { Id = 2, Description = "Project Start", ProjectId = 1, Start = new DateTime(2020, 8, 7), End = new DateTime(2020, 8, 7) },
                            new Task { Id = 3, Description = "Mobilize", ProjectId = 1, MilestoneId = 1, Start = new DateTime(2020, 8, 8), End = new DateTime(2020, 8, 23) },
                            new Task { Id = 4, Description = "Grade Site", ProjectId = 1, MilestoneId = 2, SupervisorId = 4, Start = new DateTime(2020, 8, 27), End = new DateTime(2020, 9, 6) },
                            new Task { Id = 5, Description = "Set Foundation", ProjectId = 1, MilestoneId = 2, SupervisorId = 4, Start = new DateTime(2020, 8, 27), End = new DateTime(2020, 9, 10) },
                            new Task { Id = 6, Description = "Install Conduit", ProjectId = 1, MilestoneId = 2, SupervisorId = 4, Start = new DateTime(2020, 9, 10), End = new DateTime(2020, 9, 12) },
                            new Task { Id = 7, Description = "Dig Cable Trench", ProjectId = 1, MilestoneId = 2, SupervisorId = 4, Start = new DateTime(2020, 9, 11), End = new DateTime(2020, 9, 17) },
                            new Task { Id = 8, Description = "Erect Steel Structures", ProjectId = 1, MilestoneId = 2, SupervisorId = 6, Start = new DateTime(2020, 9, 13), End = new DateTime(2020, 9, 26) },
                            new Task { Id = 9, Description = "Install Equipment", ProjectId = 1, MilestoneId = 2, SupervisorId = 6, Start = new DateTime(2020, 9, 18), End = new DateTime(2020, 9, 26) },
                            new Task { Id = 10, Description = "Install Grounding", ProjectId = 1, MilestoneId = 2, SupervisorId = 6, Start = new DateTime(2020, 9, 27), End = new DateTime(2020, 10, 1) },
                            new Task { Id = 11, Description = "Install Bus and Jumpers", ProjectId = 1, MilestoneId = 2, SupervisorId = 6, Start = new DateTime(2020, 9, 27), End = new DateTime(2020, 10, 10) },
                            new Task { Id = 12, Description = "Lay Control Cable", ProjectId = 1, MilestoneId = 2, SupervisorId = 6, Start = new DateTime(2020, 9, 27), End = new DateTime(2020, 10, 17) },
                            new Task { Id = 13, Description = "Install Fence", ProjectId = 1, MilestoneId = 2, SupervisorId = 6, Start = new DateTime(2020, 9, 10), End = new DateTime(2020, 9, 19) },
                            new Task { Id = 14, Description = "Remove Equipment", ProjectId = 1, MilestoneId = 3, SupervisorId = 8, Start = new DateTime(2020, 10, 18), End = new DateTime(2020, 10, 25) },
                            new Task { Id = 15, Description = "Lay Stoning", ProjectId = 1, MilestoneId = 3, SupervisorId = 8, Start = new DateTime(2020, 9, 18), End = new DateTime(2020, 9, 19) },
                            new Task { Id = 16, Description = "Lay Roadway", ProjectId = 1, MilestoneId = 3, SupervisorId = 8, Start = new DateTime(2020, 9, 18), End = new DateTime(2020, 9, 24) },
                            new Task { Id = 17, Description = "Project SignOff", ProjectId = 1, MilestoneId = 4, Start = new DateTime(2020, 10, 29), End = new DateTime(2020, 11, 13) },
                            new Task { Id = 18, Description = "Start Project", ProjectId = 2, Start = new DateTime(2020, 7, 17), End = new DateTime(2020, 7, 17) },
                            new Task { Id = 19, Description = "Site Survey", ProjectId = 2, Start = new DateTime(2020, 7, 17), End = new DateTime(2020, 7, 17) },
                            new Task { Id = 20, Description = "Mobilize on Site", ProjectId = 2, Start = new DateTime(2020, 7, 18), End = new DateTime(2020, 7, 18) },
                            new Task { Id = 21, Description = "Backhoe Excavate", ProjectId = 2, MilestoneId = 5, SupervisorId = 5, Start = new DateTime(2020, 7, 19), End = new DateTime(2020, 7, 24) },
                            new Task { Id = 22, Description = "Install Shoring", ProjectId = 2, MilestoneId = 5, SupervisorId = 5, Start = new DateTime(2020, 7, 24), End = new DateTime(2020, 7, 25) },
                            new Task { Id = 23, Description = "Common Laborer Excavate", ProjectId = 2, MilestoneId = 5, SupervisorId = 5, Start = new DateTime(2020, 7, 25), End = new DateTime(2020, 7, 25) },
                            new Task { Id = 24, Description = "Install Piping", ProjectId = 2, MilestoneId = 6, SupervisorId = 7, Start = new DateTime(2020, 7, 27), End = new DateTime(2020, 8, 1) },
                            new Task { Id = 25, Description = "Install Couplings", ProjectId = 2, MilestoneId = 6, SupervisorId = 7, Start = new DateTime(2020, 8, 2), End = new DateTime(2020, 8, 2) },
                            new Task { Id = 26, Description = "QA Inspection", ProjectId = 2, MilestoneId = 6, SupervisorId = 7, Start = new DateTime(2020, 8, 3), End = new DateTime(2020, 8, 3) },
                            new Task { Id = 27, Description = "Remove Shoring", ProjectId = 2, MilestoneId = 7, SupervisorId = 9, Start = new DateTime(2020, 8, 4), End = new DateTime(2020, 8, 4) },
                            new Task { Id = 28, Description = "Backfill & Compact", ProjectId = 2, MilestoneId = 7, SupervisorId = 9, Start = new DateTime(2020, 8, 7), End = new DateTime(2020, 8, 10) },
                            new Task { Id = 29, Description = "Asphalt Surface Roadway", ProjectId = 2, MilestoneId = 7, SupervisorId = 9, Start = new DateTime(2020, 8, 11), End = new DateTime(2020, 8, 11) },
                            new Task { Id = 30, Description = "Project SignOff", ProjectId = 2, MilestoneId = 8, Start = new DateTime(2020, 8, 11), End = new DateTime(2020, 8, 11) }
                        });

                    #endregion Tasks

                    dbContext.SaveChanges();
                }
            }
        }

        private static IEdmModel GetEdmModel()
        {
            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();

            modelBuilder.EntityType<Task>();
            modelBuilder.EntityType<Milestone>();
            modelBuilder.EntitySet<Employee>("Employees");

            SingletonConfiguration<Company> companyConfiguration = modelBuilder.Singleton<Company>("Company");
            companyConfiguration.EntityType.Function("Projects").ReturnsCollectionFromEntitySet<Project>("Projects");
            
            EntitySetConfiguration<Project> projectsConfiguration = modelBuilder.EntitySet<Project>("Projects");
            projectsConfiguration.EntityType.Collection.Function("KeyProjects").ReturnsCollectionFromEntitySet<Project>("Projects");

            return modelBuilder.GetEdmModel();
        }
    }
}
