using Frontend.ViewModel;
using Frontend.Model;
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
using System.Windows.Shapes;
using Frontend.View;
using Frontend;

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        BoardViewModel boardViewModel;
        private BoardControllerViewModel viewModel;
        public BoardView(BoardModel board)
        {
            InitializeComponent();
            this.viewModel = new BoardControllerViewModel(board.user);
            this.boardViewModel = new BoardViewModel(board);
            this.DataContext = boardViewModel;
        }


        /// <summary>
        /// Log out button
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        /// <returns></returns>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Back to board view button
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        /// <returns></returns>
        private void Button_Click_listOfboards(object sender, RoutedEventArgs e)
        {
            BoardControllerView boardcontroller = new BoardControllerView(viewModel.user);
            boardcontroller.Show();
            this.Close();
        }
    }
}
