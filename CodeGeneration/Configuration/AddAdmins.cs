//using LeadManagement.Data;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LeadManagement.Configuration
//{
//    public class AddAdmins : IEntityTypeConfiguration<ApplicationUser>
//    {
//        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
//        {
//            builder.HasData(
//                new ApplicationUser
//                {
//                    Email = "nseif@icon-creations.com",
//                    UserName = "nseif@icon-creations.com",
//                    EmailConfirmed = true,
//                    PasswordHash = ""
//                },
//                new ApplicationUser
//                {
//                    Id = 2,
//                    Name = "Comfort Suites",
//                    Address = "George Town",
//                    CountryId = 3,
//                    Rating = 4.3
//                },
//                new ApplicationUser
//                {
//                    Id = 3,
//                    Name = "Grand Palldium",
//                    Address = "Nassua",
//                    CountryId = 2,
//                    Rating = 4
//                }
//            );
//        }
//    }
//}
