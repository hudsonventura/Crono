class Console
{
    public enum typeMessage{
        SUCESS,
        FAIL,
        NULL
    }

    private static char notice = '\u2757';

    public static void WriteLine(string msg, typeMessage success = typeMessage.NULL){
        Console.Write(msg, success);
        System.Console.WriteLine();
    }

    public static void Write(string msg, typeMessage success = typeMessage.NULL)
    {
        string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz");

        var beforeColor = System.Console.ForegroundColor;
        System.Console.Write($"{time} - ");

        if (success == typeMessage.SUCESS) { 
            System.Console.ForegroundColor = ConsoleColor.Green;
            //System.Console.Write(notice);
        }


        if (success == typeMessage.FAIL) {
            System.Console.ForegroundColor = ConsoleColor.Red;
            //System.Console.Write(notice);
        }
            

        System.Console.Write(" "+msg);
        System.Console.ForegroundColor = beforeColor;
    }
}