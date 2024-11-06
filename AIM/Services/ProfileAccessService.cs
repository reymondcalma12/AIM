using AIM.Data;
using AIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIM.Services
{
    public class ProfileAccessService
    {
        private readonly AimDbContext dbContext;

        public ProfileAccessService(AimDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateProfileAccesses()
        {
            // Retrieve profiles and modules from the database
            List<Profile> profiles = dbContext.tbl_aim_profiles.ToList();
            List<Module> modules = dbContext.tbl_aim_modules.ToList();

            // Retrieve existing profile access records from the database
            List<ProfileAccess> existingProfileAccesses = dbContext.tbl_aim_profileaccess.ToList();

            // Generate access records for each combination of profiles and modules
            List<ProfileAccess> profileAccesses = new List<ProfileAccess>();
            foreach (Profile profile in profiles)
            {
                foreach (Module module in modules)
                {
                    // Check if a profile access record already exists for the combination of profile and module
                    bool accessExists = existingProfileAccesses.Any(pa => pa.ProfileId == profile.ProfileId && pa.ModuleId == module.ModuleId);
                    if (accessExists)
                    {
                        // Skip creating the profile access if it already exists
                        continue;
                    }

                    ProfileAccess access = new ProfileAccess
                    {
                        ProfileId = profile.ProfileId,
                        ModuleId = module.ModuleId,
                        OpenAccess = "N", // Set the default access as "N" (no access)
                        UserCreated = "dtan",
                        UserDtCreated = DateTime.Now
                    };
                    profileAccesses.Add(access);
                }
            }

            // Save the new access records to the database
            dbContext.tbl_aim_profileaccess.AddRange(profileAccesses);
            dbContext.SaveChanges();
        }

        public void CreateProfileAccessForNewProfile(Profile newProfile)
        {
            List<Module> modules = dbContext.tbl_aim_modules.ToList();
            List<ProfileAccess> profileAccesses = new List<ProfileAccess>();

            foreach (Module module in modules)
            {
                ProfileAccess access = new ProfileAccess
                {
                    ProfileId = newProfile.ProfileId,
                    ModuleId = module.ModuleId,
                    OpenAccess = "N", // Set the default access as "N" (no access)
                    UserCreated = "A004",
                    UserDtCreated = DateTime.Now
                };
                profileAccesses.Add(access);
            }

            dbContext.tbl_aim_profileaccess.AddRange(profileAccesses);
            dbContext.SaveChanges();
        }

        public void CreateProfileAccessForNewModule(Module newModule)
        {
            List<Profile> profiles = dbContext.tbl_aim_profiles.ToList();
            List<ProfileAccess> profileAccesses = new List<ProfileAccess>();

            foreach (Profile profile in profiles)
            {
                ProfileAccess access = new ProfileAccess
                {
                    ProfileId = profile.ProfileId,
                    ModuleId = newModule.ModuleId,
                    OpenAccess = "N", // Set the default access as "N" (no access)
                    UserCreated = "A004",
                    UserDtCreated = DateTime.Now
                };
                profileAccesses.Add(access);
            }

            dbContext.tbl_aim_profileaccess.AddRange(profileAccesses);
            dbContext.SaveChanges();
        }
    }

}
