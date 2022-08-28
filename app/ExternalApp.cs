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
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            //Console.WriteLine(output);
            string err = process.StandardError.ReadToEnd();
            //Console.WriteLine(err);
            process.WaitForExit();

            if (output == null)
            {
                return err;
            }
            return output;
        }
    }
}
