using System.Diagnostics;

namespace Crono
{
    internal class ExternalApp
    {

        public static string run(string app, string args)
        {
            Process process = new Process();
            process.StartInfo.FileName = app;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            //System.Console.WriteLine(output);

            if (output == "" && err == "")
            {
                throw new Exception(process.BasePriority.ToString());
            }
            if (err != "")
            {
                throw new Exception(err);
            }


            if (output == null)
            {
                throw new Exception(err);
            }
            return output;
        }
    }
}
