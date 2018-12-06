using System;

namespace RestClientJenkins
{
    class Program
    {
        static void Main(string[] args)
        {
            JenkinsClient client = new JenkinsClient("http://localhost:8080", "simon", "11e9ad4166271e3c4d4306a82adecf6c94");
            //JenkinsClient client = new JenkinsClient("http://localhost:8080", "simon", "simon");
            Console.WriteLine(client.GetBuildLog("test2", "24"));
            Console.ReadKey(true);
        }
    }
}
