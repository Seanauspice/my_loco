using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using MyRazorApp.Models;

namespace MyRazorApp.Pages
{
    public class SearchBySpecsModel:PageModel
    {
        private readonly string _connectionString = "Data Source=MyDatabase.db";
        // 支持 GET 请求绑定，方便用户分享搜索结果 URL
        [BindProperty(SupportsGet = true)]
        public double? InputSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public double? InputWeight { get; set; }

        public List<string> MatchNames { get; set; } = new List<string>();
        public void OnGet()
        {
            if (InputSize == null || InputWeight == null) return;

            using (var conn=new SqliteConnection(_connectionString))
            {
                conn.Open();

                
            }
            
        }

    }

}