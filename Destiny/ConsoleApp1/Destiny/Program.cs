
using System;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Linq;
using System.Media;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using OpenWeatherMap;
namespace SampleSynthesis
{
    
    class Program
    {
        public string TempC { get; internal set; }
        static List<string> jokes;
        static Stopwatch stopWatch = new Stopwatch();
        static TimeSpan ts = stopWatch.Elapsed;
        public static string name = string.Empty;
        public static bool stateOfSingleWordAnswer = false;
        static string commandUrl = "C:/Users/Aspire VX/source/repos/Destiny/ConsoleApp1/ConsoleApp1/Commands.txt";
        static string jokesUrl = "C:/Users/Aspire VX/source/repos/Destiny/ConsoleApp1/ConsoleApp1/Jokes.txt";
        static string destinyStatesUrl = "";
        static string searchKeywords = "C:/Users/Aspire VX/source/repos/Destiny/ConsoleApp1/ConsoleApp1/Searches.txt";
        static string storiesUrl = "";
        static string searchWord = "";
        static void Main(string[] args)
        {
            Destiny();
        }
        public static void Destiny()
        {
            PromptBuilder pBuilder = new PromptBuilder();
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            Choices sList = new Choices();
            sList.Add(File.ReadAllLines(commandUrl));
            GrammarBuilder gbuild = new GrammarBuilder();
            gbuild.Append(sList);
            gbuild.Culture = new System.Globalization.CultureInfo("en-US");
            Grammar gr = new Grammar(gbuild);

            recognizer.RequestRecognizerUpdate();
            recognizer.LoadGrammarAsync(gr);
            recognizer.SpeechRecognized += Recognizer_recognized;
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            while (true)
            {
                Console.ReadKey();
            }
        }
        private static void Recognizer_recognized(object sender, SpeechRecognizedEventArgs e)
        {
            var speak = new SpeechSynthesizer();
            string answer = "";
            string command = e.Result.Text.ToString();
            Console.WriteLine(command);
            speak.Speak(answer);
            switch (command)
            {
                case "Hello Destiny":
                case "Hello":
                case "Hi":
                    answer = "Hello, how are you!";
                    speak.SpeakAsync(answer);
                    break;
                case "I'm fine":
                case "I'm okay":
                case "Everything is great":
                    answer = "Im glad to hear that";
                    speak.SpeakAsync(answer);
                    break;
                case "How are you?":
                case "How about you?":
                case "How are you doing?":
                    answer = RandomPersonalStateAnswer();
                    speak.Speak(answer);
                    break;
                case "What time is it?":
                case "What is the time?":
                case "Tell me the time":
                case "Time now":
                case "Time":
                case "What is the time now":
                    answer = DateTime.Now.ToString("HH:mm");
                    Console.WriteLine(answer);
                    speak.Speak(answer);
                    break;
                case "What date is it?":
                case "What is the date?":
                case "Tell me the date":
                case "Date today":
                case "Date":
                case "What is the date today":
                    answer = DateTime.Now.ToString("dddd, dd MMMM yyyy");
                    Console.WriteLine(answer);
                    speak.Speak(answer);
                    break;
                case "Open Google":
                    answer = "Opening Google";
                    speak.Speak(answer);
                    GoToSite("www.google.com");
                    break;
                case "I am boris":
                    answer = "Hello bori";
                    Console.WriteLine(answer );
                    speak.Speak(answer);
                    break;
                case "Open Faceook":
                    answer = "Opening Facebook";
                    speak.Speak(answer);
                    GoToSite("www.facebook.com");
                    break;
                case "Open YouTube":
                    answer = "Opening Youtube";
                    speak.Speak(answer);
                    GoToSite("www.youtube.com");
                    break;
                case "Tell me a joke":
                    answer = RandomJoke();
                    Console.WriteLine(answer);
                    speak.Speak(answer);
                    break;
                case "Exit":
                case "Close":
                case "Thank you Destiny, Bye for now":
                    answer = ($"Goodbye, {name},Activate me again if you need me");
                    speak.Speak(answer);
                    Environment.Exit(0);
                    break;
                case "What is the Weather?":
                    answer = "Opening weather forecast";
                    speak.Speak(answer);
                    GoToSite("https://community-open-weather-map.p.rapidapi.com/weather?callback=test&id=2172797&units=%22metric%22+or+%22imperial%22&mode=xml%2C+html&q=London%2Cuk");

                    break;
                case "Search":
                case "Search in google":
                case "Search in the internet":
                case "New search":
                    answer = "What do you want me to search for";
                    Console.WriteLine(answer);
                    speak.Speak(answer);
                    Search();
                    break;
                case "Who am i":
                    answer = "You are spiderman";
                    Console.WriteLine(answer);
                    speak.Speak(answer);
                    break;
                


                //case "What is my name?":
                //    if (name == string.Empty)
                //    {
                //        answer = "You haven't told me yet, do you want me to remeber it?";
                //        speak.Speak(answer);
                //        SingleCommand();
                //        speak.Speak(answer);
                //    }
                //    else
                //    {
                //        answer = $"Your name is {name}";
                //        speak.Speak(answer);
                //    }
                //    break;
            }
        }
        public static void SingleCommand()
        {
            PromptBuilder pBuilder = new PromptBuilder();
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            Choices sList = new Choices();
            sList.Add(new string[] {
                "Yes",// positive
                "No",//negative
                "Start",//positive
                "Stop",//negative
                "Wrong command",
                "Back",
                "Return",
                "Restart",
                "No",
                "Nope",
                "Search Again",
                "New Search",
                "Wrong",
            });
            GrammarBuilder gbuild = new GrammarBuilder();
            gbuild.Append(sList);
            gbuild.Culture = new System.Globalization.CultureInfo("en-US");
            Grammar gr = new Grammar(gbuild);
            recognizer.RequestRecognizerUpdate();
            recognizer.LoadGrammarAsync(gr);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Recognizer_recognizedSingle);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            Console.ReadKey();
        }
        private static void Recognizer_recognizedSingle(object sender, SpeechRecognizedEventArgs e)
        {
            var speak = new SpeechSynthesizer();
            string answer = "";
            Console.WriteLine(e.Result.Text.ToString());
            string command = e.Result.Text.ToString();
            speak.Speak(answer);
            switch (command)
            {
                case "Yes":
                case "Shome me":
                case "Correct":
                    answer = "Searching information about " + searchWord;
                    Console.WriteLine(answer);
                    speak.Speak(answer);
                    GoToSite("https://www.google.com/search?q=" + searchWord);
                    Destiny();
                    break;
                case "No":
                case "Nope":
                case"Search Again":
                case "New Search":
                case "Wrong":
                    answer = "What do you want me to search for";
                    Console.WriteLine(answer);
                    speak.Speak(answer);
                    Search();
                    break;
                case "Wrong command":
                case "Back":
                case "Return":
                case "Restart":
                    ReturnToMainMenu();
                    break;
            }
        }
        public static void Search()
        {
            PromptBuilder pBuilder = new PromptBuilder();
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            Choices sList = new Choices();
            sList.Add(File.ReadAllLines(searchKeywords));
            GrammarBuilder gbuild = new GrammarBuilder();
            gbuild.Append(sList);
            gbuild.Culture = new System.Globalization.CultureInfo("en-US");
            Grammar gr = new Grammar(gbuild);
            recognizer.RequestRecognizerUpdate();
            recognizer.LoadGrammarAsync(gr);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SearchResult);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            Console.ReadKey();
        }
        private static void SearchResult(object sender, SpeechRecognizedEventArgs e)
        {
            var speak = new SpeechSynthesizer();
            string answer = "";
             searchWord = e.Result.Text.ToString();
            if(answer == "return"|| answer == "back" || answer == "go back" || answer == "main menu")
            {
                ReturnToMainMenu();
            }
            answer = "You want to see information about "+searchWord +" is that right?";
            Console.WriteLine(answer);
            speak.Speak(answer);
            SingleCommand();
        }
        public static void GoToSite(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
        public static string RandomJoke()
        {
            //var jokesToRead = File.ReadAllLines(jokesUrl);
            //var jk = new List<string>(jokesToRead);
            var jokes = System.IO.File.ReadLines(jokesUrl).ToList();
            string joke = "";
            Random random = new Random();
            int index =random.Next(0, 2);
            joke = jokes[index];
            return joke;
        }
        public static string RandomPersonalStateAnswer()
        {
            var jk = new List<string>();
            jk.Add("Im okay, my systems are functioning correctly");
            jk.Add("Im good, it is a beautiful day today!");
            jk.Add("Im fine thank you for asking") ;
            string joke = "";
            Random random = new Random();
            int index = random.Next(0, 2);
            joke = jk[index];
            return joke;
        }
        public static void ReturnToMainMenu()
        {
            var speak = new SpeechSynthesizer();
            string answer = "Returning to the main commands";
            Console.WriteLine(answer);
            speak.Speak(answer);
            Destiny();
        }
    }
    
}