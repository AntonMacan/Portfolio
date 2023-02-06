using Microsoft.AspNetCore.Mvc;
using RPPP_WebApp.ViewModels;

namespace RPPP_WebApp.Controllers;
/// <summary>
/// Web API servis za ispis loga 
/// </summary>
public class ViewLogController : Controller
{        
    /// <summary>
    /// Vraća osnovnu stranicu logova
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {      
        return View();
    }

    /// <summary>
    /// Vraća stranicu logova koji su zapisani na dan koji je jedank parametru dan
    /// </summary>
    /// <param name="dan">Datum</param>
    /// <returns></returns>
    public async Task<IActionResult> Show(DateTime dan)
    {
        ViewBag.Dan = dan;
        List<LogEntry> list = new List<LogEntry>();
        string format = dan.ToString("yyyy-MM-dd");
        string filename = Path.Combine(AppContext.BaseDirectory, $"logs/nlog-own-{format}.log");
        if (System.IO.File.Exists(filename))
        {
            String previousEntry = string.Empty;
            using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {          
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (line.StartsWith(format))
                        {
                            //počinje novi zapis, starog dodaj u listu
                            if (previousEntry != string.Empty)
                            {
                                LogEntry logEntry = LogEntry.FromString(previousEntry);
                                list.Add(logEntry);
                            }
                            previousEntry = line;
                        }
                        else
                        {
                            previousEntry += line;
                        }
                    }
                }
            }
            //dodaj zadnji

            if (previousEntry != string.Empty)
            {
                LogEntry logEntry = LogEntry.FromString(previousEntry);
                list.Add(logEntry);
            }
        }
        list.Sort((a, b) => -a.Time.CompareTo(b.Time));
        return View(list);
    }
}