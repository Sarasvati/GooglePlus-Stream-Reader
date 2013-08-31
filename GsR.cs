/********************************************************************************************************************************************
 * GsR - Google+ Stream Reader in C#                                                                                                      **
 * Author: Nicla Rossini http://niclarossini.com                                                                                           ** 
 *                                                                                                                                      *
 * ******************************************************************************************************************************************
 * This is open source software. Open source, means you can see the source code. It also means free software.                             ***
 * However, free software doesn't mean the same as free beer. Free software is closer to free speech, free thinker, and freedom in general.**
 * Free software doesn't mean that you don't have to acknowledge the author and abide to their license.                                    *
 * The author is releasing this for learning/educational purposes and/or in the hope it will be useful and with NO WARRANTY OF ANY KIND    **
 * (see License below).                                                                                                                     *
 ********************************************************************************************************************************************
 * **GsR IS PORTED FROM OPEN SOURCE PHP work by DEVIN SMITH http://devin.la and MICHAEL MAHEMOFF http://mahemoff.com***                     *   
 *                                                                                                                                         **
 *  PORTING, as a sort of "translation", is DERIVATIVE WORK and thus SUBJECT TO COPYRIGHT.                                                ***
 *  PORTED CODE normally bears THE SAME COPYRIGHT and LICENSE as the source.                                                               **
 *  However, I asked and obtained permission of the authors of the models I used to change the copyright and license of my porting work,   **   
 *  for a special reason (developers are strange and have ideals).                                                                         **
 *  This means that, as long as you KEEP INTACT ALL NOTICES, and DO NOT USE GsR for COMMERCIAL software, you can do what you want with it.  *
 *                                                                                                                                          *
 ********************************************************************************************************************************************
 *                                                                                                                                        ***
 *                                                                                                                                         **
 *                 Copyright (c) of the PHP source:  Devin Smyth (with New BSD License) and Michael Mahemoff (with MIT License)             *
 *                             Copyright (c) of the C# code below: Nicla Rossini 2013^  (with GNU/GPL 3 License)                           **   
 *                                                    ^ With thanks to Devin and Michael                                                  * *
 *                                                                                                                                          *
 *                                                                                                                                          *
 * This program is free software: you can redistribute it and/or modify                                                                     **
 *   it under the terms of the GNU General Public License as published by                                                                 ***
 *  the Free Software Foundation, either version 3 of the License, or                                                                      **
 *  (at your option) any later version.                                                                                                     *
 *                                                                                                                                         **
 *   This program is distributed in the hope that it will be useful,                                                                        *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of                                                                         **
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                                                                         ***
 *  GNU General Public License for more details.                                                                                          * *
 *                                                                                                                                         **
 *    You should have received a copy of the GNU General Public License                                                                     *
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.                                                                   *
 *                                                                                                                                          *
 * ******************************************************************************************************************************************/
//This is a console application, but you can use it for whatever else. 
//This part goes in the Program.cs file, btw (in case you want to use it to learn VisualStudio).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Dynamic;
using System.Web;
using System.Globalization;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;
//"using" just means we're using a namespace. Click here for info http://www.c-sharpcorner.com/UploadFile/ggaganesh/WorkingWithNamespaces11082005070923AM/WorkingWithNamespaces.aspx
// and yes I may have added namespaces I don't use ;)
namespace Googl
{
    class Program
    {

