using CafeT.Objects;
using CafeT.Text;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CafeT.SmartObjects
{
    public class PhraseProcessor
    {
        public bool CheckWord(string word)
        {
            if (word.StartWithUpper()) return true;
            return false;
        }
    }

    public class TextProcessor
    {
        public string Input { set; get; } = string.Empty;
        public string Output { set; get; } = string.Empty;
        public string CurrentText { set; get; } = string.Empty;
        public WordObject CurrentWord { set; get; } = new WordObject();

        public List<WordObject> CleanWordObjects { set; get; } = new List<WordObject>();
        public List<WordObject> FullWordObjects { set; get; } = new List<WordObject>();
        public List<string> Words { set; get; } = new List<string>();
        public List<string> EnglihsWords { set; get; } = new List<string>();
        public List<string> VietnameseWords { set; get; } = new List<string>();
        public List<string> Numbers { set; get; } = new List<string>();
        public List<string> UrlLinks { set; get; } = new List<string>();
        public List<string> ImageLinks { set; get; } = new List<string>();
        public List<string> Emails { set; get; } = new List<string>();
        public List<string> Sentences { set; get; } = new List<string>();
        public List<string> QuestionSentences { set; get; } = new List<string>();
        public List<string> Phrases { set; get; } = new List<string>();
        public List<string> MathExprs { set; get; } = new List<string>();
        public List<string> Commands { set; get; } = new List<string>();

        public int CountOfWords { set; get; } = 0;
        public int CountOfFullWords { set; get; } = 0;
        public int TimeToRead { set; get; } = 0;
        public int CountOfQuestions { set; get; } = 0;

        public TextProcessor(string text)
        {
            Input = text;
            Input = HttpUtility.HtmlDecode(Input);
            CurrentText = Input;
            CurrentText = CurrentText.StripHtml().ToStandard();

            var _models = CurrentText.ToWords();
            _models = _models.Where(t => t.IsWord()).ToArray();
            for (int i = 0; i < _models.Length; i++)
            {
                var _word = new Word(_models[i]);

                if(_word.CanRead())
                {
                    WordObject _object = new WordObject(_models[i], i);
                    _object.Indexs = CurrentText.IndexAll(_word.Value);
                    FullWordObjects.Add(_object);
                    _object.ToClean();
                    if(_object.IsClean())
                    {
                        CleanWordObjects.Add(_object);
                    }
                }
            }
            CountOfWords = CleanWordObjects.Count;
            CountOfFullWords = FullWordObjects.Count;
            TimeToRead = CountOfWords / 200; //Normal read speed
            LoadFull();
        }
        
        public void LoadFull()
        {
            Words = FullWordObjects.Select(t => t.Value).ToList();
            EnglihsWords = CleanWordObjects.Where(t => t.Lang == WordLang.English)
                .Select(t=>t.Value.ToLower())
                .Distinct()
                .ToList();
            VietnameseWords = CleanWordObjects.Where(t => t.Lang == WordLang.Vietnamese)
                .Select(t => t.Value.ToLower())
                .Distinct()
                .ToList();
            Numbers = CleanWordObjects.Where(t => t.IsNumber())
                .Select(t => t.Value.ToLower())
                .Distinct()
                .ToList();
            UrlLinks = CleanWordObjects.Where(t => t.Value.IsUrl())
                .Select(t => t.Value.ToLower())
                .Distinct()
                .ToList();
            Emails = CleanWordObjects.Where(t => t.Value.IsEmail())
                .Select(t => t.Value.ToLower())
                .Distinct()
                .ToList();
            Sentences = CurrentText.GetSentences()
                .Distinct()
                .ToList();
            QuestionSentences = GetQuestionSentences();
            CountOfQuestions = QuestionSentences.Count();
            try
            {
                Phrases = GetPhrases().ToList();
                MathExprs = GetMathExprs().ToList();
                Commands = GetCommands();
                MakeOutput();
            }
            catch
            {
                return;
            }
        }

        public List<string> GetCommands()
        {
            List<string> commands = new List<string>();
            var _first = FullWordObjects.First();
            var _last = FullWordObjects.Last();
            var _run = _first;

            while(HasNext(_run))
            {
              if (_run.Contains("[>") && _run.Contains(";"))
                {
                    commands.Add(_run.Value.ExtendRightTo(text:CurrentText, to:";"));
                }
                _run = Next(_run);
                if(_run == null)
                {
                    System.Console.Write(_run.PrintAllProperties());
                }
            }
            return commands;
        }

        public bool HasNext(WordObject model)
        {
            if(model != null)
            {
                if (model.Index < CountOfWords) return true;
            }
            
            return false;
        }
        public List<string> GetQuestionSentences()
        {
            return Sentences.Where(t => t.EndsWith("?"))
                .Distinct()
                .ToList();
        }
        
        public string[] GetVnKeywords()
        {
            var _keywords = CleanWordObjects
                .Where(t => t.IsVnKeyword())
                .Select(t => t.Value)
                .Distinct()
                .ToArray();
            return _keywords;
        }

        public IEnumerable<string> GetPhrases()
        {
            List<string> phrases = new List<string>();
            if(CleanWordObjects != null && CleanWordObjects.Count > 0)
            {
                var runObject = CleanWordObjects[0];
                while (HasNext(runObject))
                {
                    int _tmp = runObject.Index;
                    if (runObject.Value.StartWithUpper())
                    {
                        string _phrase = string.Empty;
                        var nextObject = Next(runObject);
                        var _i = nextObject;
                        while (_i != null && _i.Value.StartWithUpper())
                        {
                            _phrase += " " + _i.Value;
                            _i = Next(_i);
                        }
                        _phrase = _phrase.AddBefore(runObject.Value);
                        phrases.Add(_phrase);
                    }
                    runObject = Next(runObject);
                }
                return phrases.Where(t => !t.IsNullOrEmptyOrWhiteSpace())
                    .Where(t => t.GetCountWords() > 1)
                    .Where(t => IsPhrase(t))
                    .Distinct();
            }
            else
            {
                return null;
            }
        }
        public bool IsPhrase(string pharse)
        {
            if (CurrentText.IndexOf(pharse) > 0) return true;
            return false;
        }
        public IEnumerable<string> GetMathExprs()
        {
            List<string> mathExprs = new List<string>();
            var _questions = GetQuestionSentences();

            if(_questions != null && _questions.Count > 0)
            {
                foreach(string q in _questions)
                {
                    string _expr = q.Substring(0, q.Length - 1);
                    Expression e = new Expression(_expr);
                    try
                    {
                        if(e.calculate().ToString().IsNumeric())
                            mathExprs.Add(q + " |=> " + e.calculate().ToString());
                    }
                    catch
                    {
                        //Nothing to do
                    }
                }
            }
            return mathExprs;
        }
        public string[] SentencesOf(WordObject word)
        {
            return Sentences.Where(t => t.Contains(word.Value))
                .ToArray();
        }
        public IEnumerable<WordObject> GetWordsBy(WordLang lang)
        {
            return CleanWordObjects.Where(t => t.Lang == lang);
        }

        public WordObject GetWord(int index)
        {
            if(index < CountOfWords)
            {
                WordObject model = CleanWordObjects[index];
                model.Indexs = CurrentText.IndexAll(model.Value);
                return model;
            }
            else
            {
                return null;
            }
        }
        public WordObject Next()
        {
            try
            {
                return GetWord(CurrentWord.Index + 1);
            }
            catch
            {
                return null;
            }
        }
        public WordObject Next(WordObject current)
        {
            int _index = current.Index;
            if(_index < CountOfWords)
            {
                return GetWord(_index + 1);
            }
            else
            {
                return null;
            }
        }
        public WordObject Previous(WordObject current)
        {
            try
            {
                return GetWord(current.Index - 1);
            }
            catch
            {
                return null;
            }
        }

        public override string ToString()
        {
            return this.PrintAllProperties();
        }
        
        public void MakeOutput()
        {
            //Output = CurrentText;
            foreach(var word in FullWordObjects)
            {
                if(word.IsClean())
                {
                    Output += word.Value + " ";
                }
                else
                {
                    TextCommand textCommand = new TextCommand(word.Value);
                    if(textCommand.IsCommand())
                    {
                        textCommand.Excute();
                        Output += textCommand.ToString() + " ";
                    }
                }
            }
        }
    }
}
