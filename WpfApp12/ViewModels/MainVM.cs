using Mvvm1125;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApp12.Models;

namespace WpfApp12.ViewModels
{
    public class MainVM : MvvmNotify
    {
        Game game;
        public string CurrentImage { get; set; }
        public IEnumerable<ViewChar> TryWord { get; set; }
        public MvvmCommand CommandTry { get; set; }
        public MvvmCommand CommandStart { get; set; }


        public int CountLetter { get => game.CountLetter; set => game.CountLetter = value; }

        public string Message
        {
            get => game.CurrentMessage;
        }

        public MainVM()
        {
            game = new Game();
            TryWord = game.GetStartWord().Select(s => new ViewChar(s));
            CommandTry = new MvvmCommand(
                () => game.TryWord(),
                () => game.Status);
            CommandStart = new MvvmCommand(
                () => game.StartGame(),
                () => !game.Status);
            game.ImageChanged += Game_ImageChanged;
            game.WordStatusChanged += Game_WordStatusChanged;
            game.CurrentMessageChanged += Game_CurrentMessageChanged;
        }

        private void Game_CurrentMessageChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Message");
        }

        private void Game_WordStatusChanged(object sender, Models.Char[] e)
        {
            TryWord = e.Select(s => new ViewChar(s));
            NotifyPropertyChanged("TryWord");

        }

        private void Game_ImageChanged(object sender, int e)
        {
            CurrentImage = $"/hangman/Hangman-{e}.png";
            NotifyPropertyChanged("CurrentImage");
        }
    }
}
