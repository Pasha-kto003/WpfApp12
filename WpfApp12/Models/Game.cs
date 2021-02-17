﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfApp12.Models
{
    public class Game
    {
        public event EventHandler GameResult;
        public event EventHandler<int> ImageChanged;
        public event EventHandler<Char[]> WordStatusChanged;
        string word;
        int errorCount = 0;
        Char[] chars = new Char[0];

        public bool Status { get; private set; }


        public void StartGame()
        {
            Status = true;
            string path = "word_rus.txt";
            var words = File.ReadAllLines(path).Where(s => s.Length == 3);
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
                            System.Windows.MessageBox.Show("Ты крут");
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
                            System.Windows.MessageBox.Show("Вы проиграли");
                            Status = false;
                        }
                    }
                    return;
                }
            }
        }
    }
}