        static void Main(string[] args)
        {

// you want to use windows forms instead. This is just a random input method and also the easiest one, at least for me

            string pat = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string patpsa = pat + @"\whatever.txt";
            if (!File.Exists(patpsa))
            {
                Console.WriteLine("\a\n \t **I CAN'T FIND THE PAGES YOU WANT TO SEE!!**\n\n\n Don't panic just do this:\n\n 1) Put all google account urls into a text (.txt) file separated by a tab \n(no new line please!).\n\n 2) Then save the file on your Desktop with the name watever.txt\n\n d) Just run me again.\n\n\n\n P.S.: Finger crossing, knocks on wood, or even prayers are perfectly fine but\n won't help much \n");
                Console.ReadKey();
            }
            else
            {
                Console.Write("\n\t Working...\t"); //this just prints a message on the Desktop

               // Spinner: http://www.c-sharpcorner.com/uploadfile/cbragg/console-application-waitbusy-spin-animation/
                SpinAnimation.Start(50);
                string[] gpages = File.ReadAllLines(patpsa);
                foreach (string page in gpages)
                {
                   //simple regex
                    string[] pages = page.Split('\t');
                    foreach (string pag in pages)
                    {

                        string[] pieces = pag.Split('/');
                        var pageid = pieces[3]; //add something to check if this is an integral

                        UriBuilder NewUrl = new UriBuilder("https", "www.googleapis.com/plus/v1/people/" + pageid);
                        string key = " "; //put your Google app key here
                        string url = NewUrl + "activities/public?key=" + key;

                        System.Threading.Thread.Sleep(6000);

                        System.Net.WebClient wc = new System.Net.WebClient();
                        var request = wc.DownloadData(url);
                        var stuff = Encoding.UTF8.GetString(request);
                        string patjson = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string pathj= patjson + @"\googleJSON.txt";
                       
                        if (!File.Exists(pathj))
                        {
                            using (StreamWriter sw = File.CreateText(pathj))
                            {
                               //this just writes the whole thing
                     
                                sw.WriteLine(stuff);
                            }

                        }
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        itemList data = new JavaScriptSerializer().Deserialize<itemList>(stuff);
                        foreach (var obj in data.items)
                        {
                            //let's set some variables shall we
                            var crapsid = obj.id;
                            var kindacrap = obj.kind;
                            var published = obj.published;
                            var updated = obj.updated;
                            var crapsurl = obj.url;
                            var actorname = obj.actor.displayName;
                            var actorid = obj.actor.id;
                            var actorprofile = obj.actor.url;
                            var objecttype = obj.Object.objectType;
                            var objcontent = obj.Object.content;
                            var reshares = obj.Object.resharers.totalItems;
                            var plusoners = obj.Object.plusoners.totalItems;
                            var replies = obj.Object.replies.totalItems;
                            var fubar = obj.provider.title;
                            Console.WriteLine("OBJECT ID" + crapsid);
                            Console.ReadKey();
                            UriBuilder commentsurl = new UriBuilder("https", "www.googleapis.com/plus/v1/activities/" + crapsid+"/comments");
                            
                           
                            string urls2 = commentsurl+ "public?key=" + key;
                            Console.WriteLine("fetching this url for comments" + urls2);
                            System.Threading.Thread.Sleep(6000);

                            System.Net.WebClient client = new System.Net.WebClient();
                            var bohica = client.DownloadData(urls2);
                            var wtf = Encoding.UTF8.GetString(bohica);
                          

                           if (wt !=null){ //checking if it has something
                            
                            string patjson2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string pathjc = patjson2 + @"\googleJSONcomments.txt";

                            if (!File.Exists(pathjc))
                            { 
                                using (StreamWriter sw = File.CreateText(pathjc))
                                {

                                    //thought you would like to see a sample JSON in a file to study it so this will do it 
                                    sw.WriteLine(wt);
                                }


                            }else{ //close if 2 open else
                                using (StreamWriter sw = File.AppendText(pathjc))
                                {
                                   
                                    sw.Write("\n\n NEW OBJECT\n"+wt);
                                }
                           } //close else
                           } //close if 1

                            //now we do it for resharers
                            System.Threading.Thread.Sleep(6000);
                            UriBuilder resharesurl = new UriBuilder("https", "www.googleapis.com/plus/v1/activities/" + crapsid + "/people/resharers");

                            string urls3 = resharesurl + "public?key=" + key;
                            Console.WriteLine("fetching this url for reshares" + urls3);
                            System.Threading.Thread.Sleep(6000);

                            System.Net.WebClient whatever = new System.Net.WebClient();
                            var snafu = whatever.DownloadData(urls3);
                            var wfu = Encoding.UTF8.GetString(snafu);
                            if (wfu !=null){
                            Console.WriteLine("RESHARERS" + wfu);

                            string patjson3 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string pathjr = patjson3 + @"\googleJSONresharers.txt";

                            if (!File.Exists(pathjr))
                            {
                                using (StreamWriter sw = File.CreateText(pathjr))
                                {

                                    //thought you would like to see a sample JSON in a file to study it so this will do it 
                                    sw.WriteLine(wfu);
                                }

                            }else{
                                using (StreamWriter sw = File.AppendText(pathjr))
                                {
                                   
                                    sw.Write("\n\n NEW OBJECT\n"+wfu); // for debug
                                }
                           }
                            }
                            //& finally the plusoners (likers) for each post
                            System.Threading.Thread.Sleep(6000);
                            UriBuilder plusesurl = new UriBuilder("https", "www.googleapis.com/plus/v1/activities/" + crapsid + "/people/resharers");

                            string urls4 = resharesurl + "public?key=" + key;
                            Console.WriteLine("fetching this url for plusones " + urls4);
                            System.Threading.Thread.Sleep(6000);

                            System.Net.WebClient whatever02 = new System.Net.WebClient();
                            var bar = whatever02.DownloadData(urls4);
                            var fu = Encoding.UTF8.GetString(bar);
                            if (fu != null)
                            {
                                Console.WriteLine("PLUSONERS" + fu);

                                string patjson3 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                                string pathjr3 = patjson3 + @"\googleJSONplusoners.txt";

                                if (!File.Exists(pathjr3))
                                {
                                    using (StreamWriter sw = File.CreateText(pathjr3))
                                    {

                                        //thought you would like to see a sample JSON in a file to sudy it so this will do it 
                                        sw.WriteLine(fu);
                                    }

                                }else{
                                using (StreamWriter sw = File.AppendText(pathjr3))
                                {
                                   
                                    sw.Write("\n\n NEW OBJECT\n"+ fu); //debug
                                }
                           }
                            }

                            //let's decide a path and file name
                            string pat1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string path = pat1 + @"\google.txt";
                            // if the file doesn't exist, we create one and insert the data we just grabbed only once
                            if (!File.Exists(path))
                            {
                                using (StreamWriter sw = File.CreateText(path))
                                {

                                  
                                    /*  
                                            just an example of what you can do
                                            sw.Write(obj.title + "\t" + obj.kind + "\t" + obj.published + "\t" + obj.updated + "\t" + obj.id + "\t" + obj.url + "\t" + obj.actor.displayName + "\t" + obj.actor.id + "\t" + obj.actor.url + "\t" + obj.Object.objectType + "\t" + obj.Object.content + "\t" + obj.Object.resharers.totalItems + "\t" + obj.Object.plusoners.totalItems + "\t" + obj.Object.replies.totalItems + "\t" + obj.provider.title);
                                             */
                                }

                            }
                            else
                            {
                             /* if instead there's already a file with that name and path, we have to do something more complex. The following hopefully opens the file, 
                             * reads the lines contained in it one by one, and compares the id of each of the posts we've just grabbed form Google+. This is just an example and has no purpose */

                                if (File.ReadLines(path).Any(line => line.Contains(obj.id)))
                                {
                                    //if the post id string is already in the file, we're about to append a duplicate. We don't want duplicates so let's skip that item
                                    break;
                                }
                                else
                                {

                                    //if there are no duplicates, we add data to what we already had (without overwriting) with AppendText. This is just an example and has no specific purpose
                                    using (StreamWriter sw = File.AppendText(path))
                                    {
                                     
                                        /*  
                                            just an example of what you can do with the data
                                            sw.Write(obj.title + "\t" + obj.kind + "\t" + obj.published + "\t" + obj.updated + "\t" + obj.id + "\t" + obj.url + "\t" + obj.actor.displayName + "\t" + obj.actor.id + "\t" + obj.actor.url + "\t" + obj.Object.objectType + "\t" + obj.Object.content + "\t" + obj.Object.resharers.totalItems + "\t" + obj.Object.plusoners.totalItems + "\t" + obj.Object.replies.totalItems + "\t" + obj.provider.title);
                                         //or use the variables I set to print data into some user interface    
                                         */
                                    }
                                }


                            }

                        }
                    }
                }

               // Spinner: http://www.c-sharpcorner.com/uploadfile/cbragg/console-application-waitbusy-spin-animation/
                SpinAnimation.Stop();

              // now we open the file and print the data on the screen
                try
                {
                    string patt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string pattpsa = patt + @"\google.txt";

                    using (StreamReader sr = new StreamReader(pattpsa))
                    {
                        String line = sr.ReadToEnd();
                        Console.WriteLine(line);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Was trying to read the file we just created/updated but couldn't:");
                    Console.WriteLine(e.Message);
                }
                
              

                Console.ReadKey();

            }
        }

    }



}


// this is the class I wrote. At a certain point I got tired. There might be something else I didn't include

public class items
{
    public string kind { get; set; }
    public string title { get; set; }
    public string published { get; set; }
    public string updated { get; set; }
    public string id { get; set; }
    public string url { get; set; }
    public actor actor { get; set; }
    public string verb { get; set; }
    public Object Object { get; set; }
    public provider provider { get; set; }
}

public class actor
{
    public string id { get; set; }
    public string displayName { get; set; }
    public string url { get; set; }
}

public class replies
{
    public string totalItems { get; set; }
}

public class plusoners
{
    public string totalItems { get; set; }
}

public class resharers
{
    public string totalItems { get; set; }
}

public class attachments
{
    public string objectType { get; set; }
    public string displayName { get; set; }
    public string url { get; set; }
    public image image { get; set; }
    public fullImage fullImage { get; set; }
}

public class image
{
    public string url { get; set; }
    public string type { get; set; }
}

public class fullImage
{
    public string url { get; set; }
    public string type { get; set; }
}

public class Object
{
    public string objectType { get; set; }
    public string content { get; set; }
    public string url { get; set; }
    public replies replies { get; set; }
    public plusoners plusoners { get; set; }
    public resharers resharers { get; set; }
    public attachments att { get; set; }
    

}
public class provider
{
 public string title { get; set; }
}

public class itemList
{
    public string itemType { get; set; }
    public List<items> items { get; set; }

}

// traditional spinner to alert you that this thing is still working (and I put a sleep fucntion to throttle a bit).
//code retrieved here (this isn't mine but it's open source and I edited it a bit) http://www.c-sharpcorner.com/uploadfile/cbragg/console-application-waitbusy-spin-animation/

public static class SpinAnimation
{
    private static System.ComponentModel.BackgroundWorker spinner = initialiseBackgroundWorker();
    private static int spinnerPosition = 25;
    private static int spinWait = 25;
    private static bool isRunning;



