using Crono;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crono;

internal class LinuxCron
{
    public static List<Container> ajustString(List<Container> containers)
    {
        for (int i = 0; i < containers.Count(); i++)
        {
            switch (containers[i].scheduling)
            {
                case "@yearly": containers[i].scheduling = "0 0 1 1 *"; break;
                case "@annually": containers[i].scheduling = "0 0 1 1 *"; break;
                case "@monthly": containers[i].scheduling = "0 0 1 * *"; break;
                case "@weekly": containers[i].scheduling = "0 0 * * 0"; break;
                case "@daily": containers[i].scheduling = "0 0 * * *"; break;
                case "@hourly": containers[i].scheduling = "0 * * * *"; break;
                default: break;
            }
        }

        return containers;
    }

    public static bool Bind(Container container) {
        if (container == null) return false;

        string[] scheduling = container.scheduling.Split(" ");

        int[] now = { DateTime.Now.Minute, DateTime.Now.Hour, DateTime.Now.Day, DateTime.Now.Month, (int)DateTime.Now.DayOfWeek };

        for (int i = 0; i < scheduling.Length; i++)
        {
            bool valid = validate(scheduling[i], now[i]);
            if (!valid) 
                return false;
        }

        return true;

    }

    private static bool validate(string v1, int v2)
    {
        //all
        if (v1 == "*")
        {
            return true;
        }

        //just one number
        try
        {
            if (Int16.Parse(v1) == v2)
            {
                return true;
            }
        }
        catch (Exception)
        {

        }
        

        //many elements separated by comma (,)
        var elementsComma = v1.Split(",");
        if (elementsComma.Length > 1) {
            foreach (var item in elementsComma)
            {
                try
                {
                    if (Int16.Parse(item) == v2)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        //many elements separated by hyphen (-)
        var elementsHyphen = v1.Split("-");
        if (elementsHyphen.Length > 1)
        {
            try
            {
                int first = Int16.Parse(elementsHyphen[0]);
                int last = Int16.Parse(elementsHyphen[elementsHyphen.Length - 1]);
                return Enumerable.Range(first, last).Contains(v2);
            }
            catch (Exception)
            {
                try
                {
                    if (v1.Contains("/"))
                    {
                        return validateForEachTime(v1, v2);
                    }
                }
                catch (Exception)
                {

                }
                
            }
        }

        try
        {
            return validateForEachTime(v1, v2);
        }
        catch (Exception)
        {

        }

        return false;
    }

    //in case "*/5" or "20-30/2"
    private static bool validateForEachTime(string v1, int v2) {

        var elements = v1.Split("/");
        if (elements.Length > 1) {
            int divisor = Int16.Parse(elements[1]);
            if (elements[0] == "*")
            {
                if (v2 % divisor == 0)
                {
                    return true;
                }
                return false;
            }
            else
            {
                var elementsHyphen = elements[0].Split("-");
                if (elementsHyphen.Length > 1) {
                    int first = Int16.Parse(elementsHyphen[0]);
                    int last = Int16.Parse(elementsHyphen[elementsHyphen.Length - 1]);
                    if (Enumerable.Range(first, last).Contains(v2))
                    {
                        if ((v2 - first) % divisor == 0) { 
                            return true;
                        }
                    }
                    
                }  
            }

        }

        return false;
    }
}
