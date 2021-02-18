using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfApp12.Models
{
    public class Game
    {
        
        public event EventHandler CurrentMessageChanged;
        public event EventHandler<int> ImageChanged;
        public event EventHandler<Char[]> WordStatusChanged;
        string word;
        int errorCount = 0;
        Char[] chars= new Char [0];
        private string currentMessage;
        public string CurrentMessage
        {
            get => currentMessage;
            set
            {
                currentMessage = value;
                CurrentMessageChanged?.Invoke(this, null);
            }
        }

        public bool Status { get; private set; }
        public int CountLetter { get; set; }

        public void StartGame()
        {
            Status = true;
            string path = "word_rus.txt";
            var words = File.ReadAllLines(path).Where(s => s.Length == CountLetter);
            word = words.Skip(new Random().Next(0, words.Count() - 1)).First();
            chars = word.Select(s => new Char()).ToArray();
            chars[0].Character = word[0];
            chars[0].Unknown = false;
            chars[word.Length - 1].Character = word[word.Length - 1];
            chars[word.Length - 1].Unknown = false;
            WordStatusChanged?.Invoke(this, chars);
            ImageChanged?.Invoke(this, 0);

        }
        
        internal Char[] GetStartWord()
        {
            return chars;
        }

        internal void TryWord()
        {

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].Unknown &&
                    char.IsLetter(chars[i].Character))
                {
                    if (word[i] == chars[i].Character)
                    {
                        chars[i].Unknown = false;
                        WordStatusChanged?.Invoke(this, chars);
                        if (chars.FirstOrDefault(s => s.Unknown) == null)
                        {
                            Status = false;
                            CurrentMessage = "Молодец 3 папры делал";
                        }
                    }
                    else
                    {
                        errorCount++;
                        ImageChanged?.Invoke(this, errorCount);
                        if (errorCount == 6)
                        {
                            WordStatusChanged?.Invoke(this,
                                word.Select(c => new Char
                                {
                                    Character = c,
                                    Unknown = false
                                }).ToArray());
                            CurrentMessage = "Лошара 3 пары делал и так обосраться";
                            Status = false;
                        }
                    }
                    return;
                }
            }
        }
    }
}
