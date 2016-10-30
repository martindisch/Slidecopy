using System.Net;

namespace test_upload
{
    class Program
    {
        // Contains no exception handling whatsoever. For development purposes only. Use at your own risk.
        static void Main(string[] args)
        {
            string url = System.IO.File.ReadAllLines("url.txt")[0];
            System.Console.Write("File to upload: ");
            string name = System.Console.ReadLine();
            System.Console.WriteLine("Uploading " + name + " ...");
            WebClient uploader = new WebClient();
            byte[] responseArray = uploader.UploadFile(url + ":8080/upload", name);
            System.Console.WriteLine("Done uploading. The response is:\n" + System.Text.Encoding.ASCII.GetString(responseArray));
        }
    }
}
