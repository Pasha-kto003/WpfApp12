using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp12.ViewModels
{
    public class ViewChar : Mvvm1125.MvvmNotify
    {
        WpfApp12.Models.Char @char;

        public ViewChar(Models.Char @char)
        {
            this.@char = @char;
        }

        public char Character
        {
            get => @char.Character;
            set => @char.Character = value;
        }

        public bool Unknown
        {
            get => @char.Unknown;
            set
            {
                @char.Unknown = value;
                NotifyPropertyChanged();
            }
        }
    }
}