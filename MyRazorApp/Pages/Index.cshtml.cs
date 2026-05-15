using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyRazorApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    //日志工具
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }//系统自动把日志工具传给你

    public int CurrentBg{get;set;}=1;
    public string Slogan{get;set;}="HEllo";

    public void OnGet()
    {
        CurrentBg=1;
        Slogan="你好！";
    }

    public IActionResult OnPostChangeBg(int currentBg)
    {
        Console.WriteLine("收到的背景号：" + currentBg);// 调试用：打印当前值

        if (currentBg == 1) CurrentBg = 2;
        else if (currentBg == 2) CurrentBg = 3;
        else if (currentBg == 3) CurrentBg = 1;

        Console.WriteLine("切换后背景号：" + CurrentBg); // 调试用：打印切换后的值

        Slogan="你好！";
        return Page();

    }


}