    public static bool IsRunning { get { return isRunning; } }

    private static System.ComponentModel.BackgroundWorker initialiseBackgroundWorker()
    {



        System.ComponentModel.BackgroundWorker obj = new System.ComponentModel.BackgroundWorker();
        obj.WorkerSupportsCancellation = true;
        obj.DoWork += delegate
        {

            spinnerPosition = Console.CursorLeft;
            while (!obj.CancellationPending)
            {

             

                char[] spinChars = new char[] { '.', '-', '+', '^', '°', '*' };;


                foreach (char spinChar in spinChars)
                {

                    Console.CursorLeft = spinnerPosition;

                    Console.Write(spinChar);
                    System.Threading.Thread.Sleep(spinWait);

                }

            }

        };

        return obj;

    }


    public static void Start(int spinWait)
    {

        //Set the running flag

        isRunning = true;

        //process spinwait value

        SpinAnimation.spinWait = spinWait;

        //start the animation unless already started

        if (!spinner.IsBusy)

            spinner.RunWorkerAsync();

        else throw new InvalidOperationException("Cannot start spinner whilst spinner is already running");

    }

    public static void Start() { Start(25); }
    public static void Stop()
    {

        //Stop the animation

        spinner.CancelAsync();

        //wait for cancellation to complete

        while (spinner.IsBusy) System.Threading.Thread.Sleep(100);

        //reset the cursor position

        Console.CursorLeft = spinnerPosition;

        //set the running flag

        isRunning = false;

    }

}



