using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using MyRazorApp.Models;

namespace MyRazorApp.Pages
{
    public class UsersModel : PageModel
    {
        private readonly string _connectionString = "Data Source=MyDatabase.db";
        // 绑定模型属性，用于前端表单接收数据
       [BindProperty]
        public User NewUser { get; set; } = new User();
        public string ErrorMessage { get; set; } = string.Empty;
        public List<User> UserList { get; set; } = new List<User>();

        public void OnGet()
        {
            LoadData();
        }
        public IActionResult Onpost()
        {
            
            if (string.IsNullOrEmpty(NewUser.Name))
            {
                ErrorMessage = "姓名不能为空！";
                LoadData();
                return Page();
            }
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                // 1. 【防重校验】检查数据库中是否存在 姓名+年龄 相同的数据
                string checkSql = "SELECT COUNT(*) FROM User WHERE Name = @Name AND Age = @Age;";
                using (var checkCmd = new SqliteCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Name", NewUser.Name);
                    checkCmd.Parameters.AddWithValue("@Age", NewUser.Age);
                    
                    long count = (long)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        // 发现重复数据，设置错误信息，不执行插入
                        ErrorMessage = $"添加失败：已存在名为 {NewUser.Name} 且年龄为 {NewUser.Age} 的用户！";
                        LoadData(); 
                        return Page();
                    }
                }

                // 2. 【执行插入】校验通过后才插入数据
                string insertSql = "INSERT INTO User (Name, Age) VALUES (@Name, @Age);";
                using (var cmd = new SqliteCommand(insertSql, conn))
                {
                    
                    cmd.Parameters.AddWithValue("@Name", NewUser.Name);
                    cmd.Parameters.AddWithValue("@Age", NewUser.Age);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage();
        }

    
        public void LoadData()
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, Name, Age FROM User;";
                using (var cmd = new SqliteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserList.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Age = reader.GetInt32(2)
                        });
                    }
                }
            }
        }
    }

    // public string Message{get;set;}="Hello Razor";
    // public int Age {get;set;}=20;
    // public void OnGet()
    // {
    //     Message="我从后台传过来的数据";
    //     Age=25;
    // }
}