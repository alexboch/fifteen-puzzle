using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace Fifteen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int minIters = 5, maxIters = 25;
        Field _field = new Field(minIters,maxIters);//поле с костяшками
        
        public int numSteps
        {
            get { return (int)GetValue(numStepsProperty); }
            set { SetValue(numStepsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for numSteps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty numStepsProperty =
            DependencyProperty.Register("numSteps", typeof(int), typeof(Window), new PropertyMetadata(0));


        public Field field
        {
            get { return _field; }
        } 
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = field;
            
        }
        bool buttonIsSliding = false;//флаг перемещения костяшки
        public void OnCellMouseDown(object sender, RoutedEventArgs e)
        {
            if (!buttonIsSliding)//если ни одна костяшка не перемещается
            {
                var btn = sender as Button;
                Cell cell = btn.DataContext as Cell;
                if (cell!=null)
                {
                    Point dir = field.MovingDirection(cell);
                    if (dir.X != 0 || dir.Y != 0)//если есть куда перемещать
                    {
                        buttonIsSliding = true;
                        DoubleAnimation slideAnim = new DoubleAnimation();
                        Storyboard.SetTarget(slideAnim, btn);
                        slideAnim.Completed += SlideAnim_Completed;
                        slideAnim.SpeedRatio = 3;
                        btn.RenderTransform = new TranslateTransform(0, 0);
                        TranslateTransform tr = (TranslateTransform)btn.RenderTransform;
                        if (dir.X != 0)
                        {
                            slideAnim.To = btn.ActualWidth * dir.X;
                            tr.BeginAnimation(TranslateTransform.XProperty, slideAnim);
                        }
                        else if (dir.Y != 0)
                        {
                            slideAnim.To = btn.ActualHeight * dir.Y;
                            tr.BeginAnimation(TranslateTransform.YProperty, slideAnim);
                        }
                    }
                }
            }
        }



        private void SlideAnim_Completed(object sender, EventArgs e)
        {
            Button target = Storyboard.GetTarget(((sender as AnimationClock).Timeline as AnimationTimeline)) as Button;
            Cell animatedCell = (Cell)target.DataContext;
            buttonIsSliding = false;
            field.MoveCell(animatedCell);
            numSteps++;
            cellsGrid.Items.Refresh();
            if(field.isSolved)
            {
                MessageBox.Show(String.Format("Ходов:{0}", numSteps),"Победа!");
            }
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            _field = new Field(minIters, maxIters);
            this.DataContext = _field;
            cellsGrid.Items.Refresh();
            numSteps = 0;
        }
    }

}
