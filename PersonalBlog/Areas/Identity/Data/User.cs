using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PersonalBlog.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the PersonalBlogUser class
    public class PersonalBlogUser : IdentityUser
    {
        public PersonalBlogUser()
        {
            CustomProperty = "MyCustomProperty";
        }

        // Add additional properties if necessary
        public string CustomProperty
        {
            get;
            set;
        }
    }
}
