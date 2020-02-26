using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuxyboxIdentity.Data;
namespace LuxyboxIdentity.Helper
{
    public class BusinessHelper
    {
        public static void AddCategory(Category category)
        {
            using (var db = new Entities())
            {
                db.Categories.Add(category);
                db.SaveChanges();
            }
        }
        public static List<Category> GetCategories()
        {
            using (var db = new Entities())
            {
                var cateogries = db.Categories.ToList();
                return cateogries;
            }
        }
    }
}