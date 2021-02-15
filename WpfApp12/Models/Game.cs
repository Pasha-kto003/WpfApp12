﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfApp12.Models
{
    public class Game
    {
        public event EventHandler<int> ImageChanged;
        public event EventHandler<Char[]> WordStatusChanged;
        string word;
        int errorCount = 0;
        Char[] chars;

        public bool Status { get; private set; }

       
        public void StartGame()
        {
            Status = true;
            string path = "word_rus.txt";
            var words = File.ReadAllLines(path).Where(s => s.Length >= 5 && s.Length <= 7);
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

        internal void TryWord(Char[] tryWord)
        {
            for (int i = 0; i < tryWord.Length; i++)
            {
                if (tryWord[i].Unknown && char.IsLetter(tryWord[i].Character))
                {
                    tryWord[i].Unknown = false;
                    WordStatusChanged?.Invoke(this, tryWord);
                    if(tryWord.FirstOrDefault(s=> s.Unknown) == null)
                    {
                        Status = false;
                        System.Windows.MessageBox.Show("Не ну русский язык знаешь молодец");
                    }
                }
                else
                {
                    errorCount++;
                    ImageChanged?.Invoke(this, errorCount);
                    if(errorCount == 6)
                    {
                        WordStatusChanged?.Invoke(this, word.Select(c=> new Char 
                        {
                            Character = c, Unknown = false
                        }).ToArray());
                        System.Windows.MessageBox.Show("Автор сам не знал какой ответ, так что все хорошо не печалься");
                        Status = false;
                        
                    }
                }
                return;
            }
        }
    }
}
