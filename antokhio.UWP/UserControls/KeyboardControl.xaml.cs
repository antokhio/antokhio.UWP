using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace antokhio.UWP.UserControls
{
    internal class KeyboardLayout
    {

        public string[] Default = new string[]
        {
              ". 1 2 3 4 5 6 7 8 9 0 - {backspace}",
              "{tab} й ц у к е н г ш щ з х ъ {delete}",
              "{enter} ф ы в а п р о л д ж э {enter}",
              "{shift} я ч с м и т ь б ю {shift}",
              "{space}"
        };

        public string[] Shift = new string[]
        {
            ". 1 2 3 4 5 6 7 8 9 0 - {backspace}",
              "{tab} Й Ц У К Е Н Г Ш Щ З Х Ъ {delete}",
              "{enter} Ф Ы В А П Р О Л Д Ж Э {enter}",
              "{shift} Я Ч С М И Т Ь Б Ю {shift}",
              "{space}"
        };

    }

    internal class KeyPressed : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public string Key { get; set; }

        public KeyboardControl KeyboardControl { get; set; }

        public KeyPressed(string key, KeyboardControl keyboardControl)
        {
            Key = key;
            KeyboardControl = keyboardControl;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Debug.WriteLine(Key);
            var element = FocusManager.GetFocusedElement();
            if (element != null)
            {
                if (element is TextBox)
                {
                    var tbox = element as TextBox;

                    // Input handling
                    switch (Key)
                    {
                        case "{backspace}":
                            {
                                var idx = tbox.SelectionStart;
                                if (idx > 0)
                                {
                                    Debug.WriteLine(idx.ToString());
                                    tbox.Text = tbox.Text.Remove(idx - 1);
                                    tbox.SelectionStart = tbox.Text.Length;
                                    tbox.SelectionLength = 0;
                                }

                                break;
                            }
                        case "{delete}":
                            {
                                tbox.Text = "";
                                tbox.SelectionStart = tbox.Text.Length;
                                tbox.SelectionLength = 0;

                                break;
                            }
                        case "{space}":
                            {
                                tbox.Text += " ";
                                tbox.SelectionStart = tbox.Text.Length;
                                tbox.SelectionLength = 0;

                                break;
                            }
                        case "{shift}":
                            {
                                KeyboardControl.SwitchLayout();
                                break;
                            }
                        case "{tab}":
                            {
                                break;
                            }
                        case "{enter}":
                            {
                                break;
                            }
                        default:
                            {
                                tbox.Text += Key;
                                tbox.SelectionStart = tbox.Text.Length;
                                tbox.SelectionLength = 0;
                                break;
                            }
                    }
                }
            }
        }
    }

    public sealed partial class KeyboardControl : UserControl
    {
        private List<Button> Buttons { get; set; }
        private KeyboardLayout Layout { get; set; }
        private bool SelectedKeyboardLayout = false;


        public void SwitchLayout()
        {
            SelectedKeyboardLayout = !SelectedKeyboardLayout;

            string[] keys;

            if (SelectedKeyboardLayout)
            {
                var allkeys = string.Join(" ", Layout.Shift);
                keys = allkeys.Split(" ");
            }
            else
            {
                var allkeys = string.Join(" ", Layout.Default);
                keys = allkeys.Split(" ");
            }


            for (int i = 0; i < Buttons.Count; i++)
            {
                switch (keys[i])
                {
                    case "{backspace}":
                    case "{delete}":
                    case "{space}":
                    case "{shift}":
                    case "{tab}":
                    case "{enter}":
                        break;
                    default:
                        {
                            var btn = Buttons[i];
                            btn.Name = keys[i];
                            btn.Content = keys[i];
                            btn.Command = new KeyPressed(btn.Name, this);
                            break;
                        }
                }

            }
            Debug.WriteLine("Shift finished");
        }

        public KeyboardControl()
        {
            this.InitializeComponent();

            Layout = new KeyboardLayout();
            Buttons = new List<Button>();

            var rowCount = Layout.Default.Count();

            var grid = new Grid();

            for (int i = 0; i < rowCount; i++)
            {
                var row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);

                grid.RowDefinitions.Add(row);
            }

            for (int i = 0; i < rowCount; i++)
            {
                var rowGrid = new Grid();
                var keys = Layout.Default[i].Split(" ");

                for (int j = 0; j < keys.Count(); j++)
                {
                    var column = new ColumnDefinition();
                    // Make control keys wider
                    var ln = keys[j].Length > 1 ? 2 : 1;
                    column.Width = new GridLength(ln, GridUnitType.Star);
                    rowGrid.ColumnDefinitions.Add(column);
                }

                for (int j = 0; j < keys.Count(); j++)
                {
                    var btn = new Button();
                    btn.Name = keys[j];

                    switch (keys[j])
                    {
                        case "{backspace}":
                            {
                                var stack = new StackPanel()
                                {
                                    Orientation = Orientation.Horizontal,
                                    Spacing = 10,
                                };
                                stack.Children.Add(new FontIcon()
                                {
                                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                                    Glyph = "\uE750"
                                });
                                stack.Children.Add(new TextBlock()
                                {
                                    Text = "Backspace"
                                });
                                btn.Content = stack;
                                break;
                            }
                        case "{delete}":
                            {
                                var stack = new StackPanel()
                                {
                                    Orientation = Orientation.Horizontal,
                                    Spacing = 10,
                                };
                                stack.Children.Add(new FontIcon()
                                {
                                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                                    Glyph = "\uE711"
                                });
                                stack.Children.Add(new TextBlock()
                                {
                                    Text = "Delete"
                                });
                                btn.Content = stack;
                                break;
                            }
                        case "{shift}":
                            {
                                var stack = new StackPanel()
                                {
                                    Orientation = Orientation.Horizontal,
                                    Spacing = 10,
                                };
                                stack.Children.Add(new FontIcon()
                                {
                                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                                    Glyph = "\uE752"
                                });
                                stack.Children.Add(new TextBlock()
                                {
                                    Text = "Shift"
                                });
                                btn.Content = stack;
                                break;
                            }
                        case "{tab}":
                            {
                                var stack = new StackPanel()
                                {
                                    Orientation = Orientation.Horizontal,
                                    Spacing = 10,
                                };
                                stack.Children.Add(new FontIcon()
                                {
                                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                                    Glyph = "\uE7FD"
                                });
                                stack.Children.Add(new TextBlock()
                                {
                                    Text = "Tab"
                                });
                                btn.Content = stack;
                                break;
                            }
                        case "{enter}":
                            {
                                var stack = new StackPanel()
                                {
                                    Orientation = Orientation.Horizontal,
                                    Spacing = 10,
                                };
                                stack.Children.Add(new FontIcon()
                                {
                                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                                    Glyph = "\uEA47"
                                });
                                stack.Children.Add(new TextBlock()
                                {
                                    Text = "Enter"
                                });
                                btn.Content = stack;
                                break;
                            }
                        case "{space}":
                            {
                                btn.Content = "Space";
                                break;
                            }
                        default:
                            {
                                btn.Content = keys[j];
                                break;
                            }

                    }

                    btn.AllowFocusOnInteraction = false;
                    btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                    btn.VerticalAlignment = VerticalAlignment.Stretch;

                    btn.Command = new KeyPressed(btn.Name, this);

                    Grid.SetColumn(btn, j);
                    rowGrid.Children.Add(btn);

                    //Add buttons to a list so we don't need to search for them after
                    Buttons.Add(btn);
                }

                Grid.SetRow(rowGrid, i);
                grid.Children.Add(rowGrid);

            }

            this.KeyboardGrid.Children.Add(grid);
        }
    }
}
