using System;
using System.IO;

namespace WordleSolver
{
    class Program
    {
        List<String> alphabet = new List<String>();
        string wordInput;
        string wordInputNoLocation;
        string lettersNotInWordInput;
        String fiveLetterPath;
        List<String> fiveLetterWord = new List<String>();
        List<String> sortedFiveLetterWord = new List<String>();
        List<String> reRunSortedFiveLetterWord = new List<String>();
        List<String> noSpacesWordInput = new List<String>();

        public static void Main(string[] args)
        {
            bool end = false;
            bool rerun = false;
            Program wordle = new Program();
            while (end == false)
            {
                if (rerun == true)
                {
                    wordle.ReRunSort(wordle);
                }
                else
                {
                    wordle.GetAlphabet();
                    // wordle.GetWords();
                    // wordle.FixWordList();
                    wordle.FindPossibleWords(wordle);
                }
                
                Console.WriteLine("End program?");
                string endProgram = Console.ReadLine();
                if (endProgram == "yes" || endProgram == "")
                {
                    end = true;
                }
                rerun = true;
            }
            
        }
        public void GetWords()
        {
            foreach (String alpha in alphabet)
            {
                //String path = @"C:\Users\dslone\Downloads\Word-lists-in-csv\Word lists in csv\" + alpha + "word.csv";
                String path = @"C:\Users\dslone\Downloads\12dicts-6.0.2\Special\neol2016.txt";
                String fiveLetterPath = @"C:\Users\dslone\Downloads\Word-lists-in-csv\Words\5Letter" + alpha + "word.csv";
                List<String> lines = new List<String>();
                List<String> fiveLetterWord = new List<String>();
                String alphaLowerCase = alpha.ToLower();


                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        String line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.ToLower();
                            if (line.ToString().Length == 5 && line.StartsWith(alphaLowerCase))
                                fiveLetterWord.Add(line);
                        }
                    }
                    foreach (String liness in fiveLetterWord)
                        Console.WriteLine("Five " + liness);
                    File.AppendAllLinesAsync(fiveLetterPath, fiveLetterWord);
                }
            }
        }
        public void FixWordList()
        {
            foreach (String alpha in alphabet)
            {
                String path = @"C:\Users\dslone\Downloads\Word-lists-in-csv\Words\5Letter" + alpha + "word.csv";
                String fiveLetterPath = @"C:\Users\dslone\Downloads\Word-lists-in-csv\Words\5LetterSorted" + alpha + "word.csv";
                List<String> lines = new List<String>();
                List<String> fiveLetterWord = new List<String>();
                
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        String line;
                        while ((line = reader.ReadLine()) != null)
                            if ((!fiveLetterWord.Contains(line)) && (!line.StartsWith("#")))
                                fiveLetterWord.Add(line);
                    }
                    foreach (String liness in fiveLetterWord)
                        Console.WriteLine("Five " + liness);
                    File.WriteAllLinesAsync(fiveLetterPath, fiveLetterWord);
                }
            }
        }
        public void FindPossibleWords(Program wordleSolver)
        {
            string knowLocationInput;

            Console.WriteLine("Do you know any letters location?");
            knowLocationInput = Console.ReadLine();
            knowLocationInput = knowLocationInput.ToLower();
            
            if (knowLocationInput == "yes")
            {
                Console.WriteLine("Please input letters you know.");
                wordInput = Console.ReadLine();
               
                if (wordInput != "")
                {
                    bool correctInput = false;
                    while (correctInput == false)
                    {
                        noSpacesWordInput.AddRange(wordInput.Split("?"));
                        if (!wordInput.StartsWith("?"))
                        {
                            wordleSolver.KnowStartingLetter();
                            correctInput = true;
                        }
                        else
                        {
                            wordleSolver.StartingLocationNotKnown();
                            correctInput = true;
                        }
                        
                        Console.WriteLine("Please input letter you do not know the location of.");
                        wordInputNoLocation = Console.ReadLine();
                        wordleSolver.NoKnownLocations();

                        Console.WriteLine("Please input letters that are not in the word.");
                        lettersNotInWordInput = Console.ReadLine();
                        wordleSolver.LettersNotInWord();
                    }
                }
                else
                    Console.WriteLine("Please type a 5 letter word.");
            }
            else
            {
                wordleSolver.NoKnownLocationsToStart();

                sortedFiveLetterWord.AddRange(fiveLetterWord);
                Console.WriteLine("Please input letters that are not in the word.");
                lettersNotInWordInput = Console.ReadLine();
                wordleSolver.LettersNotInWord();
            }
        }
        public void NoKnownLocationsToStart()
        {
            bool correctInput = false;
            while (correctInput == false)
            {
                Console.WriteLine("Please input letters you know.");
                wordInput = Console.ReadLine();
                if (wordInput != null)
                {
                    foreach (String alpha in alphabet)
                    {
                        fiveLetterPath = @"C:\Users\dslone\Downloads\Word-lists-in-csv\Words\5LetterSorted" + alpha + "word.csv";
                        if (File.Exists(fiveLetterPath))
                        {
                            using (StreamReader reader = new StreamReader(fiveLetterPath))
                            {
                                String line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    int wordCursor = 0;
                                    int lettersLocatated = 0;
                                    List<String> repeatLetters = new List<String>();
                                    while (wordCursor < line.Length)
                                    {
                                        int letterCounter = 0;
                                        while (letterCounter < wordInput.Length)
                                        {
                                            if (line[wordCursor].ToString() == wordInput[letterCounter].ToString() && !repeatLetters.Contains(line[wordCursor].ToString()))
                                            {
                                                repeatLetters.Add(line[wordCursor].ToString());
                                                lettersLocatated++;
                                            }
                                            if (!fiveLetterWord.Contains(line) && lettersLocatated >= wordInput.Length)
                                                fiveLetterWord.Add(line);
                                            letterCounter++;
                                        }
                                        wordCursor++;
                                    }
                                }
                            }
                        }
                    }
                    foreach (String lines in fiveLetterWord)
                        Console.WriteLine(lines);
                    correctInput = true;
                }
                else
                    Console.WriteLine("Please type a letter.");
            }
        }
        public void KnowStartingLetter()
        {
            string alpha = wordInput[0].ToString();
            alpha = alpha.ToUpper();
            fiveLetterPath = @"C:\Users\dslone\Downloads\Word-lists-in-csv\Words\5LetterSorted" + alpha + "word.csv";

            if (File.Exists(fiveLetterPath))
            {
                using (StreamReader reader = new StreamReader(fiveLetterPath))
                {
                    String line;
                    String subLine;
                    while ((line = reader.ReadLine()) != null)
                    {
                        subLine = line.Substring(1, line.Length-1);
                        foreach (String liness in noSpacesWordInput)
                        {
                            if (subLine.Contains(liness) && wordInput.Length < 6)
                            {
                                if (!fiveLetterWord.Contains(line))
                                    fiveLetterWord.Add(line);
                            }
                            else if (fiveLetterWord.Contains(line))
                                fiveLetterWord.Remove(line);
                        }
                    }
                }
                foreach (String lines in fiveLetterWord)
                {
                    bool endLoop = false;
                    int letterCounter = 1;
                    while (letterCounter < wordInput.Length)
                    {
                        if (wordInput[letterCounter].ToString() != "?")
                        {
                            if (lines[letterCounter].ToString() == wordInput[letterCounter].ToString())
                            {
                                if (!sortedFiveLetterWord.Contains(lines))
                                    sortedFiveLetterWord.Add(lines);
                            }
                            else
                            {
                                endLoop = true;
                                goto AfterLoop;
                            }
                        }
                        letterCounter++;
                    }
                    AfterLoop:
                    {
                        if (endLoop)
                            sortedFiveLetterWord.Remove(lines);
                    }
                }
                //foreach (String lines in sortedFiveLetterWord)
                //    Console.WriteLine(lines);
            }
        }
        public void StartingLocationNotKnown()
        {
            foreach (String alpha in alphabet)
            {
                fiveLetterPath = @"C:\Users\dslone\Downloads\Word-lists-in-csv\Words\5LetterSorted" + alpha + "word.csv";

                if (File.Exists(fiveLetterPath))
                {
                    using (StreamReader reader = new StreamReader(fiveLetterPath))
                    {
                        String line;
                        String subLine;
                        while ((line = reader.ReadLine()) != null)
                        {
                            subLine = line.Substring(1, line.Length-1);
                            foreach (String liness in noSpacesWordInput)
                            {
                                if (subLine.Contains(liness) && wordInput.Length < 6)
                                {
                                    if (!fiveLetterWord.Contains(line))
                                        fiveLetterWord.Add(line);
                                }
                                else if (fiveLetterWord.Contains(line))
                                    fiveLetterWord.Remove(line);
                            }
                        }
                    }
                    foreach (String lines in fiveLetterWord)
                    {
                        bool endLoop = false;
                        int letterCounter = 1;
                        while (letterCounter < wordInput.Length)
                        {
                            if (wordInput[letterCounter].ToString() != "?")
                            {
                                if (lines[letterCounter].ToString() == wordInput[letterCounter].ToString())
                                {
                                    if (!sortedFiveLetterWord.Contains(lines))
                                        sortedFiveLetterWord.Add(lines);
                                }
                                else
                                {
                                    endLoop = true;
                                    goto AfterLoop;
                                }
                            }
                            letterCounter++;
                        }
                        AfterLoop:
                        {
                            if (endLoop)
                                sortedFiveLetterWord.Remove(lines);
                        }
                    }
                    //foreach (String lines in sortedFiveLetterWord)
                    //    Console.WriteLine(lines);
                }
            }
        }
        public void NoKnownLocations()
        {
            if (wordInputNoLocation != "")
            {
                if (sortedFiveLetterWord != null)
                {
                    fiveLetterWord.Clear();
                    fiveLetterWord.AddRange(sortedFiveLetterWord);
                    foreach (String lines in fiveLetterWord)
                    {
                        int wordCursor = 0;
                        int lettersLocatated = 0;
                        while (wordCursor < lines.Length)
                        {
                            int letterCounter = 0;
                            while (letterCounter < wordInputNoLocation.Length)
                            {
                                if (lines[wordCursor].ToString() == wordInputNoLocation[letterCounter].ToString())
                                {
                                    lettersLocatated++;
                                }   
                                letterCounter++;
                            }
                            wordCursor++;
                        }
                        if (lettersLocatated < wordInputNoLocation.Length)
                            sortedFiveLetterWord.Remove(lines);
                    }
                    foreach (String lines in sortedFiveLetterWord)
                        Console.WriteLine(lines);
                }
            }
        }
        public void LettersNotInWord()
        {
            if (lettersNotInWordInput != null)
            {
                if (sortedFiveLetterWord.Count != 0)
                {
                    fiveLetterWord.Clear();
                    fiveLetterWord.AddRange(sortedFiveLetterWord);
                    foreach (String lines in fiveLetterWord)
                    {
                        bool endLoop = false;
                        int wordCursor = 0;
                        while (wordCursor < lines.Length)
                        {
                            int letterCounter = 0;
                            while (letterCounter < lettersNotInWordInput.Length)
                            {
                                if (lines[wordCursor].ToString() == lettersNotInWordInput[letterCounter].ToString())
                                {
                                    sortedFiveLetterWord.Remove(lines);
                                    goto AfterLoop;
                                }
                                letterCounter++;
                            }
                            wordCursor++;
                        }
                        AfterLoop:
                        { }
                    }
                    foreach (String lines in sortedFiveLetterWord)
                        Console.WriteLine(lines);
                }
            }
        }
        public void ReRunSort(Program wordleSolver)
        {
            reRunSortedFiveLetterWord.Clear();
            reRunSortedFiveLetterWord.AddRange(sortedFiveLetterWord);
            wordleSolver.LocationsLettersAreNotIn();

            String newLetterLocation;
            Console.WriteLine("Please input new letter locations found");
            newLetterLocation = Console.ReadLine();

            if (newLetterLocation != "")
            {
                foreach (String lines in reRunSortedFiveLetterWord.ToList())
                {
                    bool endLoop = false;
                    int letterCounter = 0;
                    while (letterCounter < newLetterLocation.Length)
                    {
                        if (newLetterLocation[letterCounter].ToString() != "?")
                        {
                            if (lines[letterCounter].ToString() != newLetterLocation[letterCounter].ToString())
                            {
                                endLoop = true;
                                goto AfterLoop;
                            }
                        }
                        letterCounter++;
                    }
                    AfterLoop:
                    {
                        if (endLoop)
                            reRunSortedFiveLetterWord.Remove(lines);
                    }
                }
                foreach (String lines in reRunSortedFiveLetterWord.ToList())
                    Console.WriteLine(lines);
            }

            newLetterLocation = "";
            Console.WriteLine("Please input new letters found");
            newLetterLocation = Console.ReadLine();

            if (reRunSortedFiveLetterWord != null && newLetterLocation != "")
            {
                fiveLetterWord.Clear();
                fiveLetterWord.AddRange(reRunSortedFiveLetterWord);
                foreach (String lines in fiveLetterWord.ToList())
                {
                    bool endLoop = false;
                    int wordCursor = 0;
                    while (wordCursor < lines.Length)
                    {
                        int letterCounter = 0;
                        while (letterCounter < newLetterLocation.Length)
                        {
                            if (lines[wordCursor].ToString() == newLetterLocation[letterCounter].ToString())
                                endLoop = true;
                            letterCounter++;
                        }
                        wordCursor++;
                    }
                    if (!endLoop)
                        reRunSortedFiveLetterWord.Remove(lines);
                }
                foreach (String lines in reRunSortedFiveLetterWord.ToList())
                    Console.WriteLine(lines);
            }

            if (reRunSortedFiveLetterWord.Count != 0)
            {
                sortedFiveLetterWord.Clear();
                sortedFiveLetterWord.AddRange(reRunSortedFiveLetterWord);
                Console.WriteLine("Please input letters that are not in the word.");
                lettersNotInWordInput = Console.ReadLine();
                wordleSolver.LettersNotInWord();
            }
            

        }
        public void LocationsLettersAreNotIn()
        {
            String newLetterLocation;

            Console.WriteLine("Please input letter locations they are not in");
            newLetterLocation = Console.ReadLine();

            if (newLetterLocation != "")
            {
                foreach (String lines in reRunSortedFiveLetterWord.ToList())
                {
                    bool endLoop = false;
                    int letterCounter = 0;
                    while (letterCounter < newLetterLocation.Length)
                    {
                        if (newLetterLocation[letterCounter].ToString() != "?")
                        {
                            if (lines[letterCounter].ToString() == newLetterLocation[letterCounter].ToString())
                            {
                                endLoop = true;
                                goto AfterLoop;
                            }
                        }
                        letterCounter++;
                    }
                AfterLoop:
                    {
                        if (endLoop)
                            reRunSortedFiveLetterWord.Remove(lines);
                    }
                }
                foreach (String lines in reRunSortedFiveLetterWord.ToList())
                    Console.WriteLine(lines);
            }
        }
        public void GetAlphabet()
        {
            alphabet.Add("A");
            alphabet.Add("B");
            alphabet.Add("C");
            alphabet.Add("D");
            alphabet.Add("E");
            alphabet.Add("F");
            alphabet.Add("G");
            alphabet.Add("H");
            alphabet.Add("I");
            alphabet.Add("J");
            alphabet.Add("K");
            alphabet.Add("L");
            alphabet.Add("M");
            alphabet.Add("N");
            alphabet.Add("O");
            alphabet.Add("P");
            alphabet.Add("Q");
            alphabet.Add("R");
            alphabet.Add("S");
            alphabet.Add("T");
            alphabet.Add("U");
            alphabet.Add("V");
            alphabet.Add("W");
            alphabet.Add("X");
            alphabet.Add("Y");
            alphabet.Add("Z");
        }
    }
}






