using Mvvm1125;
using System;
using System.Collections.Generic;
using System.Text;
using WpfApp12.Models;

namespace WpfApp12.ViewModels
{
    public class MainVM : MvvmNotify
    {
        Game game;
        public string CurrentImage { get; set; }
        public MvvmCommand CommandTry { get; set; }
        public Models.Char[] TryWord { get; set; }
        
        public MvvmCommand CommandStart { get; set; }

        public MainVM()
        {
            game = new Game();
            TryWord = game.GetStartWord();
            CommandTry = new MvvmCommand(
                () => game.TryWord(TryWord), 
                () => game.Status);
            game.ImageChanged += Game_ImageChanged;
            game.WordStatusChanged += Game_WordStatusChanged;
            CommandStart = new MvvmCommand(() => game.StartGame(), () => !game.Status);
        }

        private void Game_WordStatusChanged(object sender, Models.Char[] e)
        {
            TryWord = e;
            NotifyPropertyChanged("TryWord");
        }

        private void Game_ImageChanged(object sender, int e)
        {
            CurrentImage = $"/hangman/Hangman-{e}.png";
            NotifyPropertyChanged("CurrentImage");
        }
    }
}
