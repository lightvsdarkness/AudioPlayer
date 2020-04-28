using System;
using System.Collections.Generic;
using System.Text;
using AudioPlayer;
using Xamarin.Forms;

namespace AudioPlayer
{
    public class CustomViewCell : ViewCell
    {
        public static readonly BindableProperty SelectedItemBackgroundColorProperty =
            BindableProperty.Create("SelectedItemBackgroundColor",
                typeof(Color),
                typeof(CustomViewCell), Color.Default);

        public Color SelectedItemBackgroundColor
        {
            get => (Color) GetValue(SelectedItemBackgroundColorProperty);
            set => SetValue(SelectedItemBackgroundColorProperty, value);
        }
    }
}
