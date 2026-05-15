using Microsoft.Data.Sqlite;
using MyRazorApp.Models;


// 初始化数据库：自动建表 + 插入测试数据
void InitDatabase(string connectionString)
{
    using var conn = new SqliteConnection(connectionString);
    {   
        conn.Open();

        // 创建 User 表
        var createTable = @"
        CREATE TABLE IF NOT EXISTS ""User"" (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Age INTEGER NOT NULL,
            UNIQUE(Name, Age)
        );";

        using var cmd = new SqliteCommand(createTable, conn);
        cmd.ExecuteNonQuery();

        var checkSql="SELECT COUNT(*)FROM User;";
        using (var checkCmd=new SqliteCommand(checkSql,conn))
        {
            long count=(long)checkCmd.ExecuteScalar();
            if(count==0)
            {  // 插入测试数据  
                var insertData = @"
                INSERT OR IGNORE INTO ""User"" (Name,Age)
                VALUES ('张三',20),('李四',22),('王五',25),('小明',21),('小li',21);";
                using var insertCmd = new SqliteCommand(insertData, conn);
                insertCmd.ExecuteNonQuery();
                Console.WriteLine(">>> 数据库为空，已插入初始数据。");
            }else 
            {
                Console.WriteLine($">>> 数据库已有 {count} 条数据，跳过初始化。");
            }
        }
  
    }
}


var builder = WebApplication.CreateBuilder(args);


// 初始化数据库(zengjiade)
InitDatabase(builder.Configuration.GetConnectionString("SqliteConn")!);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
