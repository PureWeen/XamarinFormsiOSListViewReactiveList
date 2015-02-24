using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SomeLibrary
{
    public class Class1
    {

        public Task SomeMethod()
        {
            HttpClient client = new HttpClient();

            return client.GetStringAsync("http://www.google.com");
        }
    }
}
