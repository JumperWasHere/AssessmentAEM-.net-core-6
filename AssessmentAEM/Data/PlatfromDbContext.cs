using AssessmentAEM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace AssessmentAEM.Data
{
    public class PlatfromDbContext:DbContext
    {
        private DateTime dateTime;

        public PlatfromDbContext(DbContextOptions<PlatfromDbContext> options) : base(options)
        {

        }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<Well> Well { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

        //     modelBuilder.Entity<Platform>().HasData(
        //        new Platform()
        //        {
        //            id = 1,
        //            PlatformName = "ANGSI",
        //            PlatformId = 11,
        //            UniqueName = "Well11",
        //            Latitude = "37.06257",
        //            Longitude = "18.406885",
        //            CreatedAt = DateTime.Parse("2018-08-04T02:16:42"),
        //            UpdatedAt = DateTime.Parse("2018-08-04T02:16:42")

        //        }
        //    );
        //}
    }
}
